using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego.padres;

namespace practicarUNI.JuegoRPG.Clases_Iniciales
{
    public class Mago:Personaje
    {
        public static(int vida, int daño, int defensa) Estadisticasbase{get;} =(100,50,10);
        public Mago(): base("Mago", Estadisticasbase.vida, Estadisticasbase.daño, Estadisticasbase.defensa)
        {
            Acciones = new Dictionary<string, Action<Enemigo>>();
            AccionesSinObjetivo = new Dictionary<string, Action>();

            Acciones.Add("Atacar", enemigo => Atacar(enemigo)); 

            AccionesSinObjetivo.Add("Curarse", () => Curarse());
            AccionesSinObjetivo.Add("Defenderse", () => Defenderse());
        }

        protected override void Atacar(Enemigo enemigo)
        {
            enemigo.RecibirDaño(Jugador_Daño, this);
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
        Console.WriteLine($"El Mago posee: - Vida: {Estadisticasbase.vida}, Daño: {Estadisticasbase.daño}, Defensa: {Estadisticasbase.defensa}");
        }
    }
}

//tengo que modificar toda la clase ya que solo trabaje con caballero