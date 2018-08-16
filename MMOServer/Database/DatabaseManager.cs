using MMOServer.ConsoleStuff;
using MMOServer.Networking;
using MMOServer.Other;
using MySql.Data.MySqlClient;
using System;

namespace MMOServer.Database
{
    class DatabaseManager
    {
        private GameServer _gameServer;
        private MySqlConnection _conection;

        public DatabaseManager(GameServer gameServer)
        {
            _gameServer = gameServer;
            Connect();
        }

        private void Connect()
        {
            var connectionString = String.Format("Server={0}; database={1}; UID={2}; password={3};  SSlMode=none", _gameServer.ConfigManager.Settings.DatabaseAddress, _gameServer.ConfigManager.Settings.DatabaseName, _gameServer.ConfigManager.Settings.DatabaseUsername, _gameServer.ConfigManager.Settings.DatabasePassword);
            _conection = new MySqlConnection(connectionString);

            try
            {
                _conection.Open();
                ConsoleUtils.Info("Connected to database successfully");
            }
            catch(Exception e)
            {
                ConsoleUtils.Error("An error occured while trying to connect the the database");
                Logger.LogError(e.GetBaseException().ToString());
            }
        }

        public void Disconnect()
        {
            _conection.Close();
            ConsoleUtils.Info("Disconnected from database");
        }

        public AccountData GetAccountFromUsername(string username)
        {
            var query = string.Format("select * from accounts where accountName = '{0}'", username);
            using (var command = new MySqlCommand(query, _conection))
            {
                var reader = command.ExecuteReader();
                if (!reader.Read())
                    return null;
                else
                {
                    return new AccountData()
                    {
                        AccountID = reader.GetInt32("accountId"),
                        AccountName = reader.GetString("accountName"),
                        PasswordHash = Utils.HexStringToByteArray(reader.GetString("passwordHash")),
                        PasswordSalt = Utils.HexStringToByteArray(reader.GetString("passwordSalt"))
                    };
                }
            }
        }
    }
}
