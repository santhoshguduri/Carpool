using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpool
{
    public enum BookingStatus { None ,Approved = 1 , Rejected, Requested}
    public class Booking
    {
        public string StartPoint { get; set; }
        public int Seats { get; set; }
        public string Destination { get; set; }
        public decimal RideFair { get; set; }
        public string Id { get; set; }
        public User BookingUser { get; set; }
        public User Host { get; set; }
        public Car Car { get; set; }
        public BookingStatus Status { get; set; } = 0;
    }
}
