using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego.padres;

namespace practicarUNI.Juego
{
    public class GeneradorDeEnemigos
    {
        public static List<Type> tiposDeEnemigos = new List<Type>();
        private static Random random= new Random();

        static GeneradorDeEnemigos()
        {
            tiposDeEnemigos.Add(typeof(Zombie));
            tiposDeEnemigos.Add(typeof(Humano));
            tiposDeEnemigos.Add(typeof(Elemental));
        }

        public static Enemigo GenerarEnemigoAleatorio(int Jugador_Nivel)
        {
            if(tiposDeEnemigos.Count==0)
            {
                throw new InvalidOperationException("No se pudieron generar enemigos por algun error desconocido");
            }

            int indiceAleatorio = random.Next(tiposDeEnemigos.Count);
            Type tipoSeleccionado= tiposDeEnemigos[indiceAleatorio];

            Enemigo enemigoGenerado = (Enemigo)Activator.CreateInstance(tipoSeleccionado, Jugador_Nivel)!;
            return enemigoGenerado;
        }

    }
}