using Npgsql;

namespace ElectroStoreApp.Data
{
    public static class Database
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=electro_shop;Username=postgres;Password=1";

        public static NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }
    }
}