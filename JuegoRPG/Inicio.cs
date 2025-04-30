using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego.padres;
using practicarUNI.JuegoRPG.Clases_Iniciales;

namespace practicarUNI.Juego
{
    public class Inicio
    {
        //me tengo que acordar de que si agrego mas clases tengo que ingresarlas manualamente :)
        //que podria hacer un metodo y dejarlo mas limpio... si peeero lo dejo para mi yo del futuro
        public Personaje EleccionDePersonaje()
        {
            Console.WriteLine("Buenas por favor elige tu clase.");
            Console.Write("Usted tiene para escoger tres clases base: ");
            Console.WriteLine("--------------------------------------------");

            Console.Write("Presione 1 para seleccionar al: ");
            Caballero.MostrarEstadisticasBase();
            Console.WriteLine();

            Console.Write("Presione 2 para seleccionar al: ");
            Mago.MostrarEstadisticasBase();
            Console.WriteLine();


            Console.Write("Presione 3 para seleccionar al: ");
            Asesino.MostrarEstadisticasBase();
            Console.WriteLine();


            int eleccionPrincipal=int.Parse (Console.ReadLine()??"1");


            Personaje jugador;  

            switch(eleccionPrincipal)
            {
                case 1:
                    jugador=new Caballero();
                    break;
                case 2:
                    jugador=new Mago();
                    break;
                case 3:
                    jugador=new Asesino();
                    break;
                default:
                    Console.WriteLine("Opcion invalida. Se asigna Caballero por defecto");
                    jugador = new Caballero();
                    break;
            }
            if(jugador!= null)
            {
                Console.WriteLine($"Has elegido la clase de: {jugador.Nombre}");
            }
            return jugador;
        }
        
    }

    //corregir el tema de la el protected del diccionary de acciones para que se pueda acceder desde este metodo

    public class Logica
    {
        public void AccionesCombate(Personaje personaje, List<Enemigo> enemigos)
        {
            Console.WriteLine("¿Qué acción quiere tomar?");
            
            int opcion = 1;
            List<string> listaAcciones = new List<string>();
            Dictionary<int, string> mapaOpciones = new Dictionary<int, string>();
            
            foreach (var accion in personaje.Acciones.Keys)
            {
                Console.WriteLine($"{opcion}. {accion}");
                mapaOpciones.Add(opcion, accion + " Objetivo Requerido");
                listaAcciones.Add(accion);
                opcion++;
            }

            foreach (var accion in personaje.AccionesSinObjetivo.Keys)
            {
                Console.WriteLine($"{opcion}. {accion}");
                mapaOpciones.Add(opcion, accion + " (A Ti Mismo)");
                listaAcciones.Add(accion);
                opcion++;
            }

            Console.Write("Elija su opción: ");
            string input = Console.ReadLine() ?? "1";
            int eleccion;

            if (int.TryParse(input, out eleccion) && mapaOpciones.ContainsKey(eleccion))
            {
                string accionCompuesta = mapaOpciones[eleccion];

                if (accionCompuesta.EndsWith(" Objetivo Requerido"))
                {
                    string nombreAccion = accionCompuesta.Replace(" Objetivo Requerido", "");
                    Action<Enemigo> accionSeleccionada = personaje.Acciones[nombreAccion];

                    Console.WriteLine("¿A qué enemigo quieres atacar?");
                    for (int i = 0; i < enemigos.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {enemigos[i].Nombre} (HP: {enemigos[i].VidaActual})");
                    }

                    string inputEnemigo = Console.ReadLine() ?? "1";
                    int eleccionEnemigo;

                    if (int.TryParse(inputEnemigo, out eleccionEnemigo) && eleccionEnemigo >= 1 && eleccionEnemigo <= enemigos.Count)
                    {
                        accionSeleccionada(enemigos[eleccionEnemigo - 1]);
                    }
                    else
                    {
                        Console.WriteLine("Enemigo inválido, turno perdido.");
                    }
                }
                else if (accionCompuesta.EndsWith(" (A Ti Mismo)"))
                {
                    string nombreAccion = accionCompuesta.Replace(" (A Ti Mismo)", "");
                    Action accionSeleccionada = personaje.AccionesSinObjetivo[nombreAccion];
                    accionSeleccionada();
                }
            }
            else
            {
                Console.WriteLine("Acción inválida. Turno perdido.");
            }
        }





        public void AccionesCombateEnemigo(List<Enemigo> enemigos, Personaje personaje)
        {
            foreach (var enemigo in enemigos)
            {
                enemigo.ElegirAccion_Enemigo(personaje, enemigos);
            }
        }





        public void GestionTurnos(Personaje personaje, List<Enemigo>enemigos)
        {
            while(!personaje.JugadorDerrotado && enemigos.Count>0)
            {
                Console.WriteLine("--- Tu Turno ---");
                AccionesCombate(personaje, enemigos);

                enemigos.RemoveAll(e => e.EnemigoDerrotado);

                if(enemigos.Count == 0)
                {
                    Console.WriteLine("¡¡Enemigo Derrotado!!");
                    break;
                }

                Console.WriteLine("--- Turno De los Enemigos ---");
                foreach(var enemigo in enemigos)
                {
                    AccionesCombateEnemigo(enemigos, personaje); 
                }

                if(personaje.JugadorDerrotado)
                {
                    Console.WriteLine("¡Fuiste Derrotado!");
                    break;
                }
                
            }
        }
    }
    
    class ComienzoJuego
    {
        Inicio inicio = new Inicio();
        Logica logica = new Logica();
        
        public void Inicia()
        {
            Console.WriteLine("Bienvenido al juego");

            Personaje personaje = inicio.EleccionDePersonaje();

            // 2. Generar lista de enemigos
            List<Enemigo> enemigos = new List<Enemigo>();
            for (int i = 0; i < 1; i++)   // Podés cambiar la cantidad de enemigos iniciales
            {
                enemigos.Add(GeneradorDeEnemigos.GenerarEnemigoAleatorio(personaje.Nivel));
            }

            // 3. Empezar la gestión de turnos
            logica.GestionTurnos(personaje, enemigos);

            // 4. Fin del combate (opcionalmente podrías mostrar mensaje final acá)
            if (personaje.JugadorDerrotado)
            {
                Console.WriteLine("¡Has derrotado a todos los enemigos!");
            }
            else
            {
                Console.WriteLine("Has sido derrotado...");
            }
        }

            
    }
    
}