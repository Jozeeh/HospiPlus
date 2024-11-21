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
                ID_FK_Usuario = m.ID_FK_Usuario,
                Nombre = m.Nombre,
                Apellido = m.Apellido,
                Rol = m.Rol,
                Especialidad = m.Especialidad,
                NumeroLicencia = m.NumeroLicencia,
                Nickname = m.Nickname,
                Correo = m.Correo,
                Telefono = m.Telefono,
                Estado = m.Estado,
                ID_Usuario = m.ID_Usuario,
            }));

            MedicoDataGrid.ItemsSource = Medicos;
        }

        private void btnEditarMedico_Click(object sender, RoutedEventArgs e)
        {
            if (MedicoDataGrid.SelectedItem is not Medico medicoSeleccionado)
            {
                MessageBox.Show("Seleccione un médico para editar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else
            {
                //Asignamos los valores a los combobox y id medico
                IDMedicoSeleccionado = medicoSeleccionado.ID_Medico;
                cmbEspecialidad.Text = medicoSeleccionado.Especialidad;
                txtLicencia.Text = medicoSeleccionado.NumeroLicencia;

                ConfirmarEditarButton.Visibility = Visibility.Visible;
                CancelarEdicionButton.Visibility = Visibility.Visible;

                EditarBtn.IsEnabled = false;
                DesactivarBtn.IsEnabled = false;
            } 
        }

        private void btnConfirmarEditar_Click(object sender, RoutedEventArgs e)
        {
            // Obtenemos los valores
            int idMedico = IDMedicoSeleccionado;
            string especialidadSeleccionada = cmbEspecialidad.Text;
            string numeroLicencia = txtLicencia.Text;

            //Verificamos que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(especialidadSeleccionada) || string.IsNullOrWhiteSpace(numeroLicencia))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else
            {
                // Ejecutamos el método de editar médico
                bool medicoEditado = new MedicoController().EditarMedico(IDMedicoSeleccionado, especialidadSeleccionada, numeroLicencia);

                // Si se edito
                if (medicoEditado == true)
                {
                    // Mostrar médicos y limpiar campos
                    MostrarMedicos();
                    LimpiarCampos();

                    // Ocultar botones y habilitar otros
                    ConfirmarEditarButton.Visibility = Visibility.Hidden;
                    CancelarEdicionButton.Visibility = Visibility.Hidden;
                    EditarBtn.IsEnabled = true;
                    DesactivarBtn.IsEnabled = true;
                }
            }
        }


        private void btnCancelarEdicion_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();

            ConfirmarEditarButton.Visibility = Visibility.Hidden;
            CancelarEdicionButton.Visibility = Visibility.Hidden;
            EditarBtn.IsEnabled = true;
            DesactivarBtn.IsEnabled = true;
        }

        private void btnDesactivarMedico_Click(object sender, RoutedEventArgs e)
        {
            if (MedicoDataGrid.SelectedItem is not Medico medicoSeleccionado)
            {
                MessageBox.Show("Seleccione un médico para desactivar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else
            {
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
