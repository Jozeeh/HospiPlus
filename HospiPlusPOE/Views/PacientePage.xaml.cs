using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HospiPlusPOE.Controllers;
using HospiPlusPOE.Models;

namespace PlusHospi.Views
{
    public partial class PacientePage : Page
    {
        //Creamos coleccion observable para mostrar los pacientes en el DataGrid
        public ObservableCollection<Paciente> Pacientes { get; set; }

        public PacientePage()
        {
            InitializeComponent();

            //Inicializamos la coleccion observable
            Pacientes = new ObservableCollection<Paciente>();
            MostrarPacientes();
        }

        //=======================================================
        //MÉTODO PARA MOSTRAR PACIENTES DESDE PacienteController
        //=======================================================
        public void MostrarPacientes()
        {
            //Obtenemos los usuarios de la base de datos
            var pacienteDesdeBD = new PacienteController().ObtenerPacientes();

            //Mostramos los usuarios en el DataGrid
            Pacientes = new ObservableCollection<Paciente>(pacienteDesdeBD.Select(paciente => new Paciente
            {
                ID_Paciente = paciente.ID_Paciente,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                FechaNacimiento = paciente.FechaNacimiento,
                Direccion = paciente.Direccion,
                Seguro_Medico = paciente.Seguro_Medico,
                DUI = paciente.DUI,
                Sexo = paciente.Sexo,
                Telefono = paciente.Telefono,
                Correo = paciente.Correo,
                ContactoEmergenciaNombre = paciente.ContactoEmergenciaNombre,
                ContactoEmergenciaTelefono = paciente.ContactoEmergenciaTelefono,
                ContactoEmergenciaRelacion = paciente.ContactoEmergenciaRelacion
            }));

            datagridPacientes.ItemsSource = Pacientes;
        }


        private void LimpiarCampos()
        {
            NombreTextBox.Clear();
            ApellidoTextBox.Clear();
            FechaNacimientoPicker.SelectedDate = null;
            DireccionTextBox.Clear();
            SeguroComboBox.SelectedIndex = -1;
            DUITextBox.Clear();
            SexoComboBox.SelectedIndex = -1;
            TelefonoTextBox.Clear();
            CorreoTextBox.Clear();
            ContactoEmergenciaNombreTextBox.Clear();
            ContactoEmergenciaTelefonoTextBox.Clear();
            ContactoEmergenciaRelacionComboBox.SelectedIndex = -1;
        }
    }
}
