using MySql.Data.EntityFramework;
using System.Data.Entity;

namespace ControlRoomApplication.RemoteDb
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class RTDbConfiguration : DbConfiguration
    {
        public RTDbConfiguration()
        {
            SetProviderFactory(AWSConstants.DATABASE_PROVIDER, new MySql.Data.MySqlClient.MySqlClientFactory());
            SetProviderServices(AWSConstants.DATABASE_PROVIDER, new MySql.Data.MySqlClient.MySqlProviderServices());
            SetDefaultConnectionFactory(new MySqlConnectionFactory(AWSConstants.REMOTE_DATABASE_NAME));
        }
    }
}