using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace practicarUNI.JuegoRPG.Padres
{
    public class Tienda
    {
        public string Nombre { get; set; }
        public List<Items> Inventario { get; set; } //para que la tienda pueda tener un inventario.
        public string Ubicacion_Tienda { get; set; }
        public string EventoAsociado { get; set; } //para que la tienda pueda tener un evento asociado.
        
        public Tienda(string nombre, List<Items> inventario, string ubicacion, string eventoAsociado = "")
        {
            Nombre = nombre;
            Inventario = inventario;
            Ubicacion_Tienda = ubicacion;
            EventoAsociado = eventoAsociado;
        }
    }
}


//tengo que crear las "localizaciones" donde el jugador puede interactuar para agregar los eventos asociados a cada localizacion