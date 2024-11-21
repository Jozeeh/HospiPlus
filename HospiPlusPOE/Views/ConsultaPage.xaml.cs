using HospiPlusPOE.Controllers;
using HospiPlusPOE.Models;
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
namespace PlusHospi.Views
{
    public partial class ConsultaPage : Page
    {
        private ConsultaController _consultaController;
        private CitaController _citaController;
        private ObservableCollection<Medico> _medicos;
        private ObservableCollection<Paciente> _pacientes;

        public ConsultaPage()
        {
            InitializeComponent();
            _consultaController = new ConsultaController();
            _citaController = new CitaController();
            CargarDatos();
        }

        private void CargarDatos()
        {
            //Llenamos el datagridCitas
            datagridCitas.ItemsSource = _citaController.ObtenerCitas();

            //Llenamos el datagridConsultas solo con los valores
            datagridConsultas.ItemsSource = _consultaController.ObtenerConsultas();
        }

        private void LimpiarCampos()
        {
            txtDiagnostico.Clear();
            txtDescripcion.Clear();
        }

        private void CancelarConsulta_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void GuardarConsulta_Click(object sender, RoutedEventArgs e)
        {
            // Recoger datos de los controles
            if (datagridCitas.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione una cita.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int idCita = ((Cita)datagridCitas.SelectedItem).ID_Cita;
            string sintomas = txtDescripcion.Text;
            string diagnostico = txtDiagnostico.Text;

            //Verificamos que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(sintomas) || string.IsNullOrWhiteSpace(diagnostico))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                // Crear consulta
                _consultaController.AgregarConsulta(idCita, sintomas, diagnostico);
                LimpiarCampos();
                CargarDatos();
            }
        }
    }
}