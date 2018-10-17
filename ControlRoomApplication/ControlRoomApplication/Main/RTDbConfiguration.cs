using MySql.Data.EntityFramework;
using System.Data.Entity;

namespace ControlRoomApplication.Main
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class RTDbConfiguration : DbConfiguration
    {
        public RTDbConfiguration()
        {
            SetProviderFactory("MySql.Data.MySqlClient", new MySql.Data.MySqlClient.MySqlClientFactory());
            SetProviderServices("MySql.Data.MySqlClient", new MySql.Data.MySqlClient.MySqlProviderServices());
            SetDefaultConnectionFactory(new MySqlConnectionFactory("RTDatabase"));
        }
    }
}