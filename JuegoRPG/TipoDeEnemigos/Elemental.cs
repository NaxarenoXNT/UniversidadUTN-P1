using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego.padres;


namespace practicarUNI.Juego.TipoDeEnemigos
{
    public abstract class Elemental: Enemigo
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