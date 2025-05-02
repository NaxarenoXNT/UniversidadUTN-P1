using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Eventos;
using practicarUNI.Juego;
using practicarUNI.Juego.padres;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace practicarUNI.JuegoRPG.TodoEventos
{
    public class EstadoJuego
    {
        public int Dinero { get; set; }
        public int Salud { get; set; }
        public int Experiencia{ get; set; }
        public List<string> EventosCompletados { get; set; } = new List<string>();
        public Personaje JugadorActual { get; set; }
        

        public EstadoJuego(Personaje jugador)
        {
            JugadorActual = jugador;
        }
    }



    public class GestorEventos
    {
        private Dictionary<string, Evento> _eventos;
        private EstadoJuego _estado;
        private Logica _logica;

        public GestorEventos(EstadoJuego estado, Logica logica)
        {
            _estado = estado;
            _logica = logica;
            _eventos = new Dictionary<string, Evento>();
            CargarEventosDesdeJSON(); // Metodo que carga los eventos que tengo que crear con el json
        }

        //Esto lo voy a cambiar para poder cargar los json de manera segregada desde un sub-carpeta json

        public void CargarEventosDesdeJSON()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var recursos = assembly.GetManifestResourceNames();
                
                Console.WriteLine("Buscando recursos disponibles...");
                
                if (recursos.Length == 0)
                {
                    Console.WriteLine("¡No se encontraron recursos incrustados!");
                }
                else
                {
                    Console.WriteLine("Recursos encontrados:");
                    foreach (var recurso in recursos)
                    {
                        Console.WriteLine($"- {recurso}");
                    }
                    
                    // Intenta cargar el primer recurso que contiene el json
                    string eventoResourceName = recursos.FirstOrDefault(r => r.EndsWith("eventos.json"));
                    
                    if (eventoResourceName != null)
                    {
                        Console.WriteLine($"Intentando cargar: {eventoResourceName}");
                        using (Stream stream = assembly.GetManifestResourceStream(eventoResourceName))
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string json = reader.ReadToEnd();
                            List<Evento> listaEventos = System.Text.Json.JsonSerializer.Deserialize<List<Evento>>(json);
                            foreach (var evento in listaEventos)
                            {
                                _eventos[evento.ID] = evento;
                            }
                            Console.WriteLine($"¡Se cargaron {_eventos.Count} eventos correctamente!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se encontró ningún recurso que termine con 'eventos.json'");
                        CrearEventosPorDefecto();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar los eventos: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                CrearEventosPorDefecto();
            }
        }
        private void CrearEventosPorDefecto()
        {
            // dependiendo de la clase que elijas se genera un evento principal
            // Evento generico en caso de que falle la carga del JSON :)
            var opcionesBasicas = new List<OpcionEvento>
            {
                new OpcionEvento("Continuar", new ResultadoOpcion(new Dictionary<string, int>(), "", true, false))
            };
            
            _eventos["inicio_generico"] = new Evento(
                "inicio_generico", 
                "No se pudieron cargar los eventos. Esta es una aventura de emergencia.", 
                opcionesBasicas
            );
            
            _eventos["inicio_caballero"] = new Evento(
                "inicio_caballero", 
                "Como caballero, comienzas tu aventura con tu espada y escudo.", 
                opcionesBasicas
            );
            
            _eventos["inicio_mago"] = new Evento(
                "inicio_mago", 
                "Como mago, comienzas tu aventura con tu libro de hechizos.", 
                opcionesBasicas
            );
            
            _eventos["inicio_asesino"] = new Evento(
                "inicio_asesino", 
                "Como asesino, comienzas tu aventura con tus dagas y venenos.", 
                opcionesBasicas
            );
        }

        public bool IniciarEvento(string id)
        {
            if (!_eventos.ContainsKey(id))
            {
                Console.WriteLine($"Evento con ID '{id}' no encontrado. Iniciando evento genérico.");
                
                // Si el evento específico no existe, intentamos usar uno generico
                if (_eventos.ContainsKey("evento_comun_exploracion"))
                {
                    id = "evento_comun_exploracion";
                }
                else if (_eventos.Count > 0)
                {
                    // Tomar cualquier evento disponible
                    id = _eventos.Keys.First();
                }
                else
                {
                    Console.WriteLine("No hay eventos disponibles.");
                    return false;
                }
            }

            var evento = _eventos[id];
            Console.WriteLine("\n" + evento.Descripcion);

            if (evento.Opciones.Count == 0)
            {
                Console.WriteLine("Este evento no tiene opciones disponibles.");
                return true;
            }

            for (int i = 0; i < evento.Opciones.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {evento.Opciones[i].Texto}");
            }

            int eleccion;
            bool seleccionValida = false;
            
            do
            {
                Console.Write("Selecciona una opción: ");
                string input = Console.ReadLine()??"";
                
                if (int.TryParse(input, out eleccion) && eleccion >= 1 && eleccion <= evento.Opciones.Count)
                {
                    seleccionValida = true;
                }
                else
                {
                    Console.WriteLine("Opción inválida. Por favor, elige una opción válida.");
                }
            } while (!seleccionValida);
            
            ProcesarEleccion(evento, eleccion - 1);
            return true;
        }

        public void ProcesarEleccion(Evento evento, int indice)
        {
            var resultado = evento.Opciones[indice].Resultado;

            if (resultado.CambiosEstado != null)
            {
                foreach (var cambio in resultado.CambiosEstado)
                {
                    _estado.JugadorActual.ModificarEstado(cambio.Key, cambio.Value);  //aca se pide hacer el cambio a la clase del jugador
                }
            }

            if (resultado.IniciarCombate)
            {
                Console.WriteLine("\n¡Te preparas para el combate!");
                List<Enemigo> enemigos = new List<Enemigo>();
                for (int i = 0; i < 2; i++)  // o usar un valor del evento
                {
                    enemigos.Add(GeneradorDeEnemigos.GenerarEnemigoAleatorio(_estado.JugadorActual.Nivel));
                }
                _logica.GestionTurnos(_estado.JugadorActual, enemigos);

                if (!_estado.JugadorActual.JugadorDerrotado)
                {
                    Console.WriteLine("\n¡Has sobrevivido al combate!");
                }
            }

            if (resultado.CompletarEvento)
            {
                if (!_estado.EventosCompletados.Contains(evento.ID))
                {
                    _estado.EventosCompletados.Add(evento.ID);
                    Console.WriteLine($"Evento '{evento.ID}' completado.");
                }
            }

            if (!string.IsNullOrEmpty(resultado.SiguienteEventoID))
            {
                IniciarEvento(resultado.SiguienteEventoID);
            }
            else
            {
                Console.WriteLine("\n o no hay mas historia o algo paso");
            }
        }

        
    }
}