using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace practicarUNI.Juego.padres
{
    public class Enemigo
    {
        public bool EnemigoDerrotado=>Enemigo_VidaToatal<=0;
        public bool EstaDefendiendose { get; set; } = false;
        protected double Enemigo_VidaToatal{get; set;}
        protected double Enemigo_VidaActual{get; set;}
        protected int Enemigo_Daño{get; set;}
        protected double Enemigo_Defensa{get; set;}

        protected string Enemigo_Nombre{get; set;}
        public string Nombre => Enemigo_Nombre;      //estas simplemente estan declaradas para que sean accesibles desde funciones de gestion
        public double VidaActual => Enemigo_VidaToatal;  // lo mismo con esta


        int Enemigo_Nivel{get; set;}
        protected int Enemigo_ExpDrop{get; set;}

        public List<Enemigo> ListaEnemigos = new List<Enemigo>();
        public Dictionary<string, Action<Personaje>> Acciones_Enemigo{get; set;} //misma logica que con el diccionario del jugador
        


        public Enemigo(string nombre , int VidaInicial, int DañoInicial, int DefensaInicial)
        {
            Enemigo_Nombre=nombre;
            Enemigo_VidaToatal=VidaInicial;
            Enemigo_Daño=DañoInicial;
            Enemigo_Defensa=DefensaInicial;
            ListaEnemigos.Add(this); //agregas las clases que herende de padre

            Acciones_Enemigo = new Dictionary<string, Action<Personaje>>();  //lo instanciamos para que puda ser modificado segun sub-clases
        }


        public virtual void ElegirAccion_Enemigo(Personaje personaje, List<Enemigo>enemigos)
        {
            /*
         if(Acciones_Enemigo.ContainsKey("Atacar"))
        {
            Acciones_Enemigo["Atacar"](personaje);
        }
            */
        }    


        protected void SubirNivelEnemigo(int Jugador_Nivel)
            {
                int minimo = Jugador_Nivel - 10;
                if (minimo < 1) minimo = 1;

                int maximo = Jugador_Nivel + 10;

                Random random = new Random();
                Enemigo_Nivel = random.Next(minimo, maximo + 1); 

                Enemigo_ExpDrop = CalcularExpDrop();
            }

        protected void DropXP(Personaje personaje)
            {
                if(EnemigoDerrotado)
                {
                    personaje.REcibirXP(true, Enemigo_ExpDrop);
                    Console.WriteLine($"El Enemigo solto {Enemigo_ExpDrop} puntos de experiencia!");
                }
            }
        protected virtual int CalcularExpDrop()
            {
                return 25 + (Enemigo_Nivel * 5);
            }

        protected void EscaladoNivel_Enemigo()
            {
                Enemigo_Daño+= Enemigo_Nivel*(2);
                Enemigo_VidaToatal+= Enemigo_Nivel*(10);
                Enemigo_Defensa+= Enemigo_Nivel*(1.25);
            }

        public virtual void Atacar(Personaje personaje)
        {
            //que la modifique cada subclase 
            //ya que no puedo volver abstracta la clase de enemigo ya que se pierden los accesos a los metodos de logica y ataque 
        }
        public virtual void DefenderceEnemigo()
        {

        }
        

        public void RecibirDaño(int Jugador_Daño)
        {
            double DañoMitigado= Jugador_Daño;

            if(EstaDefendiendose)
            {
                DañoMitigado*=0.7;
                EstaDefendiendose = false;
                Console.WriteLine("El enemigo Mitigo parte del daño recibido");
            }

            double DañoFinal = Math.Max(DañoMitigado - Enemigo_Defensa,0);
            Enemigo_VidaActual -= DañoFinal;

            Console.WriteLine($"El enemigo recibio {DañoFinal} de daño. Vida restante: {Enemigo_VidaActual}");
        }

        public virtual void EnemigoSeCura()
        {
            double curacion = 50+(5*Enemigo_Nivel);
            Enemigo_VidaActual+= curacion;
            Console.WriteLine($"El enemigo se curo un total de {curacion} y ahora posee {Enemigo_VidaActual} puntos de vida.");
        }

    }

    //me falta crear el diccionary con las acciones de cada clase hija
    //me falta independizar cada subclase(tal como realize con las clases de personaje) (ya casi esta)







    public class Zombie: Enemigo
    {
        public static(int vida, int daño, int defensa) Estadisticasbase{get;} =(100,10,0);
        public Zombie(int Jugador_Nivel): base("Zombie" , Estadisticasbase.vida, Estadisticasbase.daño, Estadisticasbase.defensa)
        {
            SubirNivelEnemigo(Jugador_Nivel);
            EscaladoNivel_Enemigo();
            Enemigo_ExpDrop = CalcularExpDrop();
        }
        public override void Atacar(Personaje personaje)
        {
            Console.WriteLine("El Enemigo realizo un ataque!");
            personaje.RecibirDaño(Enemigo_Daño);
        }
    }






    public class Humano: Enemigo
    {
        private int cargasCuracionEnemigo=0;
        private int cargasParaLlamarAliado=1;

        public static(int vida, int daño, int defensa) Estadisticasbase{get;} =(100,20,20);
        public Humano(int Jugador_Nivel): base("humano" , Estadisticasbase.vida, Estadisticasbase.daño, Estadisticasbase.defensa)
        {
            SubirNivelEnemigo(Jugador_Nivel);
            EscaladoNivel_Enemigo();
            Enemigo_ExpDrop = CalcularExpDrop();
        }
        public override void Atacar(Personaje personaje)
        {
            Console.WriteLine("El Enemigo realizo un ataque!");
            personaje.RecibirDaño(Enemigo_Daño);
        }

        public override void DefenderceEnemigo()
        {
            Console.WriteLine("El enemigo se prepara para recibir menos daño.");
            EstaDefendiendose = true;
        }

        public override void ElegirAccion_Enemigo(Personaje personaje, List<Enemigo> enemigos)
        {
            bool NoMasCuras = cargasCuracionEnemigo>=2;
            bool NoMasAliados = cargasParaLlamarAliado<1;


            if(Enemigo_VidaActual> Enemigo_VidaToatal*0.50 || NoMasCuras || NoMasAliados)
            {
                Atacar(personaje);
            }
            else if(Enemigo_VidaActual <= Enemigo_VidaToatal*0.50 && !NoMasCuras)
            {
                EnemigoSeCura();
                cargasCuracionEnemigo++;
            }
            else if(Enemigo_VidaActual <= Enemigo_VidaToatal*0.50 && NoMasCuras)
            {
                DefenderceEnemigo();
            }
            else if(Enemigo_VidaActual == Enemigo_VidaToatal*0.25 && NoMasCuras && NoMasAliados)
            {
                LLamarAliado(enemigos, personaje.Nivel);
                cargasParaLlamarAliado--;
            }
        }


        public void LLamarAliado(List<Enemigo> enemigos ,int Jugador_Nivel)
        {
            Console.WriteLine("¡¡El enemigo está llamando refuerzos!!");

            Enemigo nuevoAliado = GeneradorDeEnemigos.GenerarEnemigoAleatorio(Jugador_Nivel);
            enemigos.Add(nuevoAliado);
        
            Console.WriteLine("Un nuevo humano se ha unido al combate.");
            
        }
    }









    public class Elemental: Enemigo
    {
        public static(int vida, int daño, int defensa) Estadisticasbase{get;} =(50,100,0);
        public Elemental(int Jugador_Nivel): base("Elemental" , Estadisticasbase.vida, Estadisticasbase.daño, Estadisticasbase.defensa)
        {
            SubirNivelEnemigo(Jugador_Nivel);
            EscaladoNivel_Enemigo();
            Enemigo_ExpDrop = CalcularExpDrop();
        }
        public override void Atacar(Personaje personaje)
        {
            Console.WriteLine("El Enemigo realizo un ataque!");
            personaje.RecibirDaño(Enemigo_Daño);
        }
    }
}