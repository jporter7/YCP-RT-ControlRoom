using System.Data.Entity;

namespace ControlRoomApplication
{
    public class RTDbConfiguration : DbConfiguration
    {
        public RTDbConfiguration()
        {
            SetProviderFactory("MySql.Data.MySqlClient", new MySql.Data.MySqlClient.MySqlClientFactory());
            SetDefaultConnectionFactory(new MySql.Data.Entity.MySqlConnectionFactory());
        }
    }
}
