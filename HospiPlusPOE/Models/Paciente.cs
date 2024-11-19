using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiPlusPOE.Models
{
    public class Paciente
    {
        public int ID_Paciente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Sexo { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string DUI { get; set; }
        public string Direccion { get; set; }
        public string Seguro_Medico { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string ContactoEmergenciaNombre { get; set; }
        public string ContactoEmergenciaTelefono { get; set; }
        public string ContactoEmergenciaRelacion { get; set; }
        public string Estado { get; set; }
    }
}
