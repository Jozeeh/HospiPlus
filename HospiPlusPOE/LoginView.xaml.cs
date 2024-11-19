using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Librerias para la conexion a la base de datos
using Microsoft.Data.SqlClient;
using System.Data;

namespace HospiPlusPOE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        //Credenciales de la conexion a la base de datos
        private string _credencialesConexion;

        public LoginView()
        {
            InitializeComponent();

            //Credenciales de la conexion a la base de datos
            _credencialesConexion = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }

        //==========================
        //BOTÓN PARA INICIAR SESIÓN
        //==========================
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            //Validacion de campos vacios
            if (txtUsername.Text == "" || txtPassword.Password == "")
            {
                MessageBox.Show("Por favor, ingrese su nombre de usuario y contraseña", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Conexion a la base de datos
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    SqlCommand query = new SqlCommand("SELECT * FROM Usuario WHERE Nickname = @nickname AND password = @password AND Estado = 'Activo'", conexion);
                    query.Parameters.AddWithValue("@nickname", txtUsername.Text);
                    query.Parameters.AddWithValue("@password", txtPassword.Password);
                    SqlDataReader reader = query.ExecuteReader();

                    if (reader.Read())
                    {
                        //Si el usuario y contraseña son correctos, se abre la ventana principal
                        AdminView irAdminView = new AdminView();
                        irAdminView.Show();
                        this.Close();

                    }
                    else
                    {
                        //Si el usuario y contraseña son incorrectos, se muestra un mensaje de error
                        MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    conexion.Close();
                }
            }

        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}