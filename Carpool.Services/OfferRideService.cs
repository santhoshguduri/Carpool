using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpool.Services
{
    public class OfferRideService
    {
        public List<Car> Cars = new List<Car>();
        public List<OfferRide> RideOffers = new List<OfferRide>();
        public List<OfferRide> AvailableRides;
        public OfferRide OfferRide;
        public void CarDetails(string carName, CarTypes typeName, int seatCapcity)
        {
            OfferRide = new OfferRide();
            Car car = new Car
            {
                Name = carName,
                TypeName = typeName,
                SeatCapacity = seatCapcity,
            };
            Cars.Add(car);
            OfferRide.Car = car;
            OfferRide.AvailableSeats = seatCapcity;
        }

        public void CreateRideOffer(decimal rideFair, int checkPoints, User selectedUser)
        {
            OfferRide.RideFairPerKm = rideFair;
            int totalAreas = checkPoints + 2, i = 0;
            while (i != totalAreas)
            {
                Helper.Print($"Please enter the area");
                string area = Console.ReadLine();
                Helper.Print($"Please enter the distance to the next checkpoint");
                long distanceToNextArea = Convert.ToInt64(Console.ReadLine());
                OfferRide.Route.Add(area,distanceToNextArea);
                i++;
            }
            OfferRide.Host = selectedUser;
            RideOffers.Add(OfferRide);
            selectedUser.LastRideOffered = OfferRide;
        }

        public OfferRide ReturnSelectedRideObject(int index)
        {
            OfferRide ride = null;
            ride = AvailableRides[index - 1];
            return ride;
        }
        
        public Booking ReturnSelectedBooking(int index,User selectedUser)
        {
            Booking book = null;
            book = selectedUser.LastRideOffered.Bookings[index - 1];
            return book;
        }
        
    }
}
