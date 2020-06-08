using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;
using Microsoft.Win32;
using QRCoder;

namespace Retail
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            QRCodeGenerator QG = new QRCodeGenerator();
            var View = textBox1.Text;
            var WholesalePrice = textBox2.Text;
            var RetailPrice = textBox3.Text;
            var QrImage = View + WholesalePrice + RetailPrice;
            var MyData = QG.CreateQrCode(QrImage, QRCodeGenerator.ECCLevel.M);
            var Data = new QRCode(MyData);
            pictureBox1.Image = Data.GetGraphic(50);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                System.Windows.Forms.SaveFileDialog savedialog = new System.Windows.Forms.SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                savedialog.OverwritePrompt = true;
                savedialog.CheckPathExists = true;
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK) 
                {
                    try
                    {
                        pictureBox1.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (label6.Visible)
                label6.Visible = false;

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) &&
                !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) )
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Владимир\source\repos\Retail\Retail\Database1.mdf;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(connectionString);
                myConnection.Open();
                /*var Name = textBox1.Text;
                var Kind = textBox2.Text;
                var Wholesale_price = textBox3.Text;
                var Retail_price = textBox4.Text;*/
                //SqlCommand command = new SqlCommand("INSERT INTO [Product] (Name,Kind,Wholesale_price,Retail_price)VALUES(Name,Kind,10000,11000)", myConnection);
                SqlCommand command = new SqlCommand("INSERT INTO [Product] (Name, Kind, Wholesale_price, Retail_price)VALUES(@Name, @Kind, @Wholesale_price, @Retail_price)", myConnection);
                command.Parameters.AddWithValue("Name", textBox1.Text);
                command.Parameters.AddWithValue("Kind", textBox2.Text);
                command.Parameters.AddWithValue("Wholesale_price", textBox3.Text);
                command.Parameters.AddWithValue("Retail_price", textBox4.Text);
                command.ExecuteNonQuery();
                myConnection.Close();
            }
            else
            {
                label6.Visible = true;
                label6.Text = "Все поля должны быть заполнены!";
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
