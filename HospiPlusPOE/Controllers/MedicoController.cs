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
                string query = "SELECT * FROM Medico";  // La consulta sigue siendo la misma

                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        medicos.Add(new Medico
                        {
                            // Usamos GetInt32() para los campos enteros
                            ID_Medico = reader.GetInt32(reader.GetOrdinal("ID_Medico")),  // ID_Medico es de tipo int
                            ID_FK_Usuario = reader.GetInt32(reader.GetOrdinal("ID_FK_Usuario")),  // ID_FK_Usuario es de tipo int
                            Especialidad = reader.IsDBNull(reader.GetOrdinal("Especialidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Especialidad")),  // Especialidad es de tipo varchar
                            NumeroLicencia = reader.IsDBNull(reader.GetOrdinal("NumeroLicencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("NumeroLicencia"))  // NumeroLicencia es de tipo varchar
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



        public bool DesactivarMedico(int idMedico)
    {
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