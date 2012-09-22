namespace Walleet
{
    public class Credentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public Credentials(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}