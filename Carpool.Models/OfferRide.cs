using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpool
{
    public class OfferRide
    {
        public Dictionary<string,long> Route = new Dictionary<string, long>();
        public int AvailableSeats { get; set; }
        public decimal RideFairPerKm { get; set; }
        public decimal TotalRideFair { get; set; }
        public Car Car { get; set; } 
        public User Host { get; set; } 
        public List<Booking> Bookings = new List<Booking>();
    }
}
