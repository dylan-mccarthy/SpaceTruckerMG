namespace SpaceTrucker.Server.Models
{
    //Class for holding the Player account information
    public class Player
    {
        public int id;
        public string username;

        public Player(int id, string username)
        {
            this.id = id;
            this.username = username;
        }

    }
    
}