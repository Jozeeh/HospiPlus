using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Windows;
using HospiPlusPOE.Models;

namespace HospiPlusPOE.Controllers
{
    public class MedicoController
    {
        private string _credencialesConexion;

        public MedicoController()
        {
            _credencialesConexion = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }

        //=============================================
        // MÉTODO PARA OBTENER EL LISTADO DE MÉDICOS
        //=============================================
        public List<Medico> ObtenerMedicos()
        {
            var medicos = new List<Medico>();

            using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
            {
                conexion.Open();
                string query = "SELECT * FROM Medico, Usuario WHERE ID_Usuario = ID_FK_Usuario AND Rol = 'Medico' AND Estado = 'Activo'";

                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        medicos.Add(new Medico
                        {
                            ID_Medico = Convert.ToInt32(reader.GetInt32(0)),
                            ID_FK_Usuario = Convert.ToInt32(reader.GetInt32(1)),
                            Especialidad = reader.GetString(2),
                            NumeroLicencia = reader.GetString(3),
                            ID_Usuario = Convert.ToInt32(reader.GetInt32(4)),
                            Nombre = reader.GetString(5),
                            Apellido = reader.GetString(6),
                            Rol = reader.GetString(7),
                            Nickname = reader.GetString(8),
                            Correo = reader.GetString(9),
                            Telefono = reader.GetString(10),
                            Password = reader.GetString(11),
                            Estado = reader.GetString(12)
                        });
                    }
                }
            }

            return medicos;
        }


        //==============================
        // MÉTODO PARA AGREGAR MÉDICO
        //==============================
        public bool AgregarMedico(int idUsuario, string especialidad, string numeroLicencia)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO Medico (ID_FK_Usuario, Especialidad, NumeroLicencia) VALUES (@ID_FK_Usuario, @Especialidad, @NumeroLicencia)";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        // Pasar el ID_FK_Usuario, Especialidad y NumeroLicencia
                        command.Parameters.AddWithValue("@ID_FK_Usuario", idUsuario);
                        command.Parameters.AddWithValue("@Especialidad", especialidad);
                        command.Parameters.AddWithValue("@NumeroLicencia", numeroLicencia);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar médico: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool EditarMedico(int idMedico, int idUsuario, string especialidad, string numeroLicencia)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "UPDATE Medico SET ID_FK_Usuario = @ID_FK_Usuario, Especialidad = @Especialidad, NumeroLicencia = @NumeroLicencia WHERE ID_Medico = @ID_Medico";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        // Pasar los valores necesarios}
                        command.Parameters.AddWithValue("@Especialidad", especialidad);
                        command.Parameters.AddWithValue("@NumeroLicencia", numeroLicencia);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar médico: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }



        public bool DesactivarMedico(int idMedico) {
        bool medicoDesactivado = false;

        try
        {
            // Preguntar al usuario si desea desactivar al médico
            MessageBoxResult result = MessageBox.Show("¿Está seguro que desea desactivar este médico?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "UPDATE Medico SET Estado = 'Inactivo' WHERE ID_Medico = @ID_Medico";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@ID_Medico", idMedico);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Médico desactivado correctamente", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                        medicoDesactivado = true;
                    }

                    conexion.Close();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al desactivar médico: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return medicoDesactivado;
    }

}
}