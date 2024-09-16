using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace qlbh1234
{
    public class DataAccess
    {
        SqlConnection objConnection = new SqlConnection();
        public DataAccess()
        {
            string strConnection = @"Data Source=LAPTOP-R275EEVN\MAY1;Initial Catalog=qlbhang;Integrated Security=True;";

            objConnection = new SqlConnection(strConnection);

        }
        
        public void UpdateData(string i_updateCommand)
        {
            try
            {
                objConnection.Open();
                Console.WriteLine("Connection succeeded");
                SqlCommand cmd = new SqlCommand(i_updateCommand, objConnection);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                if (objConnection != null)
                    objConnection.Close();
            }
        }

        public DataTable GetDataTable(string i_selectCommand)
        {
            DataTable dt = new DataTable();
            try
            {
                objConnection.Open();
                Console.WriteLine("Connection succeeded");
                SqlDataAdapter objAdapter = new SqlDataAdapter(i_selectCommand, objConnection);

                objAdapter.Fill(dt);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                if (objConnection != null)
                    objConnection.Close();
            }

            return dt;
        }
    }
}
