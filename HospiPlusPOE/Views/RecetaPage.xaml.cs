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

namespace PlusHospi.Views
{
    /// <summary>
    /// Lógica de interacción para RecetaPage.xaml
    /// </summary>
    public partial class RecetaPage : Page
    {

        public RecetaPage()
        {
            InitializeComponent();
        }

        private void LimpiarCampos()
        {
            dpFecha.SelectedDate = null;
            txtMedicamentos.Clear();
            cmbConsulta.SelectedIndex = -1;
        }
    }
}
