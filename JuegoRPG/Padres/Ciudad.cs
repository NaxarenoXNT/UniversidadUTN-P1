using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace practicarUNI.JuegoRPG.Padres
{
    //todo preparado para cuando se me ocurra como impleementar las condiciones para los eventos.
    public abstract class Ciudad
    {
        public string Nombre { get; set; }
        public List<string> Eventos_Disponibles { get; set; }
        public List<NPCs> NPCs { get; set; }
        public List<Tienda> Tiendas { get; set; }



        public Ciudad(string nombre, List<string> eventos_disponibles, List<NPCs> npcs, List<Tienda> tiendas)
        {
            Nombre = nombre;
            Eventos_Disponibles = eventos_disponibles;
            NPCs = npcs;
            Tiendas = tiendas;
        }
    }

    

    
}