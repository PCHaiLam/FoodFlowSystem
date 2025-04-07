namespace FoodFlowSystem.Helpers
{
    public class HashPassword
    {
        public static string Hash(string password)
        {
            //var data = System.Text.Encoding.ASCII.GetBytes(password);
            //data = System.Security.Cryptography.SHA256.HashData(data);
            //return System.Text.Encoding.ASCII.GetString(data);
            return password;
        }
    }
}
