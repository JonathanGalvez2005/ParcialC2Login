using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace login
{
    public partial class Form1 : Form
    {
        string cadena = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\jonat\OneDrive\Escritorio\parciall\login\login\usuarios.mdf;Integrated Security=True";
        int idSeleccionado = 0;
        Conexion_base_datos bd = new Conexion_base_datos();

        public Form1()
        {
            InitializeComponent();
            dgvUsuarios.SelectionChanged += (s, ev) =>
            {
                if (dgvUsuarios.CurrentRow != null)
                    CargarFila(dgvUsuarios.CurrentRow);
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM usuarios_usss121926", cn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvUsuarios.AutoGenerateColumns = false;
                dgvUsuarios.Columns[0].DataPropertyName = "idUsuario";
                dgvUsuarios.Columns[1].DataPropertyName = "usuario";
                dgvUsuarios.Columns[2].DataPropertyName = "clave";
                dgvUsuarios.Columns[3].DataPropertyName = "nombre";
                dgvUsuarios.Columns[4].DataPropertyName = "direccion";
                dgvUsuarios.Columns[5].DataPropertyName = "telefono";
                dgvUsuarios.DataSource = dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void MoverA(int indice)
        {
            if (dgvUsuarios.Rows.Count == 0) return;

            if (indice < 0) indice = 0;
            if (indice > dgvUsuarios.Rows.Count - 1) indice = dgvUsuarios.Rows.Count - 1;

            dgvUsuarios.ClearSelection();
            dgvUsuarios.CurrentCell = dgvUsuarios.Rows[indice].Cells[1];
            dgvUsuarios.Rows[indice].Selected = true;
        }

        private bool ObtenerIdSeleccionado()
        {
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.IsNewRow
                || dgvUsuarios.CurrentRow.Cells[0].Value == null
                || dgvUsuarios.CurrentRow.Cells[0].Value == DBNull.Value)
            {
                idSeleccionado = 0;
                return false;
            }

            idSeleccionado = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells[0].Value);
            return true;
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text.Length < 6 || txtUsuario.Text.Length > 16)
            {
                MessageBox.Show("El usuario debe tener entre 6 y 16 caracteres.");
                return;
            }
            if (txtClave.Text != txtConfirmar.Text)
            {
                MessageBox.Show("La contraseña y su confirmación no coinciden.");
                return;
            }
            if (!txtClave.Text.Any(char.IsLetter) || !txtClave.Text.Any(char.IsDigit))
            {
                MessageBox.Show("La contraseña debe llevar letras y números.");
                return;
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    string sql = "INSERT INTO usuarios_usss121926 (usuario, clave, nombre, direccion, telefono) " +
                                 "VALUES (@usuario, @clave, @nombre, @direccion, @telefono)";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@clave", txtClave.Text);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                    cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Usuario agregado correctamente.");
                LimpiarCampos();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            txtUsuario.Clear();
            txtClave.Clear();
            txtConfirmar.Clear();
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtUsuario.Focus();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow fila = dgvUsuarios.Rows[e.RowIndex];
            if (fila.Cells[0].Value == null || fila.Cells[0].Value == DBNull.Value) return;

            idSeleccionado = Convert.ToInt32(fila.Cells["idUsuario"].Value);
            txtUsuario.Text = fila.Cells["usuario"].Value.ToString();
            txtClave.Text = fila.Cells["clave"].Value.ToString();
            txtConfirmar.Text = fila.Cells["clave"].Value.ToString();
            txtNombre.Text = fila.Cells["nombre"].Value.ToString();
            txtDireccion.Text = fila.Cells["direccion"].Value.ToString();
            txtTelefono.Text = fila.Cells["telefono"].Value.ToString();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un registro de la tabla primero.");
                return;
            }

            if (txtUsuario.Text.Length < 6 || txtUsuario.Text.Length > 16)
            {
                MessageBox.Show("El usuario debe tener entre 6 y 16 caracteres.");
                return;
            }
            if (txtClave.Text != txtConfirmar.Text)
            {
                MessageBox.Show("La contraseña y su confirmación no coinciden.");
                return;
            }
            if (!txtClave.Text.Any(char.IsLetter) || !txtClave.Text.Any(char.IsDigit))
            {
                MessageBox.Show("La contraseña debe llevar letras y números.");
                return;
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    string sql = "UPDATE usuarios_usss121926 SET usuario=@usuario, clave=@clave, " +
                                 "nombre=@nombre, direccion=@direccion, telefono=@telefono " +
                                 "WHERE idUsuario=@id";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@clave", txtClave.Text);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                    cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                    cmd.Parameters.AddWithValue("@id", idSeleccionado);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Usuario modificado correctamente.");
                LimpiarCampos();
                idSeleccionado = 0;
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!ObtenerIdSeleccionado())
            {
                MessageBox.Show("Seleccione un registro de la tabla primero.");
                return;
            }

            DialogResult r = MessageBox.Show("¿Seguro que desea eliminar este usuario?",
                                             "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM usuarios_usss121926 WHERE idUsuario=@id", cn);
                    cmd.Parameters.AddWithValue("@id", idSeleccionado);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Usuario eliminado correctamente.");
                LimpiarCampos();
                idSeleccionado = 0;
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                CargarFila(dgvUsuarios.Rows[e.RowIndex]);
        }

        private void CargarFila(DataGridViewRow fila)
        {
            if (fila == null || fila.IsNewRow || fila.Cells[0].Value == null) return;

            idSeleccionado = Convert.ToInt32(fila.Cells[0].Value);
            txtUsuario.Text = fila.Cells[1].Value?.ToString();
            txtClave.Text = fila.Cells[2].Value?.ToString();
            txtConfirmar.Text = fila.Cells[2].Value?.ToString();
            txtNombre.Text = fila.Cells[3].Value?.ToString();
            txtDireccion.Text = fila.Cells[4].Value?.ToString();
            txtTelefono.Text = fila.Cells[5].Value?.ToString();
        }

        private void btnUltimoAlumno_Click(object sender, EventArgs e)
        {
            MoverA(dgvUsuarios.Rows.Count - 1);
        }

        private void btnPrimeroAlumno_Click(object sender, EventArgs e)
        {
            MoverA(0);
        }

        private void btnAnteriorAlumno_Click(object sender, EventArgs e)
        {
            int actual = dgvUsuarios.CurrentRow != null ? dgvUsuarios.CurrentRow.Index : 0;
            MoverA(actual - 1);
        }

        private void btnSiguienteAlumno_Click(object sender, EventArgs e)
        {
            int actual = dgvUsuarios.CurrentRow != null ? dgvUsuarios.CurrentRow.Index : -1;
            MoverA(actual + 1);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dgvUsuarios.ClearSelection();
            dgvUsuarios.CurrentCell = null;
            idSeleccionado = 0;
            LimpiarCampos();
        }
    }
}
