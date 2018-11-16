

namespace NWebCrawlerLib.Common
{
    using System;
    using System.Data;
    using System.IO;

    /// <summary>
    /// 这是一个SQLite的封装
    /// </summary>
    public class SQLiteUtility
    {
        //private static string ds = MemCache.SQLiteDBFolder;
        //private static string createTableSQl =
        //    "CREATE TABLE [repository] (" +
        //    "[vc_url] VARCHAR(255)  UNIQUE NOT NULL," +
        //    "[vc_file_name] VARCHAR(255) NOT NULL," +
        //    "[i_mime] INTEGER DEFAULT 0 NOT NULL," +
        //    "[blob_resource] BLOB  NULL," +
        //    "[dt_inserted_datetime] DATE  NULL," +
        //    "[dt_updated_datetime] DATE  NULL," +
        //    "[vc_agent_name] VARCHAR(64)  NULL," +
        //    "[vc_server_name] VARCHAR(64)  NULL," +
        //    "[i_size] INTEGER DEFAULT 0 NOT NULL)";

        //public static void CreateDB()
        //{
        //    if (File.Exists(MemCache.SQLiteDBFolder))
        //    {
        //        //SQLiteConnection.CreateFile("crawlerdb.s3db");
        //        //ExecuteNonQuery(createTableSQl);
        //    }
        //}

        //public static DataTable GetDataTable(string sql)
        //{
        //    DataTable dt = new DataTable();
        //    SQLiteConnection cnn = null;
        //    try
        //    {
        //        cnn = new SQLiteConnection("Data Source=" + ds);
        //        cnn.Open();
        //        SQLiteCommand mycommand = new SQLiteCommand(cnn);
        //        mycommand.CommandText = sql;
        //        SQLiteDataReader reader = mycommand.ExecuteReader();
        //        dt.Load(reader);
        //    }
        //    catch
        //    {
        //        // Catching exceptions is for communists
        //    }
        //    finally
        //    {
        //        if (cnn != null)
        //        {
        //            cnn.Close();
        //        }
        //    }
        //    return dt;
        //}

        //public static int ExecuteNonQuery(string sql)
        //{
        //    SQLiteConnection cnn = null;
        //    try
        //    {
        //        cnn = new SQLiteConnection("Data Source=" + ds);
        //        cnn.Open();
        //        SQLiteCommand mycommand = new SQLiteCommand(cnn);
        //        mycommand.CommandText = sql;
        //        int rowsUpdated = mycommand.ExecuteNonQuery();
        //        return rowsUpdated;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Error(e.Message + e.StackTrace);
        //    }
        //    finally
        //    {
        //        if (cnn != null)
        //        {
        //            cnn.Close();
        //        }
        //    }
        //    return 0;
        //}

        //public static string ExecuteScalar(string sql)
        //{
        //    SQLiteConnection cnn = null;
        //    try
        //    {
        //        cnn = new SQLiteConnection("Data Source=" + ds);
        //        cnn.Open();
        //        SQLiteCommand mycommand = new SQLiteCommand(cnn);
        //        mycommand.CommandText = sql;
        //        object value = mycommand.ExecuteScalar();
        //        if (value != null)
        //        {
        //            return value.ToString();
        //        }
        //    }
        //    catch
        //    {
        //        // Catching exceptions is for communists
        //    }
        //    finally
        //    {
        //        if (cnn != null)
        //        {
        //            cnn.Close();
        //        }
        //    }
        //    return "";
        //}

        //public static int InsertToRepo(Double pr,string url, int status, string mime, byte[] resource, DateTime start_time, DateTime end_time, int retry_count, string agent_name, string server_name,int links)
        //{
        //    SQLiteConnection cnn = null;
        //    try
        //    {
        //        cnn = new SQLiteConnection("Data Source=" + ds);
        //        cnn.Open();
        //        SQLiteCommand mycommand = new SQLiteCommand(
        //            "INSERT INTO Repository (" +
        //            "pr_rank,vc_url,i_status,i_mime,blob_resource,i_size,dt_start_time,dt_end_time,i_retry_count,vc_agent_name,vc_server_name,outer_links) " +
        //            "VALUES(@pr_rank,@url,@status,@mime,@resource,@size,@start_time,@end_time,@retry_count,@agent_name, @server_name,@outer_links)", cnn);

        //        mycommand.Parameters.Add("@pr_rank", DbType.Double).Value = pr;
        //        mycommand.Parameters.Add("@url", DbType.String, 255).Value = url;
        //        mycommand.Parameters.Add("@status", DbType.Int32).Value = status;
        //        mycommand.Parameters.Add("@mime", DbType.Int32).Value = 1;
        //        mycommand.Parameters.Add("@resource", DbType.Binary, 20).Value = resource;
        //        mycommand.Parameters.Add("@size", DbType.Int32).Value = resource.Length;
        //        mycommand.Parameters.Add("@start_time", DbType.DateTime).Value = start_time;
        //        mycommand.Parameters.Add("@end_time", DbType.DateTime).Value = end_time;
        //        mycommand.Parameters.Add("@retry_count", DbType.Int32).Value = retry_count;
        //        mycommand.Parameters.Add("@agent_name", DbType.String, 64).Value = agent_name;
        //        mycommand.Parameters.Add("@server_name", DbType.String, 64).Value = server_name;
        //        mycommand.Parameters.Add("@outer_links", DbType.Int32).Value = links;

        //        int rowsUpdated = mycommand.ExecuteNonQuery();
        //        return rowsUpdated;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Error(e.Message + e.StackTrace);
        //    }
        //    finally
        //    {
        //        if (cnn != null)
        //        {
        //            cnn.Close();
        //        }
        //    }

        //    return 0;
        //}

        //public static int Cleanup(string tableName)
        //{
        //    int rowsUpdated = ExecuteNonQuery("DELETE FROM " + tableName);
        //    return rowsUpdated;
        //}

        //public static void ExportSQLiteToFile(string sqlQuery)
        //{
        //    SQLiteConnection cnn=null;
        //    try
        //    {
        //        cnn = new SQLiteConnection("Data Source=" + ds);
        //        cnn.Open();
        //        SQLiteCommand mycommand = new SQLiteCommand(cnn);
        //        mycommand.CommandText = sqlQuery;
        //        SQLiteDataReader reader = mycommand.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            //resouce = new Byte[(reader.GetBytes(1, 0, resouce, 0, int.MaxValue))];
        //            FileSystemUtility.StoreWebFile(reader[0].ToString(), (byte[])reader.GetValue(1));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Error(e.Message + e.StackTrace);
        //    }
        //    finally
        //    {
        //        if (cnn != null)
        //        {
        //            cnn.Close();
        //        }
        //    }
        //}
    }
}
