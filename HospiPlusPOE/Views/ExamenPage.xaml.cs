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
        }


        private void LimpiarCampos()
        {
            // Limpiamos los campos después de guardar el examen
            cmbTipoExamen.SelectedIndex = -1;
            dateFechaExamen.SelectedDate = null;
            cmbResultadoExamen.SelectedIndex = -1;
            cmbConsulta.SelectedIndex = -1;
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
    }
}
