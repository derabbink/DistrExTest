namespace PingPong.Models
{
    public class Ponger
    {
        public Ponger(bool canPong)
        {
            CanPong = canPong;
        }

        public bool CanPong { get; set; }
    }
}
