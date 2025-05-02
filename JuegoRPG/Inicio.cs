using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego.padres;
using practicarUNI.JuegoRPG.Clases_Iniciales;
using practicarUNI.Eventos;
using practicarUNI.JuegoRPG.TodoEventos;

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

    

    public class Logica
    {
        public void AccionesCombate(Personaje personaje, List<Enemigo> enemigos)
        {

            Console.WriteLine();
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
                        Console.WriteLine($"{i + 1}. {enemigos[i].Nombre}, (Nivel: {enemigos[i].Nivel}) - (HP: {enemigos[i].VidaActual})");
                    }

                    string inputEnemigo = Console.ReadLine() ?? "1";
                    int eleccionEnemigo;

                    if (int.TryParse(inputEnemigo, out eleccionEnemigo) && eleccionEnemigo >= 1 && eleccionEnemigo <= enemigos.Count)
                    {
                        Enemigo objetivo = enemigos[eleccionEnemigo - 1];
                        accionSeleccionada(objetivo);

                        if(objetivo.VidaActual <= 0)
                        {
                            Console.WriteLine($"{objetivo.Nombre} ha muerto y fue eliminado del campo de batalla.");
                            enemigos.Remove(objetivo);
                        }
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
            foreach (var enemigo in enemigos.ToList()) 
            {
                enemigo.ElegirAccion_Enemigo(personaje, enemigos);
                
                
                if (personaje.JugadorDerrotado)
                {
                    break; 
                }
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
                    Console.WriteLine("¡¡Enemigos Derrotado!!");
                    break;
                }

                Console.WriteLine("--- Turno De los Enemigos ---");
                AccionesCombateEnemigo(enemigos, personaje);         
                

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
        private readonly Inicio inicio = new Inicio();
        private readonly Logica logica = new Logica();
        
        public void Inicia()
        {
            Console.WriteLine("Bienvenido al juego");

            try
            {
                // Seleccion de personaje
                Personaje personaje = inicio.EleccionDePersonaje();
                
                // Creacion del estado del juego
                EstadoJuego estado = new EstadoJuego(personaje);

                // Creacion del gestor de eventos
                GestorEventos gestor = new GestorEventos(estado, logica);
                
                // Determinar el evento inicial basado en la clase del personaje
                string eventoInicial = ObtenerEventoInicialSegunClase(personaje);
                
                // Iniciar el evento especifico para la clase del personaje
                if (!gestor.IniciarEvento(eventoInicial))
                {
                    Console.WriteLine($"Error: No se encontró el evento inicial para {personaje.Nombre}.");
                    return;
                }
                
                // Bucle principal del juego sujeto a cambios
                BucleJuego(personaje, estado, gestor);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar el juego: {ex.Message}");
            }
        }
        
        private string ObtenerEventoInicialSegunClase(Personaje personaje)
        {
            // Determinar el evento inicial basado en el tipo de personaje
            if (personaje is Caballero)
            {
                return "inicio_caballero";
            }
            else if (personaje is Mago)
            {
                return "inicio_mago";
            }
            else if (personaje is Asesino)
            {
                return "inicio_asesino";
            }
            else
            {
                return "inicio_generico";
            }
        }
        
        private void BucleJuego(Personaje personaje, EstadoJuego estado, GestorEventos gestor)
        {
            bool juegoActivo = true;
            
            while (juegoActivo && !personaje.JugadorDerrotado)
            {
                // Menu principal del juego
                Console.WriteLine("\n1. Ver estadísticas del personaje");
                Console.WriteLine("2. Buscar nuevo evento");
                Console.WriteLine("3. Salir del juego"); //esta opcion puede que la modifique no se
                
                int opcion;
                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción inválida. Intente de nuevo.");
                    continue;
                }
                
                switch (opcion)
                {
                    case 1:
                        personaje.MostrarStats();
                        break;
                    case 2:
                        // Generar eventos basados en la ubicación o la clase del personaje 
                        //Esto me permite que a posteriori introdusca eventos ocultos
                        string eventoSiguiente = GenerarEventoSegunClaseYProgreso(personaje, estado);
                        gestor.IniciarEvento(eventoSiguiente);
                        break;
                    case 3:
                        juegoActivo = false;
                        Console.WriteLine("Gracias por jugar.");
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            }
            
            if (personaje.JugadorDerrotado)
            {
                Console.WriteLine("¡Has sido derrotado! Fin del juego.");
            }
        }
        
        private string GenerarEventoSegunClaseYProgreso(Personaje personaje, EstadoJuego estado)
        {
            Random random = new Random();
            List<string> eventosDisponibles = new List<string>();
            
            // Eventos comunes para todas las clases
            eventosDisponibles.Add("evento_comun_exploracion");
            eventosDisponibles.Add("evento_comun_combate");
            eventosDisponibles.Add("evento_comun_tienda"); //Nisiquiera cree un inventario asi que es un boceto mas que algo funcional
            
            //No se si esto lo voy a tener que modificar ya que mi idea es implementar evolcuiones para cada clase, problema para mi yo futuro
            //Esta parte tengo que ver como optimizarla ya que me enerva tener varios if

            //Tengo que corregir esto ya que estoy llamando eventos que no existen y entro en un bucle infinito

            // Eventos especificos de cada clase 
            if (personaje is Caballero)
            {
                eventosDisponibles.Add("evento_caballero_honor");
                eventosDisponibles.Add("evento_caballero_duelo");
            }
            else if (personaje is Mago)
            {
                eventosDisponibles.Add("evento_mago_grimorio");
                eventosDisponibles.Add("evento_mago_ritual");
            }
            else if (personaje is Asesino)
            {
                eventosDisponibles.Add("evento_asesino_contrato");
                eventosDisponibles.Add("evento_asesino_sigilo");
            }
            
            // Filtrar eventos ya completados
            var eventosFiltrados = eventosDisponibles
                .Where(e => !estado.EventosCompletados.Contains(e))
                .ToList();
            
            // Si no hay eventos disponibles que no se hayan completado, permitir repetir algunos eventos comunes
            if (eventosFiltrados.Count == 0)
            {
                eventosFiltrados.Add("evento_comun_exploracion");
                eventosFiltrados.Add("evento_comun_combate");
                eventosFiltrados.Add("evento_comun_tienda"); 
            }
            
            // Seleccionar un evento aleatorio de los disponibles
            return eventosFiltrados[random.Next(eventosFiltrados.Count)];
        }
    }
    
}


//Los eventos del json son simplemente algo generado de forma random ya que quiero testear si primero funciona jajaja
