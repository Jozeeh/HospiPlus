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
using System.Windows.Shapes;
using PlusHospi.Views;

namespace HospiPlusPOE
{
    /// <summary>
    /// Lógica de interacción para MedicoView.xaml
    /// </summary>
    public partial class MedicoView : Window
    {
        public MedicoView()
        {
            InitializeComponent();

            //Mostramos citas
            AbrirCita_Click(null, null);
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Cierra la ventana
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas salir?",
                                                      "Confirmación de salida",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        // Cierra sesión y muestra la vista de login
        private void GoToLoginPage(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas cerrar sesión?",
                                                      "Confirmación de salida",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                LoginView loginView = new LoginView();
                loginView.Show();
                this.Close();
            }
        }

        private void AbrirReceta_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new RecetaPage());
        }

        private void AbrirCita_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new CitaPage());
        }

        private void AbrirConsulta_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new ConsultaPage());
        }

        private void AbrirExamen_Click(object sender, RoutedEventArgs e)
        {
            // Navega a la ExamenPage pasando el ID_Paciente como parámetro
            ContentArea.Navigate(new ExamenPage());
        }
    }
}
