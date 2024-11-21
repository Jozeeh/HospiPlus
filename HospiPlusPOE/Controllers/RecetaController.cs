using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class RecetaController
    {

        //Credenciales de la conexion a la base de datos
        private string _credencialesConexion;

        public RecetaController()
        {
            //Credenciales de la conexion a la base de datos
            _credencialesConexion = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }

        //=========================================================
        //MÉTODO PARA OBTENER LISTADO CONSULTAS SEGUN ID PACIENTE
        //=========================================================
        public List<Consulta> ObtenerConsultasPorPaciente(int idPaciente)
        {
            var consultas = new List<Consulta>();

            try
            {
                //Obtenemos las consultas de la base de datos
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "SELECT " +
                        "cons.ID_Consulta, " +
                        "cons.Sintomas, " +
                        "cons.Diagnostico, " +
                        "p.ID_Paciente, " +
                        "p.Nombre AS NombrePaciente, " +
                        "p.Apellido AS ApellidoPaciente, " +
                        "u.ID_Usuario AS ID_Medico, " +
                        "u.Nombre AS NombreMedico, " +
                        "u.Apellido AS ApellidoMedico, " +
                        "m.Especialidad, " +
                        "c.ID_Cita " +
                        "FROM Consulta cons " +
                        "INNER JOIN Cita c ON " +
                        "cons.ID_FK_Cita = c.ID_Cita " +
                        "INNER JOIN Paciente p ON c.ID_FK_Paciente = p.ID_Paciente " +
                        "INNER JOIN Medico m ON c.ID_FK_Medico = m.ID_Medico " +
                        "INNER JOIN Usuario u ON m.ID_FK_Usuario = u.ID_Usuario " +
                        "WHERE c.ID_FK_Paciente = @ID_Paciente " +
                        "ORDER BY c.Fecha DESC, c.Hora DESC;";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@ID_Paciente", idPaciente);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            //Creamos un objeto de tipo Consulta
                            consultas.Add(new Consulta
                            {
                                ID_Consulta = reader.GetInt32(0),
                                Sintomas = reader.GetString(1),
                                Diagnostico = reader.GetString(2),
                                ID_FK_Paciente = reader.GetInt32(3),
                                NombrePaciente = reader.GetString(4),
                                ApellidoPaciente = reader.GetString(5),
                                ID_FK_Medico = reader.GetInt32(6),
                                NombreMedico = reader.GetString(7),
                                ApellidoMedico = reader.GetString(8),
                                Especialidad = reader.GetString(9),
                                ID_FK_Cita = reader.GetInt32(10)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las consultas del paciente: " + ex.Message);
            }

            return consultas;
        }

        //========================================================
        //MÉTODO PARA OBTENER LISTADO DE RECETAS POR ID PACIENTE
        //========================================================
        public List<Receta> ObtenerRecetasPorPaciente(int idPaciente)
        {
            var recetas = new List<Receta>();

            try
            {
                //Obtenemos las recetas de la base de datos
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "SELECT " +
                        "r.ID_Receta, " +
                        "cons.ID_Consulta, " +
                        "r.Medicamento, " +
                        "r.Dosis, " +
                        "r.Duracion " +
                        "FROM Receta r " +
                        "INNER JOIN Consulta cons ON r.ID_FK_Consulta = cons.ID_Consulta " +
                        "INNER JOIN Cita c ON cons.ID_FK_Cita = c.ID_Cita " +
                        "WHERE c.ID_FK_Paciente = @ID_Paciente " +
                        "ORDER BY r.ID_Receta ASC;";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@ID_Paciente", idPaciente);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            //Creamos un objeto de tipo Receta
                            recetas.Add(new Receta
                            {
                                ID_Receta = reader.GetInt32(0),
                                ID_FK_Consulta = reader.GetInt32(1),
                                Medicamento = reader.GetString(2),
                                Dosis = reader.GetString(3),
                                Duracion = reader.GetString(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las recetas del paciente: " + ex.Message);
            }

            return recetas;
        }

        //=================================
        //MÉTODO PARA AGREGAR RECETA
        //=================================
        public void AgregarReceta(int idFkReceta, string medicamento, string dosis, string duracion)
        {

            try
            {
                //Insertamos la receta en la base de datos
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO Receta (ID_FK_Consulta, Medicamento, Dosis, Duracion) VALUES (@idFkReceta, @Medicamento, @Dosis, @Duracion)";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@idFkReceta", idFkReceta);
                        command.Parameters.AddWithValue("@Medicamento", medicamento);
                        command.Parameters.AddWithValue("@Dosis", dosis);
                        command.Parameters.AddWithValue("@Duracion", duracion);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Receta agregada correctamente");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar la receta: " + ex.Message);
            }
        }

        //=================================
        //MÉTODO PARA EDITAR RECETA
        //=================================
        public void EditarReceta(int idReceta, string medicamento, string dosis, string duracion)
        {

            try
            {
                //Editamos la receta en la base de datos
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "UPDATE Receta SET Medicamento = @Medicamento, Dosis = @Dosis, Duracion = @Duracion WHERE ID_Receta = @ID_Receta";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@ID_Receta", idReceta);
                        command.Parameters.AddWithValue("@Medicamento", medicamento);
                        command.Parameters.AddWithValue("@Dosis", dosis);
                        command.Parameters.AddWithValue("@Duracion", duracion);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Receta editada correctamente");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar la receta: " + ex.Message);
            }
        }

        //=================================
        //MÉTODO PARA ELIMINAR RECETA
        //=================================
        public bool EliminarReceta(int idReceta)
        {
            bool recetaEliminada = false;

            //Preguntamos si quiere eliminar
            MessageBoxResult result = MessageBox.Show("¿Está seguro que desea eliminar la receta?", "Eliminar receta", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    //Eliminamos la receta en la base de datos
                    using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                    {
                        conexion.Open();
                        string query = "DELETE FROM Receta WHERE ID_Receta = @ID_Receta";

                        using (SqlCommand command = new SqlCommand(query, conexion))
                        {
                            command.Parameters.AddWithValue("@ID_Receta", idReceta);

                            command.ExecuteNonQuery();
                            MessageBox.Show("Receta eliminada correctamente");
                            recetaEliminada = true;
                        }
                        conexion.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar la receta: " + ex.Message);
                }
            }

            return recetaEliminada;
        }

    }
}