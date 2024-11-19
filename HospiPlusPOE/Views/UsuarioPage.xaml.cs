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

        //==========================================================
        //METODO PARA MOSTRAR LOS USUARIOS DESDE UsuarioController
        //==========================================================
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

        //======================================================
        //BOTON PARA DESACTIVAR USUARIO DESDE UsuarioController
        //======================================================
        private void btnDesactivar_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos el usuario seleccionado (el ? es para que no haya error si no se selecciona nada)
            Usuario? usuarioSeleccionado = datagridUsuario.SelectedItem as Usuario;

            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            } else
            {
                //Ejecutamos el método de EliminarUsuario en UsuarioController
                new UsuarioController().DesactivarUsuario(usuarioSeleccionado.ID_Usuario);

                //Actualizamos el DataGrid
                MostrarUsuarios();
            }
        }

        //====================================================
        //BOTON PARA AGREGAR USUARIO DESDE UsuarioController
        //====================================================
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos los valores de los TextBox
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string rol = cmbRol.Text;
            string nickname = txtNickname.Text;
            string correo = txtCorreo.Text;
            string telefono = txtTelefono.Text;
            string password = txtPassword.Password;

            //Validamos que los campos no estén vacíos
            if (nombre == "" || apellido == "" || rol == "" || nickname == "" || correo == "" || telefono == "" || password == "")
            {
                MessageBox.Show("Por favor, llene todos los campos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Ejecutamos el método de AgregarUsuario en UsuarioController
            bool validacion = new UsuarioController().AgregarUsuario(nombre, apellido, rol, nickname, correo, telefono, password);

            //Si el usuario se agrega correctamente, limpiamos los campos y actualizamos el DataGrid
            if (validacion == true)
            {
                //Actualizamos el DataGrid
                LimpiarCampos();
                MostrarUsuarios();
            }
        }

        private int IDUsuarioSeleccionado;
        //=============================================================
        //BOTON PARA EDITAR USUARIO (Solo asigna valores a los campos)
        //==============================================================
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos el usuario seleccionado y agregamos los valores a los campos
            Usuario? usuarioSeleccionado = datagridUsuario.SelectedItem as Usuario;

            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                IDUsuarioSeleccionado = usuarioSeleccionado.ID_Usuario;
                txtNombre.Text = usuarioSeleccionado.Nombre;
                txtApellido.Text = usuarioSeleccionado.Apellido;
                cmbRol.Text = usuarioSeleccionado.Rol;
                txtNickname.Text = usuarioSeleccionado.Nickname;
                txtCorreo.Text = usuarioSeleccionado.Correo;
                txtTelefono.Text = usuarioSeleccionado.Telefono;

                //Mostramos los botones de confirmar y cancelar editar
                btnConfirmarEditar.Visibility = Visibility.Visible;
                btnCancelarEditar.Visibility = Visibility.Visible;

                //Desactivamos agregar, editar y desactivar
                btnAgregar.IsEnabled = false;
                btnEditar.IsEnabled = false;
                btnDesactivar.IsEnabled = false;
            }
        }

        
        //=============================================================
        //BOTON PARA CONFIRMAR EDITAR USUARIO DESDE UsuarioController
        //=============================================================
        private void btnConfirmarEditar_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos los valores de los TextBox
            int idUsuario = IDUsuarioSeleccionado;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string rol = cmbRol.Text;
            string nickname = txtNickname.Text;
            string correo = txtCorreo.Text;
            string telefono = txtTelefono.Text;
            string password = txtPassword.Password;

            //Validamos que los campos no estén vacíos
            if (nombre == "" || apellido == "" || rol == "" || nickname == "" || correo == "" || telefono == "")
            {
                MessageBox.Show("Por favor, llene todos los campos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Ejecutamos el método de EditarUsuario en UsuarioController
            bool validacion = new UsuarioController().EditarUsuario(idUsuario, nombre, apellido, rol, nickname, correo, telefono, password);

            //Si el usuario se edita correctamente, limpiamos los campos y actualizamos el DataGrid
            if (validacion == true)
            {
                //Actualizamos el DataGrid
                LimpiarCampos();
                MostrarUsuarios();

                //Ocultamos los botones de confirmar y cancelar editar
                btnConfirmarEditar.Visibility = Visibility.Hidden;
                btnCancelarEditar.Visibility = Visibility.Hidden;

                //Activamos agregar, editar y desactivar
                btnAgregar.IsEnabled = true;
                btnEditar.IsEnabled = true;
                btnDesactivar.IsEnabled = true;
            }
        }

        //=======================================================
        //BOTON PARA CANCELAR EDITAR USUARIO (Limpia los campos)
        //=======================================================
        private void btnCancelarEditar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();

            //Ocultamos los botones de confirmar y cancelar editar
            btnConfirmarEditar.Visibility = Visibility.Hidden;
            btnCancelarEditar.Visibility = Visibility.Hidden;

            //Activamos agregar, editar y desactivar
            btnAgregar.IsEnabled = true;
            btnEditar.IsEnabled = true;
            btnDesactivar.IsEnabled = true;
        }

        //============================
        //METODO PARA LIMPIAR CAMPOS
        //============================
        public void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            cmbRol.SelectedIndex = -1;
            txtNickname.Clear();
            txtCorreo.Clear();
            txtTelefono.Clear();
            txtPassword.Clear();
        }
    }
}

