using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiPlusPOE.Models
{
    public class Receta
    {
        public int ID_Receta { get; set; }
        public int ID_FK_Consulta { get; set; }
        public string Medicamento { get; set; }
        public string Dosis { get; set; }
        public string Duracion { get; set; }
    }
}
