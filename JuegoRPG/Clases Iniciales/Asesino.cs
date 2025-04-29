using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego.padres;

namespace practicarUNI.JuegoRPG.Clases_Iniciales
{
    public class Asesino:Personaje
    {
        public static(int vida, int daño, int defensa) Estadisticasbase{get;} =(150,40,20);
        public Asesino(): base("Asesino" , Estadisticasbase.vida, Estadisticasbase.daño, Estadisticasbase.defensa)
        {
            Acciones = new Dictionary<string, Action<Enemigo>>();

            Acciones.Add("Atacar", enemigo => Atacar(enemigo)); 
            Acciones.Add("Curarse", enemigo => Curarse());
            Acciones.Add("Defenderse", enemigo => Defenderse());
        }

        protected override void Atacar(Enemigo enemigo)
        {
            enemigo.RecibirDaño(Jugador_Daño);
        }
        protected override void Defenderse()
        {
            
        }
        protected override void Curarse()
        {
            Jugador_VidaToatal += 30;
            Console.WriteLine("Te has curado 30 puntos de vida.");
        }

        public static void MostrarEstadisticasBase()
        {
        Console.WriteLine($"El Asesiono posee: - Vida: {Estadisticasbase.vida}, Daño: {Estadisticasbase.daño}, Defensa: {Estadisticasbase.defensa}");
        }
    }
}


//tengo que modificar toda la clase ya que solo trabaje con caballero