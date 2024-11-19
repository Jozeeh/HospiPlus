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

namespace PlusHospi.Views
{
    public partial class ConsultaPage : Page
    {

        public ConsultaPage()
        {
            InitializeComponent();
        }

        private void LimpiarCampos()
        {
            FechaConsulta.SelectedDate = null;
            txtDiagnostico.Clear();
            txtDescripcion.Clear();
            cmbMedico.SelectedIndex = -1;
            cmbPaciente.SelectedIndex = -1;
        }

        private void CancelarConsulta_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }
    }
}

