namespace PingPong.Models
{
    public class Pinger
    {
        public Pinger(bool canPing)
        {
            CanPing = canPing;
        }

        public bool CanPing { get; set; }
    }
}
