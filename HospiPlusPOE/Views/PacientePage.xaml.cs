using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PlusHospi.Views
{
    public partial class PacientePage : Page
    {

        public PacientePage()
        {
            InitializeComponent();
        }


        private void LimpiarCampos()
        {
            NombreTextBox.Clear();
            ApellidoTextBox.Clear();
            FechaNacimientoPicker.SelectedDate = null;
            DireccionTextBox.Clear();
            SeguroMedicoTextBox.Clear();
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
