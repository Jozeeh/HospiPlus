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
    public class ConsultaController
    {

        private string _credencialesConexion;

        public ConsultaController()
        {
            _credencialesConexion = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }

        

    }
}
