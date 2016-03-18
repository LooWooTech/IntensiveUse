using IntensiveUse.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntensiveUse.Console
{
    public static class MySQLHelper
    {
        private static string ConnectionString { get; set; }
        static MySQLHelper()
        {
            ConnectionString = string.Format("server=10.22.102.90;user id=loowootech;passsword=Ztop123456;database=intensiveuse");
        }

        public static UploadFile GetNation()
        {
            using (var connection=new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select ID,FileName,SavePath,FileTypeName from files where AnalyzeFlag=true";
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new UploadFile
                        {
                            ID = int.Parse(reader[0].ToString()),
                            FileName = reader[1].ToString(),
                            SavePath = reader[2].ToString(),
                            FileTypeName = reader[3].ToString()
                        };
                    }
                }
                connection.Clone();
            }
            return null;
        }

        public static void Finish(int id)
        {
            using (var connection=new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("UPDATE files set AnalyzeFlag=0 where ID= {0}", id);
                    command.ExecuteNonQuery();
                }

                connection.Clone();
            }
        }
    }
}
