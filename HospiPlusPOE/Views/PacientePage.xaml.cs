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

            //Ocultamos botones de confirmar editar y cancelar editar
            btnConfirmarEditarPaciente.Visibility = Visibility.Hidden;
            btnCancelar.Visibility = Visibility.Hidden;

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

        //=======================================================
        //BOTÓN PARA AGREGAR PACIENTES DESDE PacienteController
        //=======================================================
        private void btnAgregarPaciente_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos los datos
            string nombre = txtNombrePac.Text;
            string apellido = txtApellidoPac.Text;
            DateTime fechaNacimiento = dateFechaNacimientoPac.SelectedDate.Value;
            string direccion = txtDireccionPac.Text;
            string seguroMedico = cmbSeguroPac.Text;
            string dui = txtDuiPac.Text;
            string sexo = cmbSexoPac.Text;
            string telefono = txtTelefonoPac.Text;
            string correo = txtCorreoPac.Text;
            string nombreEmergencia = txtNombreEmergenciaPac.Text;
            string telefonoEmergencia = txtTelefonoEmergenciaPac.Text;
            string relacionEmergencia = cmbRelacionEmergenciaPac.Text;

            //Validamos que los campos no estén vacíos
            if (nombre == "" || apellido == "" || fechaNacimiento == null || direccion == "" || seguroMedico == "" || dui == "" || sexo == "" || telefono == "" || correo == "" || nombreEmergencia == "" || telefonoEmergencia == "" || relacionEmergencia == "")
            {
                MessageBox.Show("Por favor, llene todos los campos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Ejecutamos metodo de AgregarPaciente en PacienteController
                bool validacion = new PacienteController().AgregarPaciente(nombre, apellido, fechaNacimiento, direccion, seguroMedico, dui, sexo, telefono, correo, nombreEmergencia, telefonoEmergencia, relacionEmergencia);

                //Actualizamos el DataGrid
                MostrarPacientes();

                //Limpiamos los campos
                LimpiarCampos();
            }
        }

        private int IDPacienteSeleccionado;
        //========================================================
        //BOTÓN PARA EDITAR PACIENTES (asigna datos a los campos)
        //========================================================
        private void btnEditarPaciente_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos el paciente seleccionado
            Paciente? pacienteSeleccionado = datagridPacientes.SelectedItem as Paciente;

            if (pacienteSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un paciente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Asignamos los valores a los campos
                IDPacienteSeleccionado = pacienteSeleccionado.ID_Paciente;
                txtNombrePac.Text = pacienteSeleccionado.Nombre;
                txtApellidoPac.Text = pacienteSeleccionado.Apellido;
                dateFechaNacimientoPac.SelectedDate = pacienteSeleccionado.FechaNacimiento;
                txtDireccionPac.Text = pacienteSeleccionado.Direccion;
                cmbSeguroPac.Text = pacienteSeleccionado.Seguro_Medico;
                txtDuiPac.Text = pacienteSeleccionado.DUI;
                cmbSexoPac.Text = pacienteSeleccionado.Sexo;
                txtTelefonoPac.Text = pacienteSeleccionado.Telefono;
                txtCorreoPac.Text = pacienteSeleccionado.Correo;
                txtNombreEmergenciaPac.Text = pacienteSeleccionado.ContactoEmergenciaNombre;
                txtTelefonoEmergenciaPac.Text = pacienteSeleccionado.ContactoEmergenciaTelefono;
                cmbRelacionEmergenciaPac.Text = pacienteSeleccionado.ContactoEmergenciaRelacion;

                //Mostramos los botones de confirmar y cancelar editar
                btnConfirmarEditarPaciente.Visibility = Visibility.Visible;
                btnCancelar.Visibility = Visibility.Visible;

                //Desabilitamos el botón de agregar, editar y eliminar
                btnAgregarPaciente.IsEnabled = false;
                btnEditarPaciente.IsEnabled = false;
                btnDesactivarPaciente.IsEnabled = false;
            }

        }

        //===============================================================
        //BOTÓN PARA CONFIRMAR EDITAR PACIENTES DESDE PacienteController
        //===============================================================
        private void btnConfirmarEditarPaciente_Click(object sender, RoutedEventArgs e)
        {

            //Obtenemos los valores de los campos
            string nombre = txtNombrePac.Text;
            string apellido = txtApellidoPac.Text;
            DateTime fechaNacimiento = dateFechaNacimientoPac.SelectedDate.Value;
            string direccion = txtDireccionPac.Text;
            string seguroMedico = cmbSeguroPac.Text;
            string dui = txtDuiPac.Text;
            string sexo = cmbSexoPac.Text;
            string telefono = txtTelefonoPac.Text;
            string correo = txtCorreoPac.Text;
            string nombreEmergencia = txtNombreEmergenciaPac.Text;
            string telefonoEmergencia = txtTelefonoEmergenciaPac.Text;
            string relacionEmergencia = cmbRelacionEmergenciaPac.Text;

            //Validamos que los campos no estén vacíos
            if (nombre == "" || apellido == "" || fechaNacimiento == null || direccion == "" || seguroMedico == "" || dui == "" || sexo == "" || telefono == "" || correo == "" || nombreEmergencia == "" || telefonoEmergencia == "" || relacionEmergencia == "")
            {
                MessageBox.Show("Por favor, llene todos los campos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Ejecutamos el método de EditarPaciente en PacienteController
                bool validacion = new PacienteController().EditarPaciente(IDPacienteSeleccionado, nombre, apellido, fechaNacimiento, direccion, seguroMedico, dui, sexo, telefono, correo, nombreEmergencia, telefonoEmergencia, relacionEmergencia);

                //Si el paciente se edita correctamente, limpiamos los campos y actualizamos el DataGrid
                if (validacion == true)
                {
                    //Actualizamos el DataGrid
                    MostrarPacientes();
                    LimpiarCampos();

                    //Ocultamos los botones de confirmar y cancelar editar
                    btnConfirmarEditarPaciente.Visibility = Visibility.Hidden;
                    btnCancelar.Visibility = Visibility.Hidden;

                    //Habilitamos el botón de agregar, editar y eliminar
                    btnAgregarPaciente.IsEnabled = true;
                    btnEditarPaciente.IsEnabled = true;
                    btnDesactivarPaciente.IsEnabled = true;
                }
                else
                {
                    //Limpiamos seleccion
                    datagridPacientes.SelectedItem = null;
                }
            }

        }

        //======================================
        //BOTÓN PARA CANCELAR EDITAR PACIENTES
        //======================================
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();

            //Ocultamos los botones de confirmar y cancelar editar
            btnConfirmarEditarPaciente.Visibility = Visibility.Hidden;
            btnCancelar.Visibility = Visibility.Hidden;

            //Habilitamos el botón de agregar, editar y eliminar
            btnAgregarPaciente.IsEnabled = true;
            btnEditarPaciente.IsEnabled = true;
            btnDesactivarPaciente.IsEnabled = true;
        }

        //=======================================================
        //BOTÓN PARA DESACTIVAR PACIENTES DESDE PacienteController
        //=======================================================
        private void btnDesactivarPaciente_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos el paciente seleccionado
            Paciente? pacienteSeleccionado = datagridPacientes.SelectedItem as Paciente;

            if (pacienteSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un paciente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Ejecutamos el método de DesactivarPaciente en PacienteController
                bool validacion = new PacienteController().DesactivarPaciente(pacienteSeleccionado.ID_Paciente);

                if (validacion == true)
                {
                    //Actualizamos el DataGrid
                    MostrarPacientes();
                
                } else
                {
                    //Limpiamos seleccion
                    datagridPacientes.SelectedItem = null;
                }
            }
        }

        //===============================
        //MÉTODO PARA LIMPIAR LOS CAMPOS
        //===============================
        private void LimpiarCampos()
        {
            txtNombrePac.Clear();
            txtApellidoPac.Clear();
            dateFechaNacimientoPac.SelectedDate = null;
            txtDireccionPac.Clear();
            cmbSeguroPac.SelectedIndex = -1;
            txtDuiPac.Clear();
            cmbSexoPac.SelectedIndex = -1;
            txtTelefonoPac.Clear();
            txtCorreoPac.Clear();
            txtNombreEmergenciaPac.Clear();
            txtTelefonoEmergenciaPac.Clear();
            cmbRelacionEmergenciaPac.SelectedIndex = -1;
        }

        
    }
}
