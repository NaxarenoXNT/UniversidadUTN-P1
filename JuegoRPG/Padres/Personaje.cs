using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego;

namespace practicarUNI.Juego.padres
{
    public abstract class Personaje
    {
        public bool JugadorDerrotado=>Jugador_VidaToatal<=0;

        public string Nombre{get;}
        protected int Jugador_VidaToatal{get; set;}
        protected int Jugador_Daño{get; set;}
        protected int Jugador_Defensa{get; set;}

        protected int Jugador_Nivel{get; set;}
        protected int Jugador_Progreso_Nivel{get; set;}
        public int Nivel => Jugador_Nivel;   //para que pueda ser accedido desde el inicio

        public Dictionary<string,Action<Enemigo>> Acciones{get; set;}= new Dictionary<string, Action<Enemigo>>(); //basicamente guarda un string(nombre del metodo) y recibe un enmigo para ejecutar un metodo
        

        public Personaje(string nombre ,int VidaInicial, int DañoInicial, int DefensaInicial)
        {
            Nombre=nombre;    
            Jugador_VidaToatal = VidaInicial;
            Jugador_Daño = DañoInicial;
            Jugador_Defensa = DefensaInicial;
            Jugador_Nivel = 1;
            Jugador_Progreso_Nivel = 0;
        }

        public virtual void MostrarStats()
        {
            Console.Write($"Sus estadisticas son: {Jugador_VidaToatal} puntos de vida, {Jugador_Daño} puntos de daño, {Jugador_Defensa} puntos de defensa");
            Console.WriteLine($"");
        }


        protected void EscaladoNivel_Jugador(int Jugador_Nivel)
        {
            Jugador_Daño+= Jugador_Nivel*(10);
            Jugador_VidaToatal+= Jugador_Nivel*(120);
            Jugador_Defensa+= Jugador_Nivel*(2);
        }
        public void SubirNivel(int Enemigo_ExpDrop)
        {
            int TopeEXP_Jugador=100+(Jugador_Nivel*25);

            Jugador_Progreso_Nivel+= Enemigo_ExpDrop;
            
            while(Jugador_Progreso_Nivel>=TopeEXP_Jugador)
            {
                Jugador_Progreso_Nivel-=TopeEXP_Jugador;
                Jugador_Nivel++;
                EscaladoNivel_Jugador(Jugador_Nivel);
                TopeEXP_Jugador=100+(Jugador_Nivel*25);
            }
        }
        public void REcibirXP(bool EnemigoDerrotado, int Enemigo_ExpDrop)
        {
            if (EnemigoDerrotado)
            {
                SubirNivel(Enemigo_ExpDrop);
            }
        }
        
        public void RecibirDaño_deEnemigo(int Enemigo_Daño, string Enemigo_Nombre)
        {
        int DañoFinal = Math.Max(Enemigo_Daño - Jugador_Defensa,0);
        Jugador_VidaToatal -= DañoFinal;
        Console.WriteLine($"El {Enemigo_Nombre} te infligio {DañoFinal} de daño. Vida restante: {Jugador_VidaToatal}");
        }

        protected abstract void Atacar(Enemigo enemigo);
        protected abstract void Defenderse();
        protected abstract void Curarse();
        
        

        public void RecibirDaño(int Enemigo_Daño)
        {
        int DañoFinal = Math.Max(Enemigo_Daño - Jugador_Defensa,0);
        Jugador_VidaToatal -= DañoFinal;
        Console.WriteLine($"El enemigo recibio {DañoFinal} de daño. Vida restante: {Jugador_VidaToatal}");
        }

    }
    
}