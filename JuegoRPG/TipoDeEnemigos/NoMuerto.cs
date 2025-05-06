using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego.padres;

namespace practicarUNI.Juego.TipoDeEnemigos
{
    public abstract class NoMuerto: Enemigo
    {

        public static(double vida, double daño, double defensa) Estadisticasbase{get;} =(100.0, 40.0, 0);

        public NoMuerto(int Jugador_Nivel): base("Zombie" , Estadisticasbase.vida, Estadisticasbase.daño, Estadisticasbase.defensa)
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
}