using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

//Importamos model/controller para poder utilizar la clase Usuario
using HospiPlusPOE.Models;
using HospiPlusPOE.Controllers;

namespace PlusHospi.Views
{
    /// <summary>
    /// Lógica de interacción para RecetaPage.xaml
    /// </summary>
    public partial class RecetaPage : Page
    {
        //Creamos coleccion observable para mostrar los pacientes y examenes en el DataGrid
        public ObservableCollection<Consulta> Consultas { get; set; }
        public RecetaPage()
        {
            InitializeComponent();

            //Desactivamos los campos para agregar recetas
            cmbMedicamento.IsEnabled = false;
            cmbDosis.IsEnabled = false;
            cmbDuracion.IsEnabled = false;

        }

        private void LimpiarCampos()
        {
            cmbMedicamento.SelectedIndex = -1;
            cmbDosis.SelectedIndex = -1;
            cmbDuracion.SelectedIndex = -1;
        }

        //=================================
        //BÓTON PARA OBTENER LAS CONSULTAS
        //=================================
        private void btnBuscarPacienteReceta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Obtenemos id paciente
                int idPaciente = Convert.ToInt32(txtIdPaciente.Text);

                //Buscamos las consultas por el id
                var consultasDesdeBD = new RecetaController().ObtenerConsultasPorPaciente(idPaciente);

                //Establecemos los datos del paciente en el DataGrid
                Consultas = new ObservableCollection<Consulta>(consultasDesdeBD.Select(consulta => new Consulta
                {
                    ID_Consulta = consulta.ID_Consulta,
                    Sintomas = consulta.Sintomas,
                    Diagnostico = consulta.Diagnostico,
                    ID_FK_Paciente = consulta.ID_FK_Paciente,
                    NombrePaciente = consulta.NombrePaciente,
                    ApellidoPaciente = consulta.ApellidoPaciente,
                    ID_FK_Medico = consulta.ID_FK_Medico,
                    NombreMedico = consulta.NombreMedico,
                    ApellidoMedico = consulta.ApellidoMedico,
                    Especialidad = consulta.Especialidad,
                    ID_FK_Cita = consulta.ID_FK_Cita
                }));

                //Mostramos las consultas en el DataGrid
                datagridConsultas.ItemsSource = Consultas;

                //Limpiamos el datagrid de recetas
                datagridRecetas.ItemsSource = null;

                //Verificamos si encontro consultas
                if (Consultas.Count > 0)
                {

                    //Obtenemos las recetas segun ID paciente
                    var recetasDesdeBD = new RecetaController().ObtenerRecetasPorPaciente(idPaciente);

                    //Establecemos los datos de las recetas en el DataGrid
                    var recetas = new ObservableCollection<Receta>(recetasDesdeBD.Select(receta => new Receta
                    {
                        ID_Receta = receta.ID_Receta,
                        ID_FK_Consulta = receta.ID_FK_Consulta,
                        Medicamento = receta.Medicamento,
                        Dosis = receta.Dosis,
                        Duracion = receta.Duracion
                    }));

                    //Mostramos las recetas en el DataGrid
                    datagridRecetas.ItemsSource = recetas;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar paciente: " + ex.Message);
            }

        }
    }
}
