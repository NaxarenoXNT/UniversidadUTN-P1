using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace practicarUNI
{
    public interface IModificable
    {
        void ModificarEstado(string atributo, object valor);
    }
}