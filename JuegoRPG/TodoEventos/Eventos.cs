using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace practicarUNI.Eventos
{
    public class Evento
    {
        public string ID{get; set;}
        public string Descripcion{get; set;}
        public List<OpcionEvento> Opciones{get; set;}
        public string Ubicacion{get; set;}

        public Evento(string id, string descripcion, List<OpcionEvento> opciones, string ubicacion = "")
        {
            ID = id;
            Descripcion = descripcion;
            Opciones= opciones;
            Ubicacion=ubicacion;
        }
    }
    public class OpcionEvento
    {
        public string Texto{get; set;}
        public ResultadoOpcion Resultado{get; set;}
        public OpcionEvento(string texto, ResultadoOpcion resultado)
        {
            Texto = texto;
            Resultado = resultado;
        }
    }
    public class ResultadoOpcion
    {
        public Dictionary<string, int> CambiosEstado { get; set; }
        public string SiguienteEventoID { get; set; }
        public bool CompletarEvento { get; set; }
        public bool IniciarCombate { get; set; }

        public ResultadoOpcion(Dictionary<string, int> cambiosEstado, string siguienteEventoID, bool completarEvento, bool iniciarCombate)
        {
            CambiosEstado = cambiosEstado;
            SiguienteEventoID = siguienteEventoID;
            CompletarEvento = completarEvento;
            IniciarCombate = iniciarCombate;
        }
    }
}