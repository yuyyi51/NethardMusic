using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MySqlHelperY
{
    public class MySqlHelper
    {
        protected MySqlConnection connection;

        public MySqlHelper(string constr)
        {
            connection = new MySqlConnection(constr);
        }

        public async Task<DataSet> ExecuteDataSetAsync(string command)
        {
            DataSet data = new DataSet();
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command, connection);
                await adapter.FillAsync(data);
                connection.Close();
                return data;
            }
            catch(Exception e)
            {
                connection.Close();
                throw e;
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string command)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                MySqlCommand com = new MySqlCommand(command, connection);
                return await com.ExecuteNonQueryAsync();
            }
            catch(Exception e)
            {
                connection.Close();
                throw e;
            }
        }
        public async Task<object> ExecuteScalarAsync(string command)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                MySqlCommand com = new MySqlCommand(command, connection);
                return await com.ExecuteScalarAsync();
            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }
    }
}
