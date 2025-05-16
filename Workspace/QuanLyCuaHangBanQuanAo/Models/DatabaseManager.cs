using System;

namespace Project_QuanAo.Helpers
{
    public class DatabaseManager
    {
        private static DatabaseManager instance;
        private static readonly object lockObj = new object();

        private DBConnection dbConnect;

        private DatabaseManager()
        {
            InitializeDatabaseConnection();
        }

        public static DatabaseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new DatabaseManager();
                        }
                    }
                }
                return instance;
            }
        }

        private void InitializeDatabaseConnection()
        {
            var configHelper = new XmlConfigurationHelper();
            string serverName = configHelper.GetServerName();
            string dbName = configHelper.GetDBName();

            dbConnect = new DBConnection(serverName, dbName);
        }

        public DBConnection GetDBConnection()
        {
            return dbConnect;
        }
    }
}