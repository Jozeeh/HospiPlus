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
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace HospiPlusPOE.Controllers
{
    public class ReporteController
    {
        private string _connectionString;

        public ReporteController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["conexionSqlServer"].ConnectionString;
        }

        //==============================
        //    REPORTE DE EXPEDIENTE
        //==============================
        public void generarReporteExpediente(string fileName, int idPaciente, string ubicacionGuardar)
        {
            // Crear documento
            PdfDocument documento = new PdfDocument();
            documento.Info.Title = "Reporte Expediente";

            // Agregar página
            PdfPage pagina = documento.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(pagina);

            // Configurar fuentes
            XFont fuenteTitulo = new XFont("Arial", 16);
            XFont fuenteTexto = new XFont("Arial", 12);
            XFont fuenteSubtitulo = new XFont("Arial", 14);

            // Encabezado
            gfx.DrawString("Reporte de Expediente", fuenteTitulo, XBrushes.Black, new XRect(0, 20, pagina.Width, 30), XStringFormats.Center);

            // Logo
            string rutaLogo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Images\hospital.png");
            XImage logo = XImage.FromFile(rutaLogo);
            gfx.DrawImage(logo, 50, 20, 100, 100); // Ajusta tamaño y posición del logo

            int pacienteEncontrado = 0;

            //Obtenemos la informacion del paciente
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT " +
                    "ID_Paciente, " +
                    "Nombre, " +
                    "Apellido, " +
                    "Sexo, " +
                    "Correo, " +
                    "Telefono, " +
                    "DUI, " +
                    "Direccion, " +
                    "Seguro_Medico, " +
                    "Fecha_Nacimiento, " +
                    "Estado, " +
                    "Contacto_Emergencia_Nombre, " +
                    "Contacto_Emergencia_Telefono, " +
                    "Contacto_Emergencia_Relacion " +
                    "FROM  Paciente " +
                    "WHERE ID_Paciente = @ID_Paciente;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID_Paciente", idPaciente);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    pacienteEncontrado = 1;

                    //Agregamos la informacion del paciente al pdf
                    //Información del paciente
                    gfx.DrawString("Paciente", fuenteSubtitulo, XBrushes.Black, new XRect(50, 140, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    //Separador abajo de paciente
                    gfx.DrawLine(XPens.Black, 50, 155, pagina.Width - 50, 155);
                    gfx.DrawString("Nombre: " + reader.GetString(1) + " " + reader.GetString(2), fuenteTexto, XBrushes.Black, new XRect(50, 160, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Sexo: " + reader.GetString(3), fuenteTexto, XBrushes.Black, new XRect(50, 180, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Correo: " + reader.GetString(4), fuenteTexto, XBrushes.Black, new XRect(50, 200, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Telefono: " + reader.GetString(5), fuenteTexto, XBrushes.Black, new XRect(50, 220, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("DUI: " + reader.GetString(6), fuenteTexto, XBrushes.Black, new XRect(50, 240, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Direccion: " + reader.GetString(7), fuenteTexto, XBrushes.Black, new XRect(50, 260, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Seguro Medico: " + reader.GetString(8), fuenteTexto, XBrushes.Black, new XRect(50, 280, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Fecha Nacimiento: " + reader.GetDateTime(9).ToString("dd/MM/yyyy"), fuenteTexto, XBrushes.Black, new XRect(50, 300, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Estado: " + reader.GetString(10), fuenteTexto, XBrushes.Black, new XRect(50, 320, pagina.Width - 100, 20), XStringFormats.TopLeft);

                    //Información de contacto de emergencia
                    gfx.DrawString("Contacto de Emergencia", fuenteSubtitulo, XBrushes.Black, new XRect(50, 340, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    //Separador abajo de contacto de emergencia
                    gfx.DrawLine(XPens.Black, 50, 355, pagina.Width - 50, 355);
                    gfx.DrawString("Nombre: " + reader.GetString(11), fuenteTexto, XBrushes.Black, new XRect(50, 360, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Telefono: " + reader.GetString(12), fuenteTexto, XBrushes.Black, new XRect(50, 380, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Relacion: " + reader.GetString(13), fuenteTexto, XBrushes.Black, new XRect(50, 400, pagina.Width - 100, 20), XStringFormats.TopLeft);
                }
            }

            //Obtenemos las ultimas 3 consultas del paciente
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 3 " +
                    "Consulta.ID_Consulta, " +
                    "Consulta.ID_FK_Cita, " +
                    "Cita.Fecha AS Fecha_Cita, " +
                    "Consulta.Sintomas, " +
                    "Consulta.Diagnostico " +
                    "FROM Consulta " +
                    "INNER JOIN  Cita ON Consulta.ID_FK_Cita = Cita.ID_Cita " +
                    "WHERE  Cita.ID_FK_Paciente = @ID_Paciente " +
                    "ORDER BY  Cita.Fecha DESC;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID_Paciente", idPaciente);

                SqlDataReader reader = command.ExecuteReader();

                //Agregamos las consultas al pdf
                int y = 420;
                while (reader.Read())
                {
                    //Información de la consulta
                    gfx.DrawString("Consulta", fuenteSubtitulo, XBrushes.Black, new XRect(50, y, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    //Separador abajo de consulta
                    gfx.DrawLine(XPens.Black, 50, y + 15, pagina.Width - 50, y + 15);
                    gfx.DrawString("Fecha: " + reader.GetDateTime(2).ToString("dd/MM/yyyy"), fuenteTexto, XBrushes.Black, new XRect(50, y + 20, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Sintomas: " + reader.GetString(3), fuenteTexto, XBrushes.Black, new XRect(50, y + 40, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Diagnostico: " + reader.GetString(4), fuenteTexto, XBrushes.Black, new XRect(50, y + 60, pagina.Width - 100, 20), XStringFormats.TopLeft);

                    y += 80;
                }
            }

            //Obtenemos la ultima receta
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 1 " +
                    "Receta.ID_Receta, " +
                    "Receta.ID_FK_Consulta, " +
                    "Consulta.ID_FK_Cita, " +
                    "Consulta.Diagnostico, " +
                    "Receta.Medicamento, " +
                    "Receta.Dosis, " +
                    "Receta.Duracion " +
                    "FROM Receta " +
                    "INNER JOIN Consulta ON Receta.ID_FK_Consulta = Consulta.ID_Consulta " +
                    "INNER JOIN Cita ON Consulta.ID_FK_Cita = Cita.ID_Cita " +
                    "WHERE Cita.ID_FK_Paciente = @ID_Paciente " +
                    "ORDER BY Consulta.ID_Consulta DESC;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID_Paciente", idPaciente);

                SqlDataReader reader = command.ExecuteReader();

                //Agregamos la receta al pdf
                int y = 680;
                while (reader.Read())
                {
                    //Información de la receta
                    gfx.DrawString("Receta", fuenteSubtitulo, XBrushes.Black, new XRect(50, y, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    //Separador abajo de receta
                    gfx.DrawLine(XPens.Black, 50, y + 15, pagina.Width - 50, y + 15);
                    gfx.DrawString("Medicamento: " + reader.GetString(4), fuenteTexto, XBrushes.Black, new XRect(50, y + 20, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Dosis: " + reader.GetString(5), fuenteTexto, XBrushes.Black, new XRect(50, y + 40, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Duracion: " + reader.GetString(6), fuenteTexto, XBrushes.Black, new XRect(50, y + 60, pagina.Width - 100, 20), XStringFormats.TopLeft);

                    y += 80;
                }
            }


            //Si se encontro paciente se guarda el documento
            if (pacienteEncontrado > 0)
            {
                // Guardamos el documento
                string fullPath = System.IO.Path.Combine(ubicacionGuardar, fileName);
                documento.Save(fullPath);
                MessageBox.Show($"Reporte guardado en: {fullPath}");

                //Reiniciamos paciente encontrado
                pacienteEncontrado = 0;
            }
            else
            {
                MessageBox.Show("No se encontró el paciente");
            }

        }

        //==================================================
        //MÉTODO PARA REPORTE DE CONSULTAS SEGUN ID MEDICO
        //==================================================
        public void generarReporteConsultas(string fileName, int idMedico, string ubicacionGuardar)
        {
            // Crear documento
            PdfDocument documento = new PdfDocument();
            documento.Info.Title = "Reporte Consultas Medico";

            // Agregar página
            PdfPage pagina = documento.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(pagina);

            // Configurar fuentes
            XFont fuenteTitulo = new XFont("Arial", 16);
            XFont fuenteTexto = new XFont("Arial", 12);
            XFont fuenteSubtitulo = new XFont("Arial", 14);

            // Encabezado
            gfx.DrawString("Reporte de Consultas", fuenteTitulo, XBrushes.Black, new XRect(0, 20, pagina.Width, 30), XStringFormats.Center);

            // Logo
            string rutaLogo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Images\hospital.png");
            XImage logo = XImage.FromFile(rutaLogo);
            gfx.DrawImage(logo, 50, 20, 100, 100); // Ajusta tamaño y posición del logo

            int medicoEncontrado = 0;

            //Obtenemos la informacion del medico
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT " +
                    "Usuario.Nombre, " +
                    "Usuario.Apellido, " +
                    "Usuario.Correo, " +
                    "Usuario.Telefono, " +
                    "Medico.Especialidad, " +
                    "Medico.NumeroLicencia " +
                    "FROM Medico " +
                    "INNER JOIN Usuario ON Medico.ID_FK_Usuario = Usuario.ID_Usuario " +
                    "WHERE Medico.ID_Medico= @ID_Medico;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID_Medico", idMedico);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    medicoEncontrado = 1;

                    //Agregamos la informacion del medico al pdf
                    //Información del medico
                    gfx.DrawString("Medico", fuenteSubtitulo, XBrushes.Black, new XRect(50, 140, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    //Separador abajo de medico
                    gfx.DrawLine(XPens.Black, 50, 155, pagina.Width - 50, 155);
                    gfx.DrawString("Nombre: " + reader["Nombre"].ToString() + " " + reader["Apellido"].ToString(), fuenteTexto, XBrushes.Black, new XRect(50, 160, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Correo: " + reader["Correo"].ToString(), fuenteTexto, XBrushes.Black, new XRect(50, 180, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Telefono: " + reader["Telefono"].ToString(), fuenteTexto, XBrushes.Black, new XRect(50, 200, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Especialidad: " + reader["Especialidad"].ToString(), fuenteTexto, XBrushes.Black, new XRect(50, 220, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Numero de Licencia: " + reader["NumeroLicencia"].ToString(), fuenteTexto, XBrushes.Black, new XRect(50, 240, pagina.Width - 100, 20), XStringFormats.TopLeft);
                }

            }

            //Obtenemos las consultas del medico
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT " +
                    "Paciente.Nombre AS Nombre_Paciente, " +
                    "Paciente.Apellido AS Apellido_Paciente, " +
                    "Consulta.Sintomas, " +
                    "Consulta.Diagnostico, " +
                    "Cita.Hora AS Hora_Cita FROM Consulta " +
                    "INNER JOIN Cita ON Consulta.ID_FK_Cita = Cita.ID_Cita " +
                    "INNER JOIN Paciente ON Cita.ID_FK_Paciente = Paciente.ID_Paciente " +
                    "WHERE Cita.ID_FK_Medico = @ID_Medico ORDER BY Cita.Hora ASC;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID_Medico", idMedico);

                SqlDataReader reader = command.ExecuteReader();

                //Agregamos las consultas al pdf
                int y = 260;

                while (reader.Read())
                {
                    //Información de la consulta
                    gfx.DrawString("Consulta", fuenteSubtitulo, XBrushes.Black, new XRect(50, y, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    //Separador abajo de consulta
                    gfx.DrawLine(XPens.Black, 50, y + 15, pagina.Width - 50, y + 15);
                    gfx.DrawString("Paciente: " + reader["Nombre_Paciente"].ToString() + " " + reader["Apellido_Paciente"].ToString(), fuenteTexto, XBrushes.Black, new XRect(50, y + 20, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Sintomas: " + reader["Sintomas"].ToString(), fuenteTexto, XBrushes.Black, new XRect(50, y + 40, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Diagnostico: " + reader["Diagnostico"].ToString(), fuenteTexto, XBrushes.Black, new XRect(50, y + 60, pagina.Width - 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString("Hora Cita: " + reader["Hora_Cita"].ToString(), fuenteTexto, XBrushes.Black, new XRect(50, y + 80, pagina.Width - 100, 20), XStringFormats.TopLeft);

                    y += 100;
                }

            }

            //Si se encontro paciente se guarda el documento
            if (medicoEncontrado > 0)
            {
                // Guardamos el documento
                string fullPath = System.IO.Path.Combine(ubicacionGuardar, fileName);
                documento.Save(fullPath);
                MessageBox.Show($"Reporte guardado en: {fullPath}");

                //Reiniciamos paciente encontrado
                medicoEncontrado = 0;
            }
            else
            {
                MessageBox.Show("No se encontró el paciente");
            }

        }
    }
}
