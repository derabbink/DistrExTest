using System.ComponentModel;
using PingPong.Annotations;

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
