using Microsoft.Data.SqlClient;
using System.Collections.Concurrent;

namespace CrawlerProject.Models.Service
{
    public class ConnectionSettingService
    {
        public static SqlConnection connection
        {
            get
            {
                SqlConnection sqlcon = new SqlConnection
                    ("Server=DESKTOP-5EPP4QL;Database=CrawlerDB;TrustServerCertificate=True;trusted_connection=True;");
                return sqlcon;
            }
        }
    }
}
