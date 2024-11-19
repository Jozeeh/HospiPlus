using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiPlusPOE.Models
{
    public class Consulta
    {
        public int ID_Consulta { get; set; }
        public string Sintomas { get; set; }
        public string Diagnostico { get; set; }
        public int ID_FK_Paciente { get; set; }
        public string NombrePaciente { get; set; }
        public string ApellidoPaciente { get; set; }
        public int ID_FK_Medico { get; set; }
        public string NombreMedico { get; set; }
        public string ApellidoMedico { get; set; }
        public string Especialidad { get; set; }
        public int ID_FK_Cita { get; set; }

    }
}
