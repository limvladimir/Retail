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

namespace Retail
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Владимир\source\repos\Retail\Retail\Database1.mdf;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(connectionString);
            myConnection.Open();
            SqlCommand command = new SqlCommand("DELETE FROM [Product] WHERE [Id]=@Id", myConnection);
            command.Parameters.AddWithValue("Id", textBox1.Text);
            command.ExecuteNonQuery();
            myConnection.Close();
            
        }
    }
}
