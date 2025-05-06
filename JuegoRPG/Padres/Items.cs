using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego.padres;

namespace practicarUNI.JuegoRPG.Padres
{
    public class Items
    {
        public bool EsConsumible { get; set; }
        public bool EsEquipable { get; set; }
        public bool EsMejorable { get; set; }
        public bool EsDestruible { get; set; }
        public bool Bloqueado { get; set; } //para que el item no se pueda usar o equipar hasta que se cumpla una condicion.
        public bool EsObjetounico { get; set; } //para que el item es o no stakeable
        public int ID { get; set; }
        public int Precio { get; set; }
        public int Cantidad { get; set; } 
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Action<Personaje> Usar { get; set; } //para que el item pueda tener una accion asociada al usarlo.
        


        public Items(
            string nombre,
            string descripcion, 
            int precio, 
            bool esConsumible = false, 
            bool esEquipable = false, 
            bool esMejorable = false, 
            bool esDestruible = false, 
            bool bloqueado = false, 
            bool esObjetounico = false)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            EsConsumible = esConsumible;
            EsEquipable = esEquipable;
            EsMejorable = esMejorable;
            EsDestruible = esDestruible;
            Bloqueado = bloqueado;
            EsObjetounico = esObjetounico;
        }


    }
}

//tengo que crear una clase para los objetos que no son consumibles, como armas y armaduras, y otra para los consumibles, como pociones y comida.