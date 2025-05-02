using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace practicarUNI.Juego.padres
{
    public class Enemigo
    {
        public bool EnemigoDerrotado= false;
        public bool EstaDefendiendose { get; set; } = false;
        protected double Enemigo_VidaToatal{get; set;}
        protected double Enemigo_VidaActual{get; set;}
        protected double Enemigo_Daño{get; set;}
        protected double Enemigo_Defensa{get; set;}

        protected string Enemigo_Nombre{get; set;}
        public string Nombre => Enemigo_Nombre;      //estas simplemente estan declaradas para que sean accesibles desde funciones de gestion
        public double VidaActual => Enemigo_VidaActual;  // lo mismo con esta


        public int Enemigo_Nivel{get; set;}
        public int Nivel => Enemigo_Nivel;
        
        
        protected int Enemigo_ExpDrop{get; set;}

        public List<Enemigo> ListaEnemigos = new List<Enemigo>();
        public Dictionary<string, Action<Personaje>> Acciones_Enemigo{get; set;} //misma logica que con el diccionario del jugador
        


        public Enemigo(string nombre , double VidaInicial, double DañoInicial, double DefensaInicial)
        {
            Enemigo_Nombre=nombre;
            Enemigo_VidaToatal=VidaInicial;
            Enemigo_VidaActual = VidaInicial;
            Enemigo_Defensa=DefensaInicial;
            ListaEnemigos.Add(this); //agregas las clases que herende de padre

            Acciones_Enemigo = new Dictionary<string, Action<Personaje>>();  //lo instanciamos para que puda ser modificado segun sub-clases
        }


        public virtual void ElegirAccion_Enemigo(Personaje personaje, List<Enemigo>enemigos)
        {
         
        }    


        protected void SubirNivelEnemigo(int Jugador_Nivel)
        {
            int minimo = Jugador_Nivel - 5;
            if (minimo < 1) minimo = 1;

            int maximo = Jugador_Nivel + 5;

            Random random = new Random();
            Enemigo_Nivel = random.Next(minimo, maximo + 1); 

            Enemigo_ExpDrop = CalcularExpDrop();
        }

        protected void DropXP(Personaje personaje)
        {
        personaje.RecibirXP(Enemigo_ExpDrop);
        Console.WriteLine($"El Enemigo solto {Enemigo_ExpDrop} puntos de experiencia!");
        }
        protected virtual int CalcularExpDrop()
        {
            return 25 + (Enemigo_Nivel * 5);
        }

        protected void EscaladoNivel_Enemigo()
        {
            Enemigo_Daño+= Enemigo_Nivel*(10);
            Enemigo_VidaToatal+= Enemigo_Nivel*(20);
            Enemigo_VidaActual+= Enemigo_Nivel*(20);
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
        

        public void RecibirDaño(double Jugador_Daño, Personaje personaje)
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

            if (Enemigo_VidaActual <= 0 && !EnemigoDerrotado)
            {
                EnemigoDerrotado = true;
                Console.WriteLine("¡¡Enemigo Derrotado!!");
                DropXP(personaje); 
            }
        }

        public virtual void EnemigoSeCura()
        {
            double curacion = 20+(5*Enemigo_Nivel);
            Enemigo_VidaActual+= curacion;
            Console.WriteLine($"El enemigo se curo un total de {curacion} y ahora posee {Enemigo_VidaActual} puntos de vida.");
        }
        

    }

    
    //me falta independizar cada subclase(tal como realize con las clases de personaje) (ya casi esta)







    public class Zombie: Enemigo
    {

        public static(double vida, double daño, double defensa) Estadisticasbase{get;} =(100.0, 40.0, 0);

        public Zombie(int Jugador_Nivel): base("Zombie" , Estadisticasbase.vida, Estadisticasbase.daño, Estadisticasbase.defensa)
        {
            SubirNivelEnemigo(Jugador_Nivel);
            EscaladoNivel_Enemigo();
            Enemigo_ExpDrop = CalcularExpDrop();
        }
        public override void Atacar(Personaje personaje)
        {
            Console.WriteLine("El Enemigo realizo un ataque!");
            personaje.RecibirDaño(Enemigo_Daño, this);
        }
        public override void ElegirAccion_Enemigo(Personaje personaje, List<Enemigo> enemigos)
        {
         Atacar(personaje);
        }
    }






    public class Humano: Enemigo
    {
        private int cargasCuracionEnemigo=0;
        private int cargasParaLlamarAliado=1;

        public static(double vida, double daño, double defensa) Estadisticasbase{get;} =(100.0, 40.0, 20.0);
        public Humano(int Jugador_Nivel): base("humano" , Estadisticasbase.vida, Estadisticasbase.daño, Estadisticasbase.defensa)
        {
            SubirNivelEnemigo(Jugador_Nivel);
            EscaladoNivel_Enemigo();
            Enemigo_ExpDrop = CalcularExpDrop();
        }
        public override void Atacar(Personaje personaje)
        {
            Console.WriteLine("El Enemigo realizo un ataque!");
            personaje.RecibirDaño(Enemigo_Daño, this);
        }

        public override void DefenderceEnemigo()
        {
            Console.WriteLine("El enemigo se prepara para recibir menos daño.");
            EstaDefendiendose = true;
        }

        public override void ElegirAccion_Enemigo(Personaje personaje, List<Enemigo> enemigos)
        {
            bool NoMasCuras = cargasCuracionEnemigo >= 2;
            bool NoMasAliados = cargasParaLlamarAliado < 1;

            
            if (Enemigo_VidaActual <= Enemigo_VidaToatal * 0.50 && !NoMasCuras)
            {
                EnemigoSeCura();
                cargasCuracionEnemigo++;
            }
            
            else if (Enemigo_VidaActual <= Enemigo_VidaToatal * 0.25 && !NoMasAliados)
            {
                LLamarAliado(enemigos, personaje.Nivel);
                cargasParaLlamarAliado--;
            }
            
            else if (Enemigo_VidaActual <= Enemigo_VidaToatal * 0.50 && NoMasCuras && NoMasAliados)
            {
                DefenderceEnemigo();
            }
            
            else
            {
                Atacar(personaje);
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
        private int cargasCuracionEnemigo=0;
        private int cargasParaLlamarAliado=1;

        public static(double vida, double daño, double defensa) Estadisticasbase{get;} =(50.0, 200.0, 0);

        public Elemental(int Jugador_Nivel): base("Elemental" , Estadisticasbase.vida, Estadisticasbase.daño, Estadisticasbase.defensa)
        {
            SubirNivelEnemigo(Jugador_Nivel);
            EscaladoNivel_Enemigo();
            Enemigo_ExpDrop = CalcularExpDrop();
        }
        public override void Atacar(Personaje personaje)
        {
            Console.WriteLine("El Enemigo realizo un ataque!");
            personaje.RecibirDaño(Enemigo_Daño, this);
        }
        public override void ElegirAccion_Enemigo(Personaje personaje, List<Enemigo> enemigos)
        {
            bool NoMasCuras = cargasCuracionEnemigo>=2;
            bool NoMasAliados = cargasParaLlamarAliado<1;


            
            if(Enemigo_VidaActual <= Enemigo_VidaToatal*0.50 && !NoMasCuras)
            {
                EnemigoSeCura();
                cargasCuracionEnemigo++;
            }
            else
            {
                Atacar(personaje);
            }
            
            
        }
    }
}