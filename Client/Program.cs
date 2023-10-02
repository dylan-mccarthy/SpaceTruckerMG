namespace SpaceTrucker.Client;
public class Program
{
    private static void Main(string[] args)
    {
        using var game = new GameClient();
        game.Run();
    }
}
