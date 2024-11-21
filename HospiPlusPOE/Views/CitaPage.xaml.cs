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
using System.Collections.ObjectModel;
using HospiPlusPOE.Controllers;
using HospiPlusPOE.Models;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Windows.Forms;
//establecemos messagebox
using MessageBox = System.Windows.MessageBox;
using System.Globalization;

namespace PlusHospi.Views
{
    public partial class CitaPage : Page
    {
        private CitaController _citaController;
        private ObservableCollection<Medico> _medicos;
        private ObservableCollection<Paciente> _pacientes;

        public CitaPage()
        {
            InitializeComponent();
            _citaController = new CitaController();
            CargarDatos();

            // LLenamos combo box de horas
            // Llenar ComboBox de horas (1-12)
            for (int hour = 1; hour <= 12; hour++)
            {
                cmbHora.Items.Add(hour.ToString("D2"));
            }

            // Llenar ComboBox de minutos (00-59)
            for (int minute = 0; minute < 60; minute += 1) // Opcional: usar intervalos
            {
                cmbMinuto.Items.Add(minute.ToString("D2"));
            }

            // Llenar ComboBox de AM/PM
            cmbAmPm.Items.Add("AM");
            cmbAmPm.Items.Add("PM");

            // Seleccionar valores predeterminados
            cmbHora.SelectedIndex = 0;
            cmbMinuto.SelectedIndex = 0;
            cmbAmPm.SelectedIndex = 0;

            // Ocultar botones de confirmación y cancelación
            btnConfirmarEditar.Visibility = Visibility.Hidden;
            btnCancelarEditar.Visibility = Visibility.Hidden;
        }

        // Método para cargar los datos de los médicos y pacientes en los ComboBox
        private void CargarDatos()
        {
            //Obtenemos el listado de medicos (debe mostrar el nombre y el guardar como valor el id medico)
            _medicos = new ObservableCollection<Medico>(new MedicoController().ObtenerMedicos());
            cmbMedico.ItemsSource = _medicos;
            cmbMedico.DisplayMemberPath = "Nombre";
            cmbMedico.SelectedValuePath = "ID_Medico";

            //Obtenemos el listado de pacientes (debe mostrar el nombre y el guardar como valor el id paciente)
            _pacientes = new ObservableCollection<Paciente>(new PacienteController().ObtenerPacientes());
            cmbPaciente.ItemsSource = _pacientes;
            cmbPaciente.DisplayMemberPath = "Nombre";
            cmbPaciente.SelectedValuePath = "ID_Paciente";

            //Mostramos listado de citas
            CitasDataGrid.ItemsSource = _citaController.ObtenerCitas();

        }


        // Método para crear una cita
        private void BtnCrearCita_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {
                //Obtenemos los valores de los campos
                DateTime fecha = dpFecha.SelectedDate.Value;
                string timeString = $"{cmbHora.Text}:{cmbMinuto.Text} {cmbAmPm.Text}";
                DateTime dateTime = DateTime.ParseExact(timeString, "hh:mm tt", CultureInfo.InvariantCulture);
                TimeSpan hora = dateTime.TimeOfDay;
                int medico = (int)cmbMedico.SelectedValue;
                int paciente = (int)cmbPaciente.SelectedValue;

                _citaController.AgregarCita(fecha, hora, medico, paciente);
                CargarDatos();
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Método para validar que los campos no estén vacíos
        private bool ValidarCampos()
        {
            return dpFecha.SelectedDate != null && !string.IsNullOrWhiteSpace(cmbHora.Text) && !string.IsNullOrWhiteSpace(cmbMinuto.Text) && !string.IsNullOrWhiteSpace(cmbAmPm.Text) && cmbMedico.SelectedIndex != -1 && cmbPaciente.SelectedIndex != -1;
        }

        // Método para limpiar los campos del formulario
        private void LimpiarCampos()
        {
            dpFecha.SelectedDate = null;
            cmbHora.SelectedIndex = 0;
            cmbMinuto.SelectedIndex = 0;
            cmbAmPm.SelectedIndex = 0;
            cmbMedico.SelectedIndex = -1;
            cmbPaciente.SelectedIndex = -1;
        }

        private int IDCitaSeleccionada = 0;
        private void btnEditarCita_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos los valors y asignamos a los campos
            if (CitasDataGrid.SelectedItem is not Cita citaSeleccionada)
            {
                MessageBox.Show("Seleccione una cita para editar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                IDCitaSeleccionada = citaSeleccionada.ID_Cita;
                dpFecha.SelectedDate = citaSeleccionada.Fecha;
                cmbHora.Text = citaSeleccionada.Hora.Hours.ToString("D2");
                cmbMinuto.Text = citaSeleccionada.Hora.Minutes.ToString("D2");

                // Manually handle AM/PM
                cmbAmPm.Text = citaSeleccionada.Hora.Hours >= 12 ? "PM" : "AM";

                cmbMedico.SelectedValue = citaSeleccionada.ID_FK_Medico;
                cmbPaciente.SelectedValue = citaSeleccionada.ID_FK_Paciente;

                btnConfirmarEditar.Visibility = Visibility.Visible;
                btnCancelarEditar.Visibility = Visibility.Visible;

                //Desactivamos los botones
                btnCrearCita.IsEnabled = false;
                btnEditarCita.IsEnabled = false;
                btnEliminarCita.IsEnabled = false;
            }

        }

        private void btnCancelarEditar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();

            btnConfirmarEditar.Visibility = Visibility.Hidden;
            btnCancelarEditar.Visibility = Visibility.Hidden;

            //Activamos los botones
            btnCrearCita.IsEnabled = true;
            btnEditarCita.IsEnabled = true;
            btnEliminarCita.IsEnabled = true;
        }

        private void btnConfirmarEditar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {
                //Obtenemos los valores de los campos
                DateTime fecha = dpFecha.SelectedDate.Value;
                string timeString = $"{cmbHora.Text}:{cmbMinuto.Text} {cmbAmPm.Text}";
                DateTime dateTime = DateTime.ParseExact(timeString, "hh:mm tt", CultureInfo.InvariantCulture);
                TimeSpan hora = dateTime.TimeOfDay;
                int medico = (int)cmbMedico.SelectedValue;
                int paciente = (int)cmbPaciente.SelectedValue;

                _citaController.EditarCita(IDCitaSeleccionada, fecha, hora, medico, paciente);
                CargarDatos();

                LimpiarCampos();

                btnConfirmarEditar.Visibility = Visibility.Hidden;
                btnCancelarEditar.Visibility = Visibility.Hidden;

                //Activamos los botones
                btnCrearCita.IsEnabled = true;
                btnEditarCita.IsEnabled = true;
                btnEliminarCita.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void btnEliminarCita_Click(object sender, RoutedEventArgs e)
        {

            if (CitasDataGrid.SelectedItem is not Cita citaSeleccionada)
            {
                MessageBox.Show("Seleccione una cita para eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                _citaController.EliminarCita(citaSeleccionada.ID_Cita);
                CargarDatos();
            }
        }
    }
}
