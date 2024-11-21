using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospiPlusPOE.Controllers;
using HospiPlusPOE.Models;

namespace HospiPlusPOE.Models
{
    public class Cita
    {
        public int ID_Cita { get; set; }
        public string NombrePaciente { get; set; }
        public string ApellidoPaciente { get; set; }
        public string NombreMedico { get; set; }
        public string ApellidoMedico { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public int ID_FK_Paciente { get; set; }
        public int ID_FK_Medico { get; set; }
        
    }

}
