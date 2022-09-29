using System.Data;
using System.Data.OleDb;

namespace webservice
{
    class Sqlhelper
    {  
        //读取配置文件连接字符串
        public static readonly string connstr = System.Configuration.ConfigurationManager.AppSettings["connstr"].ToString();
        public static int ExecuteNonQuery(string cmdText, params OleDbParameter[] parameters)
        {
            using (OleDbConnection conn =new OleDbConnection(connstr))
            {
                conn.Open();
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.CommandTimeout = 3000;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public static object ExecuteScalar(string cmdText, params OleDbParameter[] parameters)
           {
                using (OleDbConnection conn = new OleDbConnection(connstr))
                   {
                      conn.Open();
                     using (OleDbCommand cmd = conn.CreateCommand())
                         {
                             cmd.CommandText = cmdText;
                             cmd.CommandTimeout = 3000;
                             cmd.Parameters.AddRange(parameters);
                             return cmd.ExecuteScalar();
                          }
                  }
        }
        public static DataTable ExecuteDataTable(string cmdText, params OleDbParameter[] parameters)
         {
                 using (OleDbConnection conn = new OleDbConnection(connstr))
                   {
                       conn.Open();
                       using (OleDbCommand cmd = conn.CreateCommand())
                         {
                            cmd.CommandText = cmdText;
                            cmd.CommandTimeout = 3000;
                            cmd.Parameters.AddRange(parameters);
                              using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                                   {
                                      DataTable dt = new DataTable();
                                      adapter.Fill(dt);
                                      return dt;
                                    }
                              }
                        }
                  }
        public static DataSet ExecuteDataSet(string cmdText, params OleDbParameter[] parameters)
          {
               using (OleDbConnection conn = new OleDbConnection(connstr))
                  {
                       conn.Open();
                       using (OleDbCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = cmdText;
                            cmd.CommandTimeout = 3000;
                            cmd.Parameters.AddRange(parameters);
                           using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                                {
                                      DataSet dt = new DataSet();
                                      adapter.Fill(dt);
                                      return dt;
                                 }
                           }
                      }
                }
        public static OleDbDataReader ExecuteDataReader(string cmdText, params OleDbParameter[] parameters)
        {
            OleDbConnection conn = new OleDbConnection(connstr);
            conn.Open();
            using (OleDbCommand cmd = conn.CreateCommand())
             {
                       cmd.CommandText = cmdText;
                       cmd.CommandTimeout = 3000;
                       cmd.Parameters.AddRange(parameters);
                       return cmd.ExecuteReader(CommandBehavior.CloseConnection);
             }
        }
    }
}
   

