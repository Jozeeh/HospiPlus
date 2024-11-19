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

        //=============================
        //MÉTODO PARA AGREGAR PACIENTE
        //=============================
        public bool AgregarPaciente(string nombre, string apellido, DateTime fechaNacimiento, string direccion, string seguroMedico, string dui, string sexo, string telefono, string correo, string nombreEmergencia, string telefonoEmergencia, string relacionEmergencia)
        {
            bool pacienteAgregado = false;

            try
            {
                //Insertamos el paciente en la base de datos
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO Paciente (Nombre, Apellido, Sexo, Correo, Telefono, DUI, Direccion, Seguro_Medico, Fecha_Nacimiento, Contacto_Emergencia_Nombre, Contacto_Emergencia_Telefono, Contacto_Emergencia_Relacion, Estado) VALUES (@Nombre, @Apellido, @Sexo, @Correo, @Telefono, @DUI, @Direccion, @Seguro_Medico, @Fecha_Nacimiento, @ContactoEmergenciaNombre, @ContactoEmergenciaTelefono, @ContactoEmergenciaRelacion, @Estado)";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Apellido", apellido);
                        command.Parameters.AddWithValue("@Sexo", sexo);
                        command.Parameters.AddWithValue("@Correo", correo);
                        command.Parameters.AddWithValue("@Telefono", telefono);
                        command.Parameters.AddWithValue("@DUI", dui);
                        command.Parameters.AddWithValue("@Direccion", direccion);
                        command.Parameters.AddWithValue("@Seguro_Medico", seguroMedico);
                        command.Parameters.AddWithValue("@Fecha_Nacimiento", fechaNacimiento);
                        command.Parameters.AddWithValue("@ContactoEmergenciaNombre", nombreEmergencia);
                        command.Parameters.AddWithValue("@ContactoEmergenciaTelefono", telefonoEmergencia);
                        command.Parameters.AddWithValue("@ContactoEmergenciaRelacion", relacionEmergencia);
                        command.Parameters.AddWithValue("@Estado", "Activo");

                        command.ExecuteNonQuery();
                        //Mostramos mensaje de confirmacion
                        MessageBox.Show("Paciente agregado correctamente", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                        pacienteAgregado = true;
                    }

                    conexion.Close();
                }
            } catch (Exception ex)
            {
                //Mostramos mensaje de error
                MessageBox.Show("Error al agregar paciente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return pacienteAgregado;
        }

        //==============================
        //MÉTODO PARA EDITAR PACIENTE
        //==============================
        public bool EditarPaciente(int idPaciente, string nombre, string apellido, DateTime fechaNacimiento, string direccion, string seguroMedico, string dui, string sexo, string telefono, string correo, string nombreEmergencia, string telefonoEmergencia, string relacionEmergencia)
        {
            bool pacienteEditado = false;

            try
            {

                //Preguntamos si desea editar
                MessageBoxResult result = MessageBox.Show("¿Está seguro que desea editar este paciente?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                    {
                        conexion.Open();
                        string query = "UPDATE Paciente SET Nombre = @Nombre, Apellido = @Apellido, Sexo = @Sexo, Correo = @Correo, Telefono = @Telefono, DUI = @DUI, Direccion = @Direccion, Seguro_Medico = @Seguro_Medico, Fecha_Nacimiento = @Fecha_Nacimiento, Contacto_Emergencia_Nombre = @ContactoEmergenciaNombre, Contacto_Emergencia_Telefono = @ContactoEmergenciaTelefono, Contacto_Emergencia_Relacion = @ContactoEmergenciaRelacion WHERE ID_Paciente = @ID_Paciente";

                        using (SqlCommand command = new SqlCommand(query, conexion))
                        {
                            command.Parameters.AddWithValue("@ID_Paciente", idPaciente);
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Apellido", apellido);
                            command.Parameters.AddWithValue("@Sexo", sexo);
                            command.Parameters.AddWithValue("@Correo", correo);
                            command.Parameters.AddWithValue("@Telefono", telefono);
                            command.Parameters.AddWithValue("@DUI", dui);
                            command.Parameters.AddWithValue("@Direccion", direccion);
                            command.Parameters.AddWithValue("@Seguro_Medico", seguroMedico);
                            command.Parameters.AddWithValue("@Fecha_Nacimiento", fechaNacimiento);
                            command.Parameters.AddWithValue("@ContactoEmergenciaNombre", nombreEmergencia);
                            command.Parameters.AddWithValue("@ContactoEmergenciaTelefono", telefonoEmergencia);
                            command.Parameters.AddWithValue("@ContactoEmergenciaRelacion", relacionEmergencia);

                            command.ExecuteNonQuery();
                            MessageBox.Show("Paciente editado correctamente", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                            pacienteEditado = true;
                        }

                        conexion.Close();
                    }
                }

            } catch (Exception ex)
            {
                MessageBox.Show("Error al editar paciente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return pacienteEditado;
        }

        //================================
        //MÉTODO PARA DESACTIVAR PACIENTE
        //================================
        public bool DesactivarPaciente(int idPaciente)
        {
            bool pacienteDesactivado = false;

            try
            {

                //Preguntamos si desea desactivar
                MessageBoxResult result = MessageBox.Show("¿Está seguro que desea desactivar este paciente?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                    {
                        conexion.Open();
                        string query = "UPDATE Paciente SET Estado = 'Inactivo' WHERE ID_Paciente = @ID_Paciente";

                        using (SqlCommand command = new SqlCommand(query, conexion))
                        {
                            command.Parameters.AddWithValue("@ID_Paciente", idPaciente);

                            command.ExecuteNonQuery();
                            MessageBox.Show("Paciente desactivado correctamente", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                            pacienteDesactivado = true;
                        }

                        conexion.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al desactivar paciente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return pacienteDesactivado;
        }
    }
}
