using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego.padres;

namespace practicarUNI.Juego.TipoDeEnemigos
{
    public abstract class Humano: Enemigo
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
}