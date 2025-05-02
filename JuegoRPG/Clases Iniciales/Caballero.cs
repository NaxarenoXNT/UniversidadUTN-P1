using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego.padres;

namespace practicarUNI.JuegoRPG.Clases_Iniciales
{
    public class Caballero:Personaje
    {
        public static(double vida, double daño, double defensa) Estadisticasbase{get;} =(250.0, 40.0, 40.0);
        public Caballero(): base("Caballero" , Estadisticasbase.vida, Estadisticasbase.daño, Estadisticasbase.defensa)
        {
            Acciones = new Dictionary<string, Action<Enemigo>>();
            AccionesSinObjetivo = new Dictionary<string, Action>();

            Acciones.Add("Atacar", enemigo => Atacar(enemigo)); 

            AccionesSinObjetivo.Add("Curarse", () => Curarse());
            AccionesSinObjetivo.Add("Defenderse", () => Defenderse());

            //RecibirDaño(Enemigo_Daño); // en caso de no recibir daño tengo que sacar esto y el double del constructor
        }

        protected override void Atacar(Enemigo enemigo)
        {
            enemigo.RecibirDaño(Jugador_Daño, this);
        }
        protected override void Defenderse()
        {
            DefenderceDeEnemigo = true;
            Console.WriteLine("Te Defiendes del siguiente ataque.");
        }
        protected override void Curarse()
        {
            Jugador_VidaToatal += 30;
            Console.WriteLine("Te has curado 30 puntos de vida.");
        }
        
        public static void MostrarEstadisticasBase()
        {
        Console.WriteLine($"El Caballero posee: - Vida: {Estadisticasbase.vida}, Daño: {Estadisticasbase.daño}, Defensa: {Estadisticasbase.defensa}");
        }

        protected override void EscaladoNivel_Jugador(int Jugador_Nivel)
        {
            Jugador_Daño+= Jugador_Nivel*(10);
            Jugador_VidaToatal+= Jugador_Nivel*(150);
            Jugador_Defensa+= Jugador_Nivel*(3);
        }
        protected override void InicializarModificadoresEstado()
        {
            base.InicializarModificadoresEstado();
            //aca tendria que ir agregando que estadisticas se pueden cambiar con los eventos
        }
    }
}