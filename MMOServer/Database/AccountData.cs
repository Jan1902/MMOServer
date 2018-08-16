namespace MMOServer.Database
{
    class AccountData
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
