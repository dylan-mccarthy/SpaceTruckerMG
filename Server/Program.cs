namespace SpaceTrucker.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var server = new Server();
            server.Start();
        }
    }
}