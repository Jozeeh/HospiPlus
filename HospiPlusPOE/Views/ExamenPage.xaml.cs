using System;
using System.Windows;
using System.Windows.Controls;

namespace PlusHospi.Views
{
    public partial class ExamenPage : Page
    {
        public ExamenPage()
        {
            InitializeComponent();
        }


        private void LimpiarCampos()
        {
            // Limpiamos los campos después de guardar el examen
            cmbTipoExamen.SelectedIndex = -1;
            dpFechaExamen.SelectedDate = null;
            txtResultado.Clear();
            cmbConsulta.SelectedIndex = -1;
        }
    }
}
