using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiPlusPOE.Models
{
    public class Medico : Usuario
    {
        public int ID_Medico { get; set; }
        public string Especialidad { get; set; }
        public string NumeroLicencia { get; set; }
        public string Estado { get; set; }
    }
}
