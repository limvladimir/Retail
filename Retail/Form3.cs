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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label8.Visible)
                label8.Visible = false;

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) &&
                !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text)) 

            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Владимир\source\repos\Retail\Retail\Database1.mdf;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(connectionString);
                myConnection.Open();
                SqlCommand command = new SqlCommand("UPDATE [Product] SET [Name]=@Name, [Kind]=@Kind, [Wholesale_price]=@Wholesale_price, [Retail_price]=@Retail_price WHERE [Id]=@Id", myConnection);
                command.Parameters.AddWithValue("Id", textBox1.Text);
                command.Parameters.AddWithValue("Name", textBox2.Text);
                command.Parameters.AddWithValue("Kind", textBox3.Text);
                command.Parameters.AddWithValue("Wholesale_price", textBox4.Text);
                command.Parameters.AddWithValue("Retail_price", textBox5.Text);
                command.ExecuteNonQuery();
                myConnection.Close();

            }

            else
            {
                label6.Visible = true;
                label6.Text = "Все поля должны быть заполнены!";
            }
        }
    }
}
