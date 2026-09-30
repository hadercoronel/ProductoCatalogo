using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Excepciones
{
    public class DomainExcepciones : Exception
    {
        public DomainExcepciones(string message) : base(message)
        {
        }
    }
}
