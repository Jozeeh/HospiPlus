using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiPlusPOE.Models
{
    public class Examen
    {
        public int ID_Examen { get; set; }
        public int ID_FK_Paciente { get; set; }
        public string Tipo { get; set; }
        public string Resultado { get; set; }
        public DateTime Fecha { get; set; }


    }
}
