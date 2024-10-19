using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Agenda
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarContactos();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            ConexionDB conexionDB = new ConexionDB();

            using (SqlCommand cmd = new SqlCommand("INSERT INTO Contactos (Nombre, Apellido, FechaNacimiento, Direccion, Genero, EstadoCivil, Movil, Telefono, CorreoElectronico) VALUES (@Nombre, @Apellido, @FechaNacimiento, @Direccion, @Genero, @EstadoCivil, @Movil, @Telefono, @CorreoElectronico)", conexionDB.AbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text);
                cmd.Parameters.AddWithValue("@FechaNacimiento", dtpFechaNacimiento.Value);
                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@Genero", cmbGenero.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@EstadoCivil", cmbEstadoCivil.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Movil", txtMovil.Text);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                cmd.Parameters.AddWithValue("@CorreoElectronico", txtCorreo.Text);

                cmd.ExecuteNonQuery();
            }


            
            conexionDB.CerrarConexion();
            MessageBox.Show("Registro insertado con éxito");

            CargarContactos();

        }



        private void CargarContactos()
        {
            ConexionDB conexionDB = new ConexionDB();

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Contactos", conexionDB.AbrirConexion()))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                dgvContactos.DataSource = dt;  // Cargamos los datos en el DataGridView
             
            }

            conexionDB.CerrarConexion();
        }

        private void dgvContactos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CargarContactos();

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            ConexionDB conexionDB = new ConexionDB();

            using (SqlCommand cmd = new SqlCommand("UPDATE Contactos SET Nombre = @Nombre, Apellido = @Apellido, FechaNacimiento = @FechaNacimiento, Direccion = @Direccion, Genero = @Genero, EstadoCivil = @EstadoCivil, Movil = @Movil, Telefono = @Telefono, CorreoElectronico = @CorreoElectronico WHERE CorreoElectronico = @Correo", conexionDB.AbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text);
                cmd.Parameters.AddWithValue("@FechaNacimiento", dtpFechaNacimiento.Value);
                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@Genero", cmbGenero.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@EstadoCivil", cmbEstadoCivil.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Movil", txtMovil.Text);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                cmd.Parameters.AddWithValue("@CorreoElectronico", txtCorreo.Text);
                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text); // Para identificar el contacto

                cmd.ExecuteNonQuery();
            }

            conexionDB.CerrarConexion();
            MessageBox.Show("Registro modificado con éxito");

            CargarContactos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ConexionDB conexionDB = new ConexionDB();
            string nombreBuscado = txtNombre.Text.Trim(); // Obtener el nombre del TextBox

            if (string.IsNullOrEmpty(nombreBuscado))
            {
                MessageBox.Show("Por favor, ingrese un nombre para buscar.");
                return; // Salir si el campo está vacío
            }

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Contactos WHERE Nombre LIKE @Nombre", conexionDB.AbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@Nombre", "%" + nombreBuscado + "%"); // Usar LIKE para buscar coincidencias

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Si se encontró el contacto, llenar los campos
                        txtNombre.Text = reader["Nombre"].ToString();
                        txtApellido.Text = reader["Apellido"].ToString();
                        dtpFechaNacimiento.Value = Convert.ToDateTime(reader["FechaNacimiento"]);
                        txtDireccion.Text = reader["Direccion"].ToString();
                        cmbGenero.SelectedItem = reader["Genero"].ToString();
                        cmbEstadoCivil.SelectedItem = reader["EstadoCivil"].ToString();
                        txtMovil.Text = reader["Movil"].ToString();
                        txtTelefono.Text = reader["Telefono"].ToString();
                        txtCorreo.Text = reader["CorreoElectronico"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el contacto");
                    }
                }
            }

            conexionDB.CerrarConexion();
            CargarContactos(); // Cargar los contactos nuevamente si es necesario

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ConexionDB conexionDB = new ConexionDB();

            using (SqlCommand cmd = new SqlCommand("DELETE FROM Contactos WHERE CorreoElectronico = @Correo", conexionDB.AbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                cmd.ExecuteNonQuery();
            }

            conexionDB.CerrarConexion();
            MessageBox.Show("Registro eliminado con éxito");
            CargarContactos();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Limpiar TextBoxes
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtMovil.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtCorreo.Text = string.Empty;

            // Limpiar ComboBoxes
            cmbGenero.SelectedIndex = -1; // O puedes usar 0 si deseas seleccionar el primer elemento
            cmbEstadoCivil.SelectedIndex = -1; // O puedes usar 0 si deseas seleccionar el primer elemento

            // Limpiar DateTimePicker
            dtpFechaNacimiento.Value = DateTime.Now; // O establece a una fecha predeterminada
        }
    }


    public class ConexionDB
    {
        private SqlConnection conexion;

        public ConexionDB()
        {
            string cadenaConexion = @"Server=DESKTOP-JALR7LH;Database=AgendaDB;Integrated Security=True;";
            conexion = new SqlConnection(cadenaConexion);
        }

        public SqlConnection AbrirConexion()
        {
            if (conexion.State == System.Data.ConnectionState.Closed)
                conexion.Open();
            return conexion;
        }

        public void CerrarConexion()
        {
            if (conexion.State == System.Data.ConnectionState.Open)
                conexion.Close();
        }
    }

    
}
