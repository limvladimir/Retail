using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Retail
{
    public static class OperationWithDB
    {
        /// <summary>
        /// Выполнение хранимой процедуры
        /// </summary>
        /// <param name="spNAme">наименование хранимой процедуры</param>
        /// <param name="sqlParams">параметры хранимой процедуры</param>
        /// <returns></returns>
        public static DataTable ExecuteSP(string spNAme, List<SqlParameter> sqlParams = null)
        {
            string connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Владимир\source\repos\Retail\Retail\Database1.mdf;Integrated Security=True";
            SqlConnection sqlConnect = new SqlConnection();
            DataTable dt = new DataTable();

            try
            {
                //подключаемся к бд 
                sqlConnect = new SqlConnection(connect);
                sqlConnect.Open();

                //создаём запрос 
                SqlCommand sqlCommand = new SqlCommand(spNAme, sqlConnect);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddRange(sqlParams.ToArray());

                //Выполнение запроса
                SqlCommand sqlCmd = sqlConnect.CreateCommand();
                SqlDataReader dr = sqlCommand.ExecuteReader();

                //Заполнение таблицы и получение результата 
                dt.Load(dr);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConnect.Close();
            }

            return dt;

        }
        /// <summary>
        /// сохранение изменений в таблицу Equipment
        /// </summary>
        /// <param name="sqlquery"> sql запрос</param>
        /// <param name="dataSet"> таблица DataSet</param>
        public static void SaveToEquipmentT(string sqlquery, DataSet dataSet)
        {
            string connString = @"Data Source = localhost\SQLEXPRESS; Initial Catalog = Inventory; Integrated Security = True";
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlquery, connString);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.InsertCommand = new SqlCommand("InsEquipment", connection);
                    adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                    adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50, "equip_name"));
                    adapter.InsertCommand.Parameters.Add(new SqlParameter("@location", SqlDbType.NVarChar, 50, "equip_location"));
                    adapter.InsertCommand.Parameters.Add(new SqlParameter("@number", SqlDbType.BigInt, 0, "equip_number"));
                    SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@id", SqlDbType.BigInt, 0, "equip_id");
                    parameter.Direction = ParameterDirection.Output;
                    adapter.Update(dataSet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Строка не заполнена. ", "Сохранение данных в базе ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        /// <summary>
        /// сохранение изменений в таблицу Inventarization
        /// </summary>
        /// <param name="sqlquery"> sql запрос</param>
        /// <param name="dataSet"> таблица DataSet</param>
        public static void SaveToInventarizationT(string sqlquery, DataSet dataSet)
        {
            string connString = @"Data Source = localhost\SQLEXPRESS; Initial Catalog = Inventory; Integrated Security = True";
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlquery, connString);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.InsertCommand = new SqlCommand("InsInventarization", connection);
                    adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                    adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50, "inv_name"));
                    adapter.InsertCommand.Parameters.Add(new SqlParameter("@location", SqlDbType.NVarChar, 50, "inv_location"));
                    adapter.InsertCommand.Parameters.Add(new SqlParameter("@number", SqlDbType.BigInt, 0, "inv_num"));
                    SqlParameter param = adapter.InsertCommand.Parameters.Add("@date", SqlDbType.DateTime, 0, "inv_date");
                    SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@id", SqlDbType.BigInt, 0, "inv_id");

                    param.Value = DateTime.Now;
                    param.Direction = ParameterDirection.Output;
                    parameter.Direction = ParameterDirection.Output;
                    adapter.Update(dataSet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Строка не заполнена. ", "Сохранение данных в базе ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
