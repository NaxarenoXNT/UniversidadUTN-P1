using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace practicarUNI.JuegoRPG.Padres
{
    public abstract class NPCs
    {
        public int ID { get; set; }

        public bool EsBueno { get; set; }
        public bool EsMalo { get; set; }
        public bool EsNeutro { get; set; }
        public string Nombre { get; set; }
        public string trabajo { get; set; } 
        public string Ubicacion_NPC { get; set; }
        public string EventoAsociado { get; set; }
        public List<string> Dialogos { get; set; }
        public Ciudad Ciudad { get; set; } //para que el NPC pueda tener una ciudad asociada.
        public List<Items> Inventario { get; set; } //para que el NPC pueda tener un inventario, no se si es necesario pero lo dejo por si acaso.
        
        public NPCs(string nombre, string trabajo, string ubicacion, List<string> dialogos, string eventoAsociado = "")
        {
            Nombre = nombre;
            this.trabajo = trabajo;
            Ubicacion_NPC = ubicacion;
            Dialogos = dialogos;
            EventoAsociado = eventoAsociado;
        }
    }
}

//tengo que crear cada npc y asociarle una actividad, locadlidad y dialogo, y si es bueno o malo, y si tiene un evento asociado.