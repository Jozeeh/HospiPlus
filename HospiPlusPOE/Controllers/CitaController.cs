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
using System.Linq.Expressions;
namespace HospiPlusPOE.Controllers
{
    public class CitaController
    {
        // Credenciales de la conexión a la base de datos
        private string _credencialesConexion;

        public CitaController()
        {
            // Obtener las credenciales de la conexión desde el archivo de configuración
            _credencialesConexion = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }

        // ========================================
        // MÉTODO PARA OBTENER EL LISTADO DE CITAS
        // ========================================
        public List<Cita> ObtenerCitas()
        {
            var citas = new List<Cita>();

            using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
            {
                conexion.Open();
                string query = "SELECT Cita.ID_Cita,Paciente.Nombre AS Nombre_Paciente, Paciente.Apellido AS Apellido_Paciente, Usuario.Nombre AS Nombre_Medico, Usuario.Apellido AS Apellido_Medico, Cita.Fecha, Cita.Hora, ID_FK_Paciente, ID_FK_Medico FROM Cita INNER JOIN Paciente ON Cita.ID_FK_Paciente = Paciente.ID_Paciente INNER JOIN Medico ON Cita.ID_FK_Medico = Medico.ID_Medico INNER JOIN Usuario ON Medico.ID_FK_Usuario = Usuario.ID_Usuario ORDER BY Cita.Fecha ASC, Cita.Hora ASC;";

                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        citas.Add(new Cita
                        {
                            ID_Cita = reader.GetInt32(0),
                            NombrePaciente = reader.GetString(1),
                            ApellidoPaciente = reader.GetString(2),
                            NombreMedico = reader.GetString(3),
                            ApellidoMedico = reader.GetString(4),
                            Fecha = reader.GetDateTime(5),
                            Hora = reader.GetTimeSpan(6),
                            ID_FK_Paciente = reader.GetInt32(7),
                            ID_FK_Medico = reader.GetInt32(8)

                        });
                    }
                }
            }

            return citas;
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


        // ===============================
        // MÉTODO PARA AGREGAR UNA CITA
        // ===============================
        public void AgregarCita(DateTime fecha, TimeSpan hora, int idMedico, int idPaciente)
        {

            try
            {
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO Cita (Fecha, Hora, ID_FK_Medico, ID_FK_Paciente) VALUES (@Fecha, @Hora, @ID_FK_Medico, @ID_FK_Paciente)";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@Fecha", fecha);
                        command.Parameters.AddWithValue("@Hora", hora);
                        command.Parameters.AddWithValue("@ID_FK_Medico", idMedico);
                        command.Parameters.AddWithValue("@ID_FK_Paciente", idPaciente);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Cita agregada correctamente", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar cita: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ================================
        // MÉTODO PARA EDITAR UNA CITA
        // ================================
        public bool EditarCita(int idCita, DateTime fecha, TimeSpan hora, int idMedico, int idPaciente)
        {
            bool citaEditada = false;

            try
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro que desea editar esta cita?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                    {
                        conexion.Open();
                        string query = "UPDATE Cita SET Fecha = @Fecha, Hora = @Hora, ID_FK_Medico = @ID_FK_Medico, ID_FK_Paciente = @ID_FK_Paciente WHERE ID_Cita = @ID_Cita";

                        using (SqlCommand command = new SqlCommand(query, conexion))
                        {
                            command.Parameters.AddWithValue("@ID_Cita", idCita);
                            command.Parameters.AddWithValue("@Fecha", fecha);
                            command.Parameters.AddWithValue("@Hora", hora);
                            command.Parameters.AddWithValue("@ID_FK_Medico", idMedico);
                            command.Parameters.AddWithValue("@ID_FK_Paciente", idPaciente);

                            command.ExecuteNonQuery();
                            MessageBox.Show("Cita editada correctamente", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                            citaEditada = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar cita: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return citaEditada;
        }

        // ================================
        // MÉTODO PARA ELIMINAR UNA CITA
        // ================================
        public bool EliminarCita(int idCita)
        {
            bool citaEliminada = false;

            try
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro que desea eliminar esta cita?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                    {
                        conexion.Open();

                        // Verificar si la cita tiene alguna consulta asociada
                        string verificarQuery = "SELECT COUNT(*) FROM Consulta WHERE ID_FK_Cita = @ID_Cita"; // Verificar consultas asociadas a la cita
                        using (SqlCommand verificarCommand = new SqlCommand(verificarQuery, conexion))
                        {
                            verificarCommand.Parameters.AddWithValue("@ID_Cita", idCita);

                            int consultasAsociadas = (int)verificarCommand.ExecuteScalar();

                            if (consultasAsociadas > 0)
                            {
                                MessageBox.Show("No se puede eliminar la cita porque tiene consultas asociadas.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return citaEliminada; // Salir sin eliminar la cita
                            }
                        }

                        // Si no hay consultas asociadas, proceder con la eliminación
                        string eliminarQuery = "DELETE FROM Cita WHERE ID_Cita = @ID_Cita";
                        using (SqlCommand eliminarCommand = new SqlCommand(eliminarQuery, conexion))
                        {
                            eliminarCommand.Parameters.AddWithValue("@ID_Cita", idCita);

                            eliminarCommand.ExecuteNonQuery();

                            MessageBox.Show("Cita eliminada correctamente.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                            citaEliminada = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar cita: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return citaEliminada;
        }

    }
}
