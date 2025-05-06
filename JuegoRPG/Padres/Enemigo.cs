using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using practicarUNI.JuegoRPG.Padres;

namespace practicarUNI.Juego.padres
{
    public abstract class Enemigo
    {
        public bool EnemigoDerrotado= false;
        public bool EstaDefendiendose { get; set; } = false;
        public bool DropItem { get; set; }
        public bool TieneItem { get; set; }


        protected double Enemigo_VidaToatal{get; set;}
        protected double Enemigo_VidaActual{get; set;}
        protected double Enemigo_Daño{get; set;}
        protected double Enemigo_Defensa{get; set;}
        public double VidaActual => Enemigo_VidaActual;  // lo mismo con esta



        protected string Enemigo_Nombre{get; set;}
        public string Nombre => Enemigo_Nombre;      //estas simplemente estan declaradas para que sean accesibles desde funciones de gestion


        public int Enemigo_Nivel{get; set;}
        public int Nivel => Enemigo_Nivel;
        protected int Enemigo_ExpDrop{get; set;}



        public List<Enemigo> ListaEnemigos = new List<Enemigo>();
        public List<Items> ListaItemsEnemigos = new List<Items>();
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


        protected void DropItemEnemigo()
        {
            if (DropItem)
            {
                Console.WriteLine($"El enemigo dejo caer un item: {ListaItemsEnemigos[0].Nombre}");
                TieneItem = true;
            }
            else
            {
                Console.WriteLine("El enemigo no ha dejado caer nada.");
                TieneItem = false;
            }
        }
        

    }

    //tengo que crear las pools de items para cada clase de enemigo y despues crear los items... o mejor al reves jajajja
}// aparte de eso voy a volver a las clases de humano a humanoide elemental la dejo igual y zombie a no-muerto para crear mas diverisdad de enemigos