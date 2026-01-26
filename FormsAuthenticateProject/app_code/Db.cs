using System.Configuration;
using System.Data.SqlClient;

public static class Db
{
    public static SqlConnection Conn()
    {
        string cs = ConfigurationManager.ConnectionStrings["HeliSoundDB"].ConnectionString;
        return new SqlConnection(cs);
    }
}
