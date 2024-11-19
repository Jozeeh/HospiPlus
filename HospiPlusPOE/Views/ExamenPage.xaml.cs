using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

//Importamos model/controller para poder utilizar la clase Usuario
using HospiPlusPOE.Models;
using HospiPlusPOE.Controllers;

namespace PlusHospi.Views
{
    public partial class ExamenPage : Page
    {
        //Creamos coleccion observable para mostrar los pacientes y examenes en el DataGrid
        public ObservableCollection<Paciente> Pacientes { get; set; }
        public ObservableCollection<Examen> Examenes { get; set; }

        public ExamenPage()
        {
            InitializeComponent();

            //Ocultamos confirmar editar y cancelar editar
            btnConfirmarEditar.Visibility = Visibility.Hidden;
            btnCancelarEditar.Visibility = Visibility.Hidden;

        }


        private void LimpiarCampos()
        {
            // Limpiamos los campos después de guardar el examen
            cmbTipoExamen.SelectedIndex = -1;
            dateFechaExamen.SelectedDate = null;
            cmbResultadoExamen.SelectedIndex = -1;
        }

        //===========================================
        //BOTÓN PARA BUSCAR PACIENTE Y SUS EXAMENES
        //===========================================
        private void btnBuscarPacienteExamen_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos el id del paciente
            try
            {
                int idPaciente = Convert.ToInt32(txtIdBuscarPaciente.Text);

                //Buscamos el paciente desde la base de datos
                var pacienteDesdeDB = new ExamenController().ObtenerPacienteBuscado(idPaciente);

                //Establecemos los datos del paciente en el DataGrid
                Pacientes = new ObservableCollection<Paciente>(pacienteDesdeDB.Select(paciente => new Paciente
                {
                    ID_Paciente = paciente.ID_Paciente,
                    Nombre = paciente.Nombre,
                    Apellido = paciente.Apellido,
                    Sexo = paciente.Sexo,
                    Correo = paciente.Correo,
                    Telefono = paciente.Telefono,
                    DUI = paciente.DUI,
                    Direccion = paciente.Direccion,
                    Seguro_Medico = paciente.Seguro_Medico,
                    FechaNacimiento = paciente.FechaNacimiento,
                    ContactoEmergenciaNombre = paciente.ContactoEmergenciaNombre,
                    ContactoEmergenciaTelefono = paciente.ContactoEmergenciaTelefono,
                    ContactoEmergenciaRelacion = paciente.ContactoEmergenciaRelacion
                }));

                //Mostramos los pacientes en el DataGrid
                datagridPaciente.ItemsSource = Pacientes;

                //Limpiamos el DataGrid de exámenes siempre al iniciar la búsqueda
                datagridExamenesPaciente.ItemsSource = null;

                //Verificamos si encontro paciente
                if (Pacientes.Count > 0)
                {
                    //Buscamos los examenes vinculados al mismo ID
                    var examenDesdeDB = new ExamenController().ObtenerExamenes(idPaciente);

                    //Establecemos los datos de los examenes en el DataGrid
                    Examenes = new ObservableCollection<Examen>(examenDesdeDB.Select(examen => new Examen
                    {
                        ID_Examen = examen.ID_Examen,
                        ID_FK_Paciente = examen.ID_FK_Paciente,
                        Tipo = examen.Tipo,
                        Resultado = examen.Resultado,
                        Fecha = examen.Fecha
                    }));

                    //Mostramos los examenes en el DataGrid
                    datagridExamenesPaciente.ItemsSource = examenDesdeDB;
                }

            } catch (Exception)
            {
                MessageBox.Show("Ingrese un ID válido");
            }
            
        }

        //===========================================
        //         BOTÓN PARA AGREGAR EXAMEN
        //===========================================
        private void btnAgregarExamen_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                //Obtenemos el ID del paciente
                int idPaciente = Convert.ToInt32(txtIdBuscarPaciente.Text);

                //Obtenemos los datos del examen
                string tipoExamen = cmbTipoExamen.Text;
                string resultadoExamen = cmbResultadoExamen.Text;
                DateTime fechaExamen = dateFechaExamen.SelectedDate.Value;

                //Verificamos si los campos están vacíos
                if (tipoExamen == "" || resultadoExamen == "" || fechaExamen == null)
                {
                    MessageBox.Show("Por favor llene todos los campos");
                    return;

                } else
                {
                    
                }
                //Creamos el examen mandando los valores a ExamenController
                var examen = new ExamenController().AgregarExamen(idPaciente, tipoExamen, resultadoExamen, fechaExamen);

                //Si se guardo el examen actualizamos los datos
                if (examen == true)
                {
                    btnBuscarPacienteExamen_Click(null, null);
                    LimpiarCampos();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Debes llenar todos los campos!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private int IDEditarExamen;
        //===========================================
        //         BOTÓN PARA EDITAR EXAMEN
        //===========================================
        private void btnEditarExamen_Click(object sender, RoutedEventArgs e)
        {

            //Obtenemos los valores del examen seleccionado en datagrid
            Examen? examenSeleccionado = datagridExamenesPaciente.SelectedItem as Examen;

            if (examenSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un examen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                //Asignamos los valores a los campos
                IDEditarExamen = examenSeleccionado.ID_Examen;
                cmbTipoExamen.Text = examenSeleccionado.Tipo;
                cmbResultadoExamen.Text = examenSeleccionado.Resultado;
                dateFechaExamen.SelectedDate = examenSeleccionado.Fecha;

                //Mostramos los botones de confirmar y cancelar editar
                btnConfirmarEditar.Visibility = Visibility.Visible;
                btnCancelarEditar.Visibility = Visibility.Visible;

                //Desactivamos agregar, editar y eliminar
                btnAgregarExamen.IsEnabled = false;
                btnEditarExamen.IsEnabled = false;
                btnEliminarExamen.IsEnabled = false;

                //Desactivamos que se pueda modificar id del paciente
                txtIdBuscarPaciente.IsEnabled = false;
            }

        }

        //===========================================
        //BOTÓN PARA CONFIRMAR EDITAR EXAMEN
        //===========================================
        private void btnCancelarEditar_Click(object sender, RoutedEventArgs e)
        {
            //Ocultamos los botones de confirmar y cancelar editar
            btnConfirmarEditar.Visibility = Visibility.Hidden;
            btnCancelarEditar.Visibility = Visibility.Hidden;

            //Activamos agregar, editar y eliminar
            btnAgregarExamen.IsEnabled = true;
            btnEditarExamen.IsEnabled = true;
            btnEliminarExamen.IsEnabled = true;

            //Activamos que se pueda modificar id del paciente
            txtIdBuscarPaciente.IsEnabled = true;

            //Limpiamos los campos
            LimpiarCampos();

        }

        //===========================================
        //  BOTÓN PARA CONFIRMAR EDITAR EXAMEN
        //===========================================
        private void btnConfirmarEditar_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                //Obtenemos los valores de los campos
                int idExamen = IDEditarExamen;
                string tipoExamen = cmbTipoExamen.Text;
                string resultadoExamen = cmbResultadoExamen.Text;
                DateTime fechaExamen = dateFechaExamen.SelectedDate.Value;

                //Verificamos si los campos están vacíos
                if (tipoExamen == "" || resultadoExamen == "" || fechaExamen == null)
                {
                    MessageBox.Show("Por favor llene todos los campos");
                    return;

                }
                else
                {
                    //Ejecutamos el método de EditarExamen en ExamenController
                    bool validacion = new ExamenController().EditarExamen(idExamen, tipoExamen, resultadoExamen, fechaExamen);

                    //Si el examen se edita correctamente, limpiamos los campos y actualizamos el DataGrid
                    if (validacion == true)
                    {
                        //Actualizamos el DataGrid
                        btnBuscarPacienteExamen_Click(null, null);

                        //Ocultamos los botones de confirmar y cancelar editar
                        btnConfirmarEditar.Visibility = Visibility.Hidden;
                        btnCancelarEditar.Visibility = Visibility.Hidden;

                        //Activamos agregar, editar y eliminar
                        btnAgregarExamen.IsEnabled = true;
                        btnEditarExamen.IsEnabled = true;
                        btnEliminarExamen.IsEnabled = true;

                        //Activamos que se pueda modificar id del paciente
                        txtIdBuscarPaciente.IsEnabled = true;

                        //Limpiamos los campos
                        LimpiarCampos();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Debes llenar todos los campos!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        //==============================
        //  BOTÓN PARA ELIMINAR EXAMEN
        //==============================
        private void btnEliminarExamen_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos el examen seleccionado en el DataGrid
            Examen? examenSeleccionado = datagridExamenesPaciente.SelectedItem as Examen;

            if (examenSeleccionado == null)
            {
                MessageBox.Show("Selecciona un examen para eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                //Ejecutamos el método de EliminarExamen en ExamenController
                bool validacion = new ExamenController().EliminarExamen(examenSeleccionado.ID_Examen);

                //Si el examen se elimina correctamente, actualizamos el DataGrid
                if (validacion == true)
                {
                    btnBuscarPacienteExamen_Click(null, null);
                }
                else
                {
                    //Limpiamos seleccion
                    datagridExamenesPaciente.SelectedItem = null;
                }
            }
        }
    }
}
