
namespace NexChip.SignMessage
{
    public class BaseDBConfig
    {
        //public static string ConnectionString = "server=.;uid=sa;pwd=Admin;database=RayPI";
        public static string ConnectionString = @"
        Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = SignMessage; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        ";    
}
}
