using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpool
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobileno { get; set; }
        public char Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }
        public OfferRide LastRideOffered { get; set; }
        public Booking LastBooking { get; set; }
    }
}
