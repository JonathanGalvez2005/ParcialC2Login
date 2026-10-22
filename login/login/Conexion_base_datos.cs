using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace login
{
    public class Conexion_base_datos
    {
        string cadena = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\jonat\OneDrive\Escritorio\parciall\login\login\usuarios.mdf;Integrated Security=True";

        public DataSet ds;

        
        public void crearDs()
        {
            ds = new DataSet();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM usuarios_usss121926", cn);
                da.Fill(ds, "usuarios");
            }
        }
        public bool procesar(string sql, SqlParameter[] parametros)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    if (parametros != null) cmd.Parameters.AddRange(parametros);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        
        public bool mantenimiento_usuarios_usss121926(string accion, int id, string usuario,
            string clave, string nombre, string direccion, string telefono)
        {
            string sql = "";
            SqlParameter[] p;

            switch (accion)
            {
                case "agregar":
                    sql = "INSERT INTO usuarios_usss121926 (usuario, clave, nombre, direccion, telefono) " +
                          "VALUES (@usuario, @clave, @nombre, @direccion, @telefono)";
                    p = new SqlParameter[]
                    {
                        new SqlParameter("@usuario", usuario),
                        new SqlParameter("@clave", clave),
                        new SqlParameter("@nombre", nombre),
                        new SqlParameter("@direccion", direccion),
                        new SqlParameter("@telefono", telefono)
                    };
                    break;

                case "modificar":
                    sql = "UPDATE usuarios_usss121926 SET usuario=@usuario, clave=@clave, nombre=@nombre, " +
                          "direccion=@direccion, telefono=@telefono WHERE idUsuario=@id";
                    p = new SqlParameter[]
                    {
                        new SqlParameter("@usuario", usuario),
                        new SqlParameter("@clave", clave),
                        new SqlParameter("@nombre", nombre),
                        new SqlParameter("@direccion", direccion),
                        new SqlParameter("@telefono", telefono),
                        new SqlParameter("@id", id)
                    };
                    break;

                case "eliminar":
                    sql = "DELETE FROM usuarios_usss121926 WHERE idUsuario=@id";
                    p = new SqlParameter[] { new SqlParameter("@id", id) };
                    break;

                default:
                    return false;
            }

            return procesar(sql, p);   
        }

        
        public bool validarUsuario(string usuario, string clave)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM usuarios_usss121926 WHERE usuario=@u AND clave=@c", cn);
                cmd.Parameters.AddWithValue("@u", usuario);
                cmd.Parameters.AddWithValue("@c", clave);
                cn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
    }
}
