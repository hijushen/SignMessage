
namespace NexChip.SignMessage
{
    public class BaseDBConfig
    {
        //public static string ConnectionString = "server=.;uid=sa;pwd=Admin;database=RayPI";
        public static string ConnectionString = @"
Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=SignMessage;Integrated Security=SSPI";
        //Data Source = (localdb)\MSSQLLocalDB; Integrated Security = True;uid=sa;pwd=123456";    ;AttachDBFilename=|DataDirectory|\Movies.mdf
    }
}
