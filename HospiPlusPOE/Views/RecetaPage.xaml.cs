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

            //Ocultamos botones de confirmarEditar y cancelarEditar
            btnConfirmarEditar.Visibility = Visibility.Hidden;
            btnCancelarEditar.Visibility = Visibility.Hidden;

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

        //=================================
        //BÓTON PARA AGREGAR RECETA
        //=================================
        private void btnAgregarReceta_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos id consulta seleccionada del datagrid
            Consulta consultaSeleccionada = (Consulta)datagridConsultas.SelectedItem;

            //Obtenemos los valores
            string medicamento = cmbMedicamento.Text;
            string dosis = cmbDosis.Text;
            string duracion = cmbDuracion.Text;

            //Verificamos si se selecciono una consulta
            if (consultaSeleccionada != null)
            {
                //Verificamos si los campos estan vacios
                if (medicamento != "" && dosis != "" && duracion != "")
                {
                    //Agregamos la receta
                    new RecetaController().AgregarReceta(consultaSeleccionada.ID_Consulta, medicamento, dosis, duracion);

                    //Limpiamos los campos
                    LimpiarCampos();

                    //Actualizamos el datagrid de recetas
                    datagridRecetas.ItemsSource = new RecetaController().ObtenerRecetasPorPaciente(consultaSeleccionada.ID_FK_Paciente);
                }
                else
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una consulta.");
            }
        }

        private int IDRecetaSeleccionada;
        //=================================
        //BÓTON PARA EDITAR RECETA
        //=================================
        private void btnEditarReceta_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos los valores
            Receta recetaSeleccionada = (Receta)datagridRecetas.SelectedItem;

            if (recetaSeleccionada != null)
            {
                IDRecetaSeleccionada = recetaSeleccionada.ID_Receta;
                cmbMedicamento.Text = recetaSeleccionada.Medicamento;
                cmbDosis.Text = recetaSeleccionada.Dosis;
                cmbDuracion.Text = recetaSeleccionada.Duracion;

                //Mostramos botones de confirmarEditar y cancelarEditar
                btnConfirmarEditar.Visibility = Visibility.Visible;
                btnCancelarEditar.Visibility = Visibility.Visible;

                //Desactivamos agregar, editar y eliminar y la modificacion de id
                txtIdPaciente.IsEnabled = false;
                btnAgregarReceta.IsEnabled = false;
                btnEditarReceta.IsEnabled = false;
                btnEliminarReceta.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una receta.");
            }
        }

        //=================================
        //BÓTON PARA CONFIRMAR EDICIÓN
        //=================================
        private void btnConfirmarEditar_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos los valores
            int idFkReceta = IDRecetaSeleccionada;
            string medicamento = cmbMedicamento.Text;
            string dosis = cmbDosis.Text;
            string duracion = cmbDuracion.Text;

            //Verificamos si los campos estan vacios
            if (medicamento != "" && dosis != "" && duracion != "")
            {
                //Editamos la receta
                new RecetaController().EditarReceta(idFkReceta, medicamento, dosis, duracion);

                //Limpiamos los campos
                LimpiarCampos();

                //Actualizamos el datagrid de recetas
                btnBuscarPacienteReceta_Click(null, null);

                //Ocultamos botones de confirmarEditar y cancelarEditar
                btnConfirmarEditar.Visibility = Visibility.Hidden;
                btnCancelarEditar.Visibility = Visibility.Hidden;

                //Volvemos activar los botones agregar, editar y eliminar tambien id modificacion
                txtIdPaciente.IsEnabled = true;
                btnAgregarReceta.IsEnabled = true;
                btnEditarReceta.IsEnabled = true;
                btnEliminarReceta.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos.");
            }

        }

        //=================================
        //BÓTON PARA CANCELAR EDITAR RECETA
        //=================================
        private void btnCancelarEditar_Click(object sender, RoutedEventArgs e)
        {
            //Limpiamos los campos
            LimpiarCampos();

            //Ocultamos botones de confirmarEditar y cancelarEditar
            btnConfirmarEditar.Visibility = Visibility.Hidden;
            btnCancelarEditar.Visibility = Visibility.Hidden;

            //Volvemos activar los botones agregar, editar y eliminar tambien id modificacion
            txtIdPaciente.IsEnabled = true;
            btnAgregarReceta.IsEnabled = true;
            btnEditarReceta.IsEnabled = true;
            btnEliminarReceta.IsEnabled = true;
        }

        //=================================
        //BÓTON PARA ELIMINAR RECETA
        //=================================
        private void btnEliminarReceta_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos los valores
            Receta recetaSeleccionada = (Receta)datagridRecetas.SelectedItem;

            if (recetaSeleccionada != null)
            {
                //Eliminamos la receta
                new RecetaController().EliminarReceta(recetaSeleccionada.ID_Receta);

                //Limpiamos los campos
                LimpiarCampos();

                //Actualizamos el datagrid de recetas
                btnBuscarPacienteReceta_Click(null, null);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una receta.");
            }
        }
    }
}
