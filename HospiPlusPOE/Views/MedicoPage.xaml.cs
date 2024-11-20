using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HospiPlusPOE.Controllers;
using HospiPlusPOE.Models;

namespace PlusHospi.Views
{
    public partial class MedicoPage : Page
    {
        public ObservableCollection<Medico> Medicos { get; set; }
        private int IDMedicoSeleccionado;

        public MedicoPage()
        {
            InitializeComponent();
            Medicos = new ObservableCollection<Medico>();
            MostrarMedicos();

            // Ocultar botones de confirmación y cancelación
            ConfirmarEditarButton.Visibility = Visibility.Hidden;
            CancelarEdicionButton.Visibility = Visibility.Hidden;
        }

        private void MostrarMedicos()
        {
            var medicosDesdeBD = new MedicoController().ObtenerMedicos();

            Medicos = new ObservableCollection<Medico>(medicosDesdeBD.Select(m => new Medico
            {
                ID_Medico = m.ID_Medico,
                Especialidad = m.Especialidad,
                NumeroLicencia = m.NumeroLicencia,
            }));

            MedicoDataGrid.ItemsSource = Medicos;
        }

        private void btnAgregarMedico_Click(object sender, RoutedEventArgs e)
        {
            // Verificar si los campos están completos
            if (cmbEspecialidad.SelectedItem == null || string.IsNullOrWhiteSpace(txtLicencia.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var especialidadSeleccionada = ((ComboBoxItem)cmbEspecialidad.SelectedItem).Content.ToString();
            var numeroLicencia = txtLicencia.Text;

            var nuevoMedico = new Medico
            {
                Especialidad = especialidadSeleccionada,
                NumeroLicencia = numeroLicencia
            };

            // Llamar al método de agregar médico
            //  new MedicoController().AgregarMedico(
            //    nuevoMedico.Especialidad,
              //  nuevoMedico.NumeroLicencia
            //);

            // Mostrar médicos y limpiar campos
            MostrarMedicos();
            LimpiarCampos();
        }


        private void btnEditarMedico_Click(object sender, RoutedEventArgs e)
        {
            if (MedicoDataGrid.SelectedItem is not Medico medicoSeleccionado)
            {
                MessageBox.Show("Seleccione un médico para editar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IDMedicoSeleccionado = medicoSeleccionado.ID_Medico;
            txtLicencia.Text = medicoSeleccionado.NumeroLicencia;

            // Establecer la especialidad seleccionada en el ComboBox
            var especialidadSeleccionada = medicoSeleccionado.Especialidad;
            var itemEspecialidad = cmbEspecialidad.Items.Cast<ComboBoxItem>()
                                                        .FirstOrDefault(item => item.Content.ToString() == especialidadSeleccionada);
            if (itemEspecialidad != null)
            {
                cmbEspecialidad.SelectedItem = itemEspecialidad;
            }

            ConfirmarEditarButton.Visibility = Visibility.Visible;
            CancelarEdicionButton.Visibility = Visibility.Visible;

            AgregarBtn.IsEnabled = false;
            EditarBtn.IsEnabled = false;
            DesactivarBtn.IsEnabled = false;
        }


        private void btnConfirmarEditar_Click(object sender, RoutedEventArgs e)
        {
            // Verificar si los campos están completos
            if (cmbEspecialidad.SelectedItem == null || string.IsNullOrWhiteSpace(txtLicencia.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var especialidadSeleccionada = ((ComboBoxItem)cmbEspecialidad.SelectedItem).Content.ToString();
            var numeroLicencia = txtLicencia.Text;

            var medicoActualizado = new Medico
            {
                ID_Medico = IDMedicoSeleccionado,
                Especialidad = especialidadSeleccionada,
                NumeroLicencia = numeroLicencia
            };

            // Llamar al método de editar médico
            //   new MedicoController().EditarMedico(
            //     medicoActualizado.ID_Medico,
            //   medicoActualizado.Especialidad,
               // medicoActualizado.NumeroLicencia
            //);

            // Mostrar médicos y limpiar campos
            MostrarMedicos();
            LimpiarCampos();

            // Ocultar botones y habilitar otros
            ConfirmarEditarButton.Visibility = Visibility.Hidden;
            CancelarEdicionButton.Visibility = Visibility.Hidden;
            AgregarBtn.IsEnabled = true;
            EditarBtn.IsEnabled = true;
            DesactivarBtn.IsEnabled = true;
        }


        private void btnCancelarEdicion_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();

            ConfirmarEditarButton.Visibility = Visibility.Hidden;
            CancelarEdicionButton.Visibility = Visibility.Hidden;
            AgregarBtn.IsEnabled = true;
            EditarBtn.IsEnabled = true;
            DesactivarBtn.IsEnabled = true;
        }

        private void MedicoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MedicoDataGrid.SelectedItem != null)
            {
                var medicoSeleccionado = (Medico)MedicoDataGrid.SelectedItem;

                cmbEspecialidad.Text = medicoSeleccionado.Especialidad;
                txtLicencia.Text = medicoSeleccionado.NumeroLicencia;
            }
        }

        private void btnDesactivarMedico_Click(object sender, RoutedEventArgs e)
        {
            if (MedicoDataGrid.SelectedItem is not Medico medicoSeleccionado)
            {
                MessageBox.Show("Seleccione un médico para desactivar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                new MedicoController().DesactivarMedico(medicoSeleccionado.ID_Medico);
                MostrarMedicos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al desactivar el médico: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtBuscarLicencia_TextChanged(object sender, TextChangedEventArgs e)
        {
            string licenciaBuscada = txtBuscarLicencia.Text;

            if (string.IsNullOrWhiteSpace(licenciaBuscada))
            {
                MostrarMedicos();
            }
            else
            {
                var medicosFiltrados = Medicos.Where(m => m.NumeroLicencia.Contains(licenciaBuscada)).ToList();
                MedicoDataGrid.ItemsSource = medicosFiltrados;
            }
        }

        private void LimpiarCampos()
        {
            cmbEspecialidad.SelectedItem = null;
            txtLicencia.Clear();
        }
    }
    }
