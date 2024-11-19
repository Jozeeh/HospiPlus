using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Importamos model para poder utilizar la clase Usuario
using HospiPlusPOE.Models;

//Importacion librerias para la conexion a la base de datos
using System.Configuration;
using Microsoft.Data.SqlClient;

//Importamos librerias para mostrar mensajes
using System.Windows;

namespace HospiPlusPOE.Controllers
{
    public class UsuarioController
    {
        //Credenciales de la conexion a la base de datos
        private string _credencialesConexion;

        public UsuarioController()
        {
            //Credenciales de la conexion a la base de datos
            _credencialesConexion = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }

        //=================================
        //METODO PARA OBTENER LOS USUARIOS
        //=================================
        public List<Usuario> ObtenerUsuarios()
        {
            //Lista de usuarios
            var usuarios = new List<Usuario>();

            //Obtenemos los usuarios de la base de datos
            using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
            {
                conexion.Open();
                string query = "SELECT * FROM Usuario WHERE Estado = 'Activo'";

                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //Creamos un objeto de tipo Usuario
                        usuarios.Add(new Usuario
                        {
                            ID_Usuario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Rol = reader.GetString(3),
                            Nickname = reader.GetString(4),
                            Correo = reader.GetString(5),
                            Telefono = reader.GetString(6),
                            Password = reader.GetString(7)
                        });
                    }
                }
            }
            return usuarios;
        }

        //=================================
        //METODO PARA AGREGAR UN USUARIO
        //=================================
        public bool AgregarUsuario(string nombre, string apellido, string rol, string nickname, string correo, string telefono, string password)
        {
            bool usuarioAgregado = false;

            //Agregamos el usuario a la base de datos
            try
            {
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO Usuario (Nombre, Apellido, Rol, Nickname, Correo, Telefono, Password, Estado) VALUES (@nombre, @apellido, @rol, @nickname, @correo, @telefono, @password, @estado)";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@apellido", apellido);
                        command.Parameters.AddWithValue("@rol", rol);
                        command.Parameters.AddWithValue("@nickname", nickname);
                        command.Parameters.AddWithValue("@correo", correo);
                        command.Parameters.AddWithValue("@telefono", telefono);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@estado", "Activo");
                        command.ExecuteNonQuery();

                        usuarioAgregado = true;
                        conexion.Close();
                        MessageBox.Show("Usuario agregado correctamente", "Usuario agregado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el usuario: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return usuarioAgregado;
        }


        //===================================
        //METODO PARA DESACTIVAR UN USUARIOS
        //===================================
        public void DesactivarUsuario(int idUsuarioSeleccionado)
        {
            //Preguntamos si quiere eliminar el usuario
            MessageBoxResult result = MessageBox.Show("¿Está seguro que desea desactivar este usuario?", "Desactivar Usuario", MessageBoxButton.YesNo, MessageBoxImage.Question);

            //En caso sea si eliminamos el usuario de la base de datos
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                    {
                        conexion.Open();
                        string query = "UPDATE Usuario SET Estado = 'Inactivo' WHERE ID_Usuario = 6;";

                        using (SqlCommand command = new SqlCommand(query, conexion))
                        {
                            command.Parameters.AddWithValue("@idUsuario", idUsuarioSeleccionado);
                            command.ExecuteNonQuery();
                            conexion.Close();

                            MessageBox.Show("Usuario desactivado correctamente", "Usuario desactivado", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("Error al desactivar el usuario: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
