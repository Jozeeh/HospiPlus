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
    public class ExamenController
    {

        //Credenciales de la conexion a la base de datos
        private string _credencialesConexion;

        public ExamenController()
        {
            //Credenciales de la conexion a la base de datos
            _credencialesConexion = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }


        //=====================================
        //MÉTODO PARA OBTENER PACIENTE BUSCADO
        //=====================================
        public List<Paciente> ObtenerPacienteBuscado(int idPaciente)
        {
            var paciente = new List<Paciente>();

            try
            {
                //Obtenemos el paciente de la base de datos
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "SELECT * FROM Paciente WHERE ID_Paciente = @ID_Paciente";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@ID_Paciente", idPaciente);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            //Creamos un objeto de tipo Usuario
                            paciente.Add(new Paciente
                            {
                                ID_Paciente = reader.GetInt32(0),
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
                        } else
                        {
                            MessageBox.Show("No se encontró paciente con el ID: " + idPaciente);
                        }
                    }
                    conexion.Close();
                }

            } catch (Exception ex)
            {
                MessageBox.Show("Error al buscar paciente: " + ex.Message);
            }

            return paciente;
        }


        //=================================
        //MÉTODO PARA OBTENER LOS EXAMENES
        //=================================
        public List<Examen> ObtenerExamenes(int idPaciente)
        {
            //Lista de examenes
            var examenes = new List<Examen>();

            //Obtenemos los examenes de la base de datos
            using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
            {
                conexion.Open();
                string query = "SELECT * FROM Examen WHERE ID_FK_Paciente = @idPaciente";

                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@idPaciente", idPaciente);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        examenes.Add(new Examen
                        {
                            ID_Examen = reader.GetInt32(0),
                            ID_FK_Paciente = reader.GetInt32(1),
                            Tipo = reader.GetString(2),
                            Resultado = reader.GetString(3),
                            Fecha = reader.GetDateTime(4),
                        });
                    }
                }

                conexion.Close();
            }

            return examenes;
        }

        //=================================
        //MÉTODO PARA AGREGAR UN EXAMEN
        //=================================
        public bool AgregarExamen(int idFkPaciente, string tipo, string resultado, DateTime fecha)
        {
            bool examenAgregado = false;

            try
            {
                //Insertamos el examen en la base de datos
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO Examen (ID_FK_Paciente, Tipo, Resultado, Fecha) VALUES (@ID_FK_Paciente, @Tipo, @Resultado, @Fecha)";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@ID_FK_Paciente", idFkPaciente);
                        command.Parameters.AddWithValue("@Tipo", tipo);
                        command.Parameters.AddWithValue("@Resultado", resultado);
                        command.Parameters.AddWithValue("@Fecha", fecha);

                        command.ExecuteNonQuery();
                    }
                    conexion.Close();
                    examenAgregado = true;
                }

                MessageBox.Show("Examen agregado correctamente");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar examen: " + ex.Message);
            }

            return examenAgregado;
        }

    }
}
