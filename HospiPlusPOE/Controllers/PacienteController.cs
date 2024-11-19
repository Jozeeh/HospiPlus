using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Importamos model para poder utilizar la clase Usuario
using HospiPlusPOE.Models;

//Importacion librerias para la conexion a la base de datos
using System.Configuration;
using Microsoft.Data.SqlClient;

//Importamos librerias para mostrar mensajes
using System.Windows;

namespace HospiPlusPOE.Controllers
{
    public class PacienteController
    {
        //Credenciales de la conexion a la base de datos
        private string _credencialesConexion;
        public PacienteController()
        {
            //Credenciales de la conexion a la base de datos
            _credencialesConexion = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }

        //============================================
        //MÉTODO PARA OBTENER EL LISTADO DE PACIENTES
        //============================================
        public List<Paciente> ObtenerPacientes()
        {
            //Lista de pacientes
            var pacientes = new List<Paciente>();

            //Obtenemos los pacientes de la base de datos
            using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
            {
                conexion.Open();
                string query = "SELECT * FROM Paciente";

                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //Creamos un objeto de tipo Paciente
                        pacientes.Add(new Paciente
                        {
                            ID_Paciente = Convert.ToInt32(reader.GetInt32(0)),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Sexo = reader.GetString(3),
                            Correo = reader.GetString(4),
                            Telefono = reader.GetString(5),
                            DUI = reader.GetString(6),
                            Direccion = reader.GetString(7),
                            Seguro_Medico = reader.GetString(8),
                            FechaNacimiento = reader.GetDateTime(9),
                            ContactoEmergenciaNombre = reader.GetString(10),
                            ContactoEmergenciaTelefono = reader.GetString(11),
                            ContactoEmergenciaRelacion = reader.GetString(12)
                        });
                    }
                }
            }

            return pacientes;
        }


    }
}
