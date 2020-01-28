using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpool.Services
{
    public class BookingService : OfferRideService
    {
        public List<Booking> BookedRides = new List<Booking>();
        public void Book(string startpoint, string destination, int seats, User selectedUser, out Booking selectedRideBooking)
        {
            Booking Book = new Booking
            {
                StartPoint = startpoint,
                Destination = destination,
                Seats = seats,
                BookingUser = selectedUser,
                Id = Guid.NewGuid().ToString()
            };
            selectedRideBooking = Book;
        }
        public void SaveBooking(OfferRide selectedRide, Booking selectedBooking, User selectedUser)
        {
            selectedBooking.Host = selectedRide.Host;
            selectedBooking.Car = selectedRide.Car;
            selectedBooking.RideFair = selectedRide.TotalRideFair;
            selectedBooking.Status = BookingStatus.Requested;
            selectedBooking.BookingUser.LastBooking = selectedBooking;
            BookedRides.Add(selectedBooking);
            selectedRide.Bookings.Add(selectedBooking);
            selectedUser.LastBooking = selectedBooking;
        }
        public bool SearchAvailableRides(string startPoint, string destination, int seats, User selectedUser, Booking selectedRideBooking)
        {
            AvailableRides = new List<OfferRide>();
            bool Available = false;
            bool pickupEncountered = false;
            var Rides = RideOffers.Where(ride => ride.Host != selectedUser && ride.Route.ContainsKey(startPoint) && ride.Route.ContainsKey(destination));
            foreach (var ride in Rides)
            {
                if (ride.AvailableSeats >= seats)
                {
                    long distance = 0;
                    foreach (KeyValuePair<string, long> area in ride.Route)
                    {
                        if (area.Key == startPoint)
                        {
                            pickupEncountered = true;
                        }
                        else if (area.Key == destination && pickupEncountered == true)
                        {
                            AvailableRides.Add(ride);
                            pickupEncountered = false;
                            Available = true;
                        }
                        if (pickupEncountered)
                        {
                            distance += area.Value;
                        }
                    }
                    ride.TotalRideFair = (distance * ride.RideFairPerKm);
                }
            }
            return Available;
        }
        public void SetStatusOfBooking(Booking selectedBooking, BookingStatus status, User selectedUser)
        {
            if (status == BookingStatus.Approved)
            {
                selectedBooking.Status = BookingStatus.Approved;
            }
            else if (status == BookingStatus.Rejected)
            {
                selectedBooking.Status = BookingStatus.Rejected;
                selectedUser.LastRideOffered.AvailableSeats += selectedBooking.Seats;
            }
        }
    }
}
