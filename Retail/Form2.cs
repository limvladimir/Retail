using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Retail
{
    public partial class Form2 : Form
    {
        SerialPort Serial;
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Владимир\source\repos\Retail\Retail\Database1.mdf;Integrated Security=True";
        public Form2()
        {
            InitializeComponent();

            LoadData();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public void Clear(DataGridView dataGridView)
        {
            while (dataGridView.Rows.Count > 1)
                for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                    dataGridView.Rows.Remove(dataGridView.Rows[i]);
        }

        private void DataCollection()
        {
            SqlConnection sqlConn = new SqlConnection(connectionString);
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            DataTable tableInvent = new DataTable();

            if (Serial.IsOpen)
            {
                string data = Serial.ReadExisting();

                char[] separator = { '\r', '\n', ',' };
                if (data != Serial.ReadExisting())
                {
                    foreach (string s in data.Split(separator, StringSplitOptions.RemoveEmptyEntries))  //разбиваем на строки
                    {
                        string[] ss = s.Split(';');
                        sqlParams.Add(new SqlParameter("Name", ss[0]));
                        sqlParams.Add(new SqlParameter("Kind", ss[1]));
                        sqlParams.Add(new SqlParameter("Wholesale_price", ss[2]));
                        sqlParams.Add(new SqlParameter("Retail_price", ss[3]));
                        tableInvent = OperationWithDB.ExecuteSP("InsQrData", sqlParams);
                    }
                }
            }
        }
        
            
            
        private void LoadData()
        {
            Clear(dataGridView1);
            SqlDataReader reader = null;
            //Строка подключения к базе данных
            //Класс для подключения к базе данных
            SqlConnection myConnection = new SqlConnection(connectionString);
            //Открываем соеденение для подключения
            myConnection.Open();
            string query = "SELECT * FROM Product ORDER BY id";
            SqlCommand command = new SqlCommand(query,myConnection);
            //Считывание данных с БД
            reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[5]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
            }
            reader.Close();
            myConnection.Close();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);

        }
        private void GetComPort()
        {
            if (SerialPort.GetPortNames() != null)
            {
                try
                {
                    ComPort_tStripComboBox.Items.Clear();
                    foreach (string portName in SerialPort.GetPortNames())
                    {
                        ComPort_tStripComboBox.Items.Add(portName);
                    }
                    ComPort_tStripComboBox.SelectedIndex = 0;

                    String sname = (string)ComPort_tStripComboBox.SelectedItem;
                    Serial = new SerialPort(sname, 9600, System.IO.Ports.Parity.None, 8, StopBits.One);

                }
                catch
                {
                    MessageBox.Show("Список портов пуст!" + '\n' + "Подключите Bluetooth, для получения COM-порта.", "Получить список портов", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (Serial.IsOpen)
            {
                Serial.Dispose();
                Serial.Close();
            }
            else
            {
                MessageBox.Show("СOM-порт не выбран", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void ConnectToComPort()
        {
            try
            {
                if (!Serial.IsOpen)
                {
                    try
                    {
                        Serial.PortName = ((string)ComPort_tStripComboBox.SelectedItem);
                        Serial.Open();
                        timer1.Start();
                        ConnBluetooth_tStripBtn.Text = "Отключение";
                    }

                    catch (IOException ex)
                    {
                        // MessageBox.Show(ex.Message + "Включите Bluetooth на вашему мобильном устройстве! " + '\n' + "Включите приложение 'Терминал сбора данных'!", "Подключение не удалось", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show(ex.Message, "Подключение не удалось", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Serial.Dispose();
                        Serial.Close();
                        ConnBluetooth_tStripBtn.Text = "Подключить";
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show(ex.Message, "Подключение не удалось", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show(ex.Message + "Включите Bluetooth на вашему мобильном устройстве! " + '\n' + "Включите приложение 'Терминал сбора данных'!", "Подключение не удалось", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    Serial.Dispose();
                    Serial.Close();
                    timer1.Stop();
                    ConnBluetooth_tStripBtn.Text = "Подключение";
                }
            }
            catch
            {
                MessageBox.Show("Нет данных COM-порта ", "Подключение не удалось", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void регистрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 fm2 = new Form3();
            fm2.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 fm3 = new Form4();
            fm3.ShowDialog();
        }

        private void ComPort_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            GetComPort();
        }

        private void ComPort_tStripComboBox_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ConnectToComPort();
        }
    }
}
