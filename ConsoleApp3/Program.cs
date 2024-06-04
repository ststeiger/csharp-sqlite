
namespace ConsoleApp3
{

#if false
    using Connection = System.Data.SQLite.SQLiteConnection;
    using ConnectionStringBuilder = System.Data.SQLite.SQLiteConnectionStringBuilder;
#else 
    using Connection = Community.CsharpSqlite.SQLiteClient.SqliteConnection;
    using ConnectionStringBuilder = Community.CsharpSqlite.SQLiteClient.SqliteConnectionStringBuilder;
#endif 



    internal class Program
    {


        public static System.IO.Stream ByteArrayAsStream(byte[] array)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(array);
            return memoryStream;
        } // End Function ByteArrayAsStream 


        static void Main(string[] args)
        {
            System.IO.Stream stream = null;
            int x = 132; int y = 164; int zoom = 8;
            string cs = "Data Source=D:\\username\\Downloads\\COR_switzerland.mbtiles;Version=3; Read Only=True;";
            cs = "Data Source=file:///D:/username/Downloads/COR_switzerland.mbtiles;Version=3;Read Only=True;";

            ConnectionStringBuilder csb = new ConnectionStringBuilder();
            // csb.ConnectionString = "Data Source=D:\\username\\Downloads\\COR_switzerland.mbtiles;Version=3; Read Only=True;"; // BAD
            // csb.Uri = "file:///D:/username/Downloads/COR_switzerland.mbtiles";
            csb.FileName = "D:/username/Downloads/COR_switzerland.mbtiles";
            
            csb.Version = 3;
            csb.ReadOnly = true;
            cs = csb.ConnectionString;
            System.Console.WriteLine(cs);


            using (System.Data.Common.DbConnection conn = new Connection(cs))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                using (System.Data.Common.DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT * FROM tiles WHERE tile_column = {0} and tile_row = {1} and zoom_level = {2}", x, y, zoom);

                    using (System.Data.Common.DbDataReader reader = cmd.ExecuteReader())
                    {
                        int ord = reader.GetOrdinal("tile_data");

                        if (reader.Read())
                        {
                            //stream = reader.GetStream(ord);
                            long dataLength = reader.GetBytes(ord, 0, null, 0, 0); // get the length 
                            byte[] buffer = new byte[dataLength];
                            reader.GetBytes(ord, 0, buffer, 0, (int)dataLength); // get the data 
                            stream = ByteArrayAsStream(buffer);
                            // string a = System.Convert.ToBase64String(buffer);
                            // System.IO.File.WriteAllText(@"D:\community_sqlite.txt", a, System.Text.Encoding.UTF8);
                        } // End if (reader.Read()) 

                    }// End Using reader 

                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Close();
                } // End Using cmd 

            } // End Using conn 


        } // End Sub Main 


    } // End Class 


} // End Namespace 
