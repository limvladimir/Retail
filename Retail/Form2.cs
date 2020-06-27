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
        string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Владимир\source\repos\Retail — Тест1\Retail\Database1.mdf;Integrated Security = True";
        //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Владимир\source\repos\Retail\Retail\Database1.mdf;Integrated Security=True";
        string product = "SELECT * FROM Product ORDER BY id";
        SerialPort Serial;
        string Date;
        public Form2()
        {
            InitializeComponent();

            LoadData(product,dataGridView1,7);
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
                        string sp = "InsQR";
                        string[] ss = s.Split(';');
                        sqlParams.Add(new SqlParameter("name", ss[0]));
                        sqlParams.Add(new SqlParameter("kind", ss[1]));
                        sqlParams.Add(new SqlParameter("price", ss[2]));
                        sqlParams.Add(new SqlParameter("retail", ss[3]));
                        tableInvent = OperationWithDB.ExecuteSP(sp, sqlParams);
                        sqlParams.Clear();
                        string query = "SELECT * FROM Scanned ORDER BY id";
                        LoadData(query, dataGridView2,6);
                    }
                }
            }
        }



        private void LoadData(string query, DataGridView dataGridView, int size)
        {
            Clear(dataGridView);
            SqlDataReader reader = null;
            //Строка подключения к базе данных
            //Класс для подключения к базе данных
            SqlConnection myConnection = new SqlConnection(connectionString);
            //Открываем соеденение для подключения
            myConnection.Open();
            SqlCommand command = new SqlCommand(query,myConnection);
            //Считывание данных с БД
            reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[size]);
                for (int i = 0; i < size; i++)
                {
                    data[data.Count - 1][i] = reader[i].ToString();
                }

            }
            reader.Close();
            myConnection.Close();
            foreach (string[] s in data)
                dataGridView.Rows.Add(s);

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
            LoadData(product,dataGridView1,7);
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

        private void button5_Click(object sender, EventArgs e)
        {
                  
        }
        private void ResultInvent(string sql, string dataMember, DataGridView dataGrid)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet dst = new DataSet();
            connection.Open();
            dataadapter.Fill(dst, dataMember);
            connection.Close();
            dataGrid.DataSource = dst;
            dataGrid.DataMember = dataMember;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //ResultInvent("SELECT * FROM Inventarization i1 WHERE  EXISTS(SELECT * FROM EquipmentTable  i2 WHERE i1.inv_num = i2.equip_number)", "Inventarization", ResultDataGrid);
            LoadData("SELECT Id, Item_name, Kind, Wholesale_price, Retail_price FROM Scanned i1 WHERE  EXISTS(SELECT * FROM Product i2 WHERE  i2.Name = i1.Item_name AND i2.Kind = i1.Kind AND i2.Wholesale_price=i1.Wholesale_price AND i2.Retail_price=i1.Retail_price )",dataGridView3,5);
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Date = DateTime.Now.ToShortDateString();
            DataCollection();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LoadData("SELECT * FROM Product WHERE Condition='Продан'", dataGridView1, 7);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
