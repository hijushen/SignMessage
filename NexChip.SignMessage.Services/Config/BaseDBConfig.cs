

using Microsoft.Extensions.Configuration;
using NexChip.SignMessage.Utils;

namespace NexChip.SignMessage.Services
{
    public class BaseDBConfig
    {
        //public static string ConnectionString = "server=.;uid=sa;pwd=Admin;database=RayPI";
        public static string ConnectionString = AppConfigManager.Configuration.GetConnectionString("DB");


        //    @" 
        //Data Source = (localdb)\.\IIS_DB;database=SignMessage; Integrated Security = True;uid=sa;pwd=123456";
        // database=SignMessage; Integrated Security = True;uid=sa;pwd=123456
    }
}
