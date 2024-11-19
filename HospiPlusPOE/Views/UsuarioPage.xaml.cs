using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

//Importamos model/controller para poder utilizar la clase Usuario
using HospiPlusPOE.Models;
using HospiPlusPOE.Controllers;


namespace PlusHospi.Views
{
    public partial class UsuarioPage : Page
    {
        //Creamos coleccion observable para mostrar los usuarios en el DataGrid
        public ObservableCollection<Usuario> Usuarios { get; set; }

        public UsuarioPage()
        {
            InitializeComponent();

            //Inicializamos la coleccion observable
            Usuarios = new ObservableCollection<Usuario>();
            MostrarUsuarios();

            //Ocultamos botones de confirmar editar y cancelar editar
            btnConfirmarEditar.Visibility = Visibility.Hidden;
            btnCancelarEditar.Visibility = Visibility.Hidden;
        }

        public void MostrarUsuarios()
        {
            //Obtenemos los usuarios de la base de datos
            var usuarioDesdeBD = new UsuarioController().ObtenerUsuarios();

            //Mostramos los usuarios en el DataGrid
            Usuarios = new ObservableCollection<Usuario>(usuarioDesdeBD.Select(usuario => new Usuario
            {
                ID_Usuario = usuario.ID_Usuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Rol = usuario.Rol,
                Nickname = usuario.Nickname,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Password = usuario.Password
            }));

            datagridUsuario.ItemsSource = Usuarios;
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos el usuario seleccionado (el ? es para que no haya error si no se selecciona nada)
            Usuario? usuarioSeleccionado = datagridUsuario.SelectedItem as Usuario;

            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            } else
            {
                //Ejecutamos el método de EliminarUsuario en UsuarioController
                new UsuarioController().EliminarUsuario(usuarioSeleccionado.ID_Usuario);

                //Actualizamos el DataGrid
                MostrarUsuarios();
            }
        }
    }
}

