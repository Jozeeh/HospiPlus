using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Importamos model para poder utilizar la clase Consulta
using HospiPlusPOE.Models;

//Importación de librerías para la conexión a la base de datos
using System.Configuration;
using Microsoft.Data.SqlClient;

//Importamos librerías para mostrar mensajes
using System.Windows;

namespace HospiPlusPOE.Controllers
{
    public class ConsultaController
    {
        private string _credencialesConexion;

        public ConsultaController()
        {
            _credencialesConexion = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }

        //===========================================
        // MÉTODO PARA OBTENER CONSULTAS POR CITA
        //===========================================
        public List<Consulta> ObtenerConsultas()
        {
            var consultas = new List<Consulta>();

            using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
            {
                conexion.Open();
                string query = "SELECT c.ID_Consulta, p.Nombre AS Nombre_Paciente, p.Apellido AS Apellido_Paciente, u.Nombre AS Nombre_Medico, u.Apellido AS Apellido_Medico, c.Sintomas, c.Diagnostico FROM Consulta c JOIN Cita ci ON c.ID_FK_Cita = ci.ID_Cita JOIN Paciente p ON ci.ID_FK_Paciente = p.ID_Paciente JOIN Medico m ON ci.ID_FK_Medico = m.ID_Medico JOIN Usuario u ON m.ID_FK_Usuario = u.ID_Usuario ORDER BY ci.Fecha DESC;";

                using (SqlCommand command = new SqlCommand(query, conexion))
                {

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        consultas.Add(new Consulta
                        {
                            ID_Consulta = reader.GetInt32(0),
                            NombrePaciente = reader.GetString(1),
                            ApellidoPaciente = reader.GetString(2),
                            NombreMedico = reader.GetString(3),
                            ApellidoMedico = reader.GetString(4),
                            Sintomas = reader.GetString(5),
                            Diagnostico = reader.GetString(6)

                        });
                    }
                }
            }

            return consultas;
        }


        public List<Paciente> ObtenerPacientes()
        {
            //Lista de pacientes
            var pacientes = new List<Paciente>();

            //Obtenemos los pacientes de la base de datos
            using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
            {
                conexion.Open();
                string query = "SELECT * FROM Paciente WHERE Estado = 'Activo'";

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


        public List<Medico> ObtenerMedicos()
        {
            var medicos = new List<Medico>();

            using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
            {
                conexion.Open();
                string query = "SELECT ID_Medico, Especialidad, NumeroLicencia FROM Medico"; // Consulta ajustada sin filtrar por estado

                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        medicos.Add(new Medico
                        {
                            ID_Medico = reader.GetInt32(0),
                            Especialidad = reader.GetString(1), // Cambié el índice para Especialidad
                            NumeroLicencia = reader.GetString(2) // Cambié el índice para NumeroLicencia
                        });
                    }
                }
            }

            return medicos;
        }


        //===========================================
        // MÉTODO PARA AGREGAR UNA CONSULTA
        //===========================================
        public void AgregarConsulta(int idCita, string sintomas, string diagnostico)
        {

            try
            {
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO Consulta (ID_FK_Cita, Sintomas, Diagnostico) VALUES (@ID_FK_Cita, @Sintomas, @Diagnostico)";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@ID_FK_Cita", idCita);
                        command.Parameters.AddWithValue("@Sintomas", sintomas);
                        command.Parameters.AddWithValue("@Diagnostico", diagnostico);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Consulta agregada correctamente", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar consulta: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //===========================================
        // MÉTODO PARA EDITAR UNA CONSULTA
        //===========================================
        public bool EditarConsulta(int idConsulta, string sintomas, string diagnostico)
        {
            bool consultaEditada = false;

            try
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro que desea editar esta consulta?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                    {
                        conexion.Open();
                        string query = "UPDATE Consulta SET Sintomas = @Sintomas, Diagnostico = @Diagnostico WHERE ID_Consulta = @ID_Consulta";

                        using (SqlCommand command = new SqlCommand(query, conexion))
                        {
                            command.Parameters.AddWithValue("@ID_Consulta", idConsulta);
                            command.Parameters.AddWithValue("@Sintomas", sintomas);
                            command.Parameters.AddWithValue("@Diagnostico", diagnostico);

                            command.ExecuteNonQuery();
                            MessageBox.Show("Consulta editada correctamente", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                            consultaEditada = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar consulta: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return consultaEditada;
        }

        //===========================================
        // MÉTODO PARA ELIMINAR UNA CONSULTA
        //===========================================
        public bool EliminarConsulta(int idConsulta)
        {
            bool consultaEliminada = false;

            try
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro que desea eliminar esta consulta?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                    {
                        conexion.Open();
                        string query = "DELETE FROM Consulta WHERE ID_Consulta = @ID_Consulta";

                        using (SqlCommand command = new SqlCommand(query, conexion))
                        {
                            command.Parameters.AddWithValue("@ID_Consulta", idConsulta);

                            command.ExecuteNonQuery();
                            MessageBox.Show("Consulta eliminada correctamente", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                            consultaEliminada = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar consulta: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return consultaEliminada;
        }
    }
}
