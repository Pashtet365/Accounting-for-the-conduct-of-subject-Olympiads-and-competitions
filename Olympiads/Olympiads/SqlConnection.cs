using System;
using System.Data;
using System.Data.SqlClient;

public class DatabaseConnection : IDisposable
{
    private string connectionString;
    private SqlConnection connection;

    public DatabaseConnection()
    {
        //computer
        connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"G:\\Files\\ЗаконченыеПроекты\\3КУРС\\4course\\БД\\КурсоваяБД\\Program\\Olympiads\\Olympiads\\OlympiadsDB.mdf\";Integrated Security=True";
        //laptor
        //connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\GitHub\\OpenAccess\\GGAEK\\4course\\БД\\КурсоваяБД\\Program\\Olympiads\\Olympiads\\OlympiadsDB.mdf\";Integrated Security=True";
        connection = new SqlConnection(connectionString);
    }

    public bool OpenConnection()
    {
        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return true;
        }
        catch (SqlException)
        {
            // Обработка ошибки подключения
            return false;
        }
    }

    public bool CloseConnection()
    {
        try
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return true;
        }
        catch (SqlException)
        {
            // Обработка ошибки закрытия подключения
            return false;
        }
    }

    public SqlConnection GetConnection()
    {
        return connection;
    }

    public void Dispose()
    {
        // Закрыть соединение при уничтожении объекта
        if (connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }
}





/*

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DatabaseConnection : IDisposable
{
    private string connectionString;
    private SqlConnection connection;

    public DatabaseConnection()
    {
        // Чтение строки подключения из конфигурационного файла
        connectionString = ConfigurationManager.ConnectionStrings["OlympiadsDBConnectionString"].ConnectionString;
        connection = new SqlConnection(connectionString);
    }

    public bool OpenConnection()
    {
        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return true;
        }
        catch (SqlException)
        {
            // Обработка ошибки подключения
            return false;
        }
    }

    public bool CloseConnection()
    {
        try
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return true;
        }
        catch (SqlException)
        {
            // Обработка ошибки закрытия подключения
            return false;
        }
    }

    public SqlConnection GetConnection()
    {
        return connection;
    }

    public void Dispose()
    {
        // Закрыть соединение при уничтожении объекта
        if (connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }
}
*/