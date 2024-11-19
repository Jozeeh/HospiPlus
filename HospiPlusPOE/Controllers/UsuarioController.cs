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
                string query = "SELECT * FROM Usuario";

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
        //METODO PARA ELIMINAR UN USUARIOS
        //=================================
        public bool EliminarUsuario(int idUsuarioSeleccionado)
        {
            var usuarioEliminado = false;

            //Preguntamos si quiere eliminar el usuario
            MessageBoxResult result = MessageBox.Show("¿Está seguro que desea eliminar este usuario?", "Eliminar Usuario", MessageBoxButton.YesNo, MessageBoxImage.Question);

            //En caso sea si eliminamos el usuario de la base de datos
            if (result == MessageBoxResult.Yes)
            {
                using (SqlConnection conexion = new SqlConnection(_credencialesConexion))
                {
                    conexion.Open();
                    string query = "DELETE FROM Usuario WHERE ID_Usuario = @idUsuario";

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@idUsuario", idUsuarioSeleccionado);
                        command.ExecuteNonQuery();
                        conexion.Close();
                        usuarioEliminado = true;
                    }
                }
            }
            return usuarioEliminado;
        }

    }
}
