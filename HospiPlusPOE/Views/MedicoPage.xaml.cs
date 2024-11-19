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

using System.Collections.ObjectModel;
namespace PlusHospi.Views
{
    public partial class MedicoPage : Page
    {

        public MedicoPage()
        {
            InitializeComponent();
        }


        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            cmbEspecialidad.SelectedIndex = -1;
            txtNumeroLicencia.Clear();
            txtTelefono.Clear();
            cmbEstado.SelectedIndex = -1;
        }
    }

}
