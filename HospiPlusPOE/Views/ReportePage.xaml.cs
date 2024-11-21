using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;

//Importamos model/controller para poder utilizar la clase Usuario
using HospiPlusPOE.Models;
using HospiPlusPOE.Controllers;

//Importacion librerias para la conexion a la base de datos
using Microsoft.Data.SqlClient;

namespace PlusHospi.Views
{
    /// <summary>
    /// Lógica de interacción para ReportePage.xaml
    /// </summary>
    public partial class ReportePage : UserControl
    {
        private string _connectionString;
        public ReportePage()
        {
            InitializeComponent();
            _connectionString = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }

        //=============================================
        //BÓTON PARA GENERAR UN REPORTE DE EXPEDIENTE
        //=============================================
        private void btnReporteExpediente_Click(object sender, RoutedEventArgs e)
        {
            // Datos para el reporte
            try
            {
                int idPaciente = Convert.ToInt32(txtIDPaciente.Text);

                //Seleccionar carpeta ubicacion
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var ubicacionGuardar = dialog.SelectedPath;

                    // Generar el PDF
                    new ReporteController().generarReporteExpediente("ReporteExpediente.pdf", idPaciente, ubicacionGuardar);
                }
                else
                {
                    MessageBox.Show("Por favor seleccione una ubicación para guardar el reporte");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Por favor ingrese un ID de paciente válido: " + ex.Message);
                return;
            }
        }

        //==============================
        //BÓTON PARA BUSCAR UN PACIENTE
        //==============================
        private void btnBuscarPaciente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idPaciente = Convert.ToInt32(txtIDPaciente.Text);

                //Obtenemos los datos del paciente desde la Base de datos
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT p.ID_Paciente, " +
                        "p.Nombre, p.Apellido, " +
                        "p.Correo, " +
                        "p.Telefono " +
                        "FROM Paciente p " +
                        "WHERE p.ID_Paciente = @ID_Paciente;";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID_Paciente", idPaciente);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        //Agregamos valores a los label
                        lblNombre.Content = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString();
                        lblCorreo.Content = reader["Correo"].ToString();
                        lblTelefono.Content = reader["Telefono"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró un paciente con el ID ingresado");
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Por favor ingrese un ID de paciente válido");
                return;
            }
        }

        //============================
        //BÓTON PARA BUSCAR UN MEDICO
        //============================
        private void btnBuscarMedico_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idMedico = Convert.ToInt32(txtIDMedico.Text);

                //Obtenemos los datos del medico desde la Base de datos
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT " +
                        "Usuario.Nombre, " +
                        "Usuario.Apellido, " +
                        "Usuario.Correo, " +
                        "Usuario.Telefono " +
                        "FROM Medico " +
                        "INNER JOIN Usuario ON Medico.ID_FK_Usuario = Usuario.ID_Usuario " +
                        "WHERE Medico.ID_Medico= @ID_Medico;";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID_Medico", idMedico);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        //Agregamos valores a los label
                        lblNombreMed.Content = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString();
                        lblCorreoMed.Content = reader["Correo"].ToString();
                        lblTelefonoMed.Content = reader["Telefono"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró un médico con el ID ingresado");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Por favor ingrese un ID de médico válido" + ex.Message);
                return;
            }
        }

        //=====================================
        //BÓTON PARA GENERAR REPORTE CONSULTAS
        //=====================================
        private void btnReporteConsultas_Click(object sender, RoutedEventArgs e)
        {

            // Datos para el reporte
            try
            {
                int idMedico = Convert.ToInt32(txtIDMedico.Text);

                //Seleccionar carpeta ubicacion
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var ubicacionGuardar = dialog.SelectedPath;

                    // Generar el PDF
                    new ReporteController().generarReporteConsultas("ReporteConsultas.pdf", idMedico, ubicacionGuardar);
                }
                else
                {
                    MessageBox.Show("Por favor seleccione una ubicación para guardar el reporte");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Por favor ingrese un ID de médico válido: " + ex.Message);
                return;
            }
        }
    }
}
