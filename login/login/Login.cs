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
    public partial class Login : Form
    {
        string cadena = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\jonat\OneDrive\Escritorio\parciall\login\login\usuarios.mdf;Integrated Security=True";
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtClave.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text.Trim() == "" || txtClave.Text == "")
            {
                MessageBox.Show("Ingrese usuario y clave.");
                return;
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    string sql = "SELECT COUNT(*) FROM usuarios_usss121926 WHERE usuario=@usuario AND clave=@clave";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text.Trim());
                    cmd.Parameters.AddWithValue("@clave", txtClave.Text);

                    cn.Open();
                    int existe = (int)cmd.ExecuteScalar();

                    if (existe > 0)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Usuario o clave incorrectos. Acceso denegado.");
                        txtClave.Clear();
                        txtUsuario.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("¿Desea salir del sistema?", "Salir",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
