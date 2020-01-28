using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpool.Services;

namespace Carpool
{
    public class Homepage
    {
        public User SelectedUser;
        public Booking SelectedBooking;
        UserServices UserService = new UserServices();
        BookingService BookingService = new BookingService();
        public OfferRide SelectedRide;

        public void SignUp()
        {
            Helper.Print("Please enter your details below");
            Helper.Print("\nEnter Name:");
            string name = Console.ReadLine();
            Helper.Print("\nEnter Mobile Number:");
            string mobile = Console.ReadLine();
            Helper.ValidateMobileNumber(mobile, out mobile);
            Helper.Print("\nEnter Gender(M/F):");
            char gender = Convert.ToChar(Console.ReadLine());
            Helper.Print("\nEnter Username : ");
            string username = Console.ReadLine();
            bool isUnique = UserService.CheckIsUnique(username);
            while (isUnique)
            {
                Helper.Print("\tUsername Already Exists");
                Helper.Print("\nEnter Username : ");
                username = Console.ReadLine();
                isUnique = UserService.CheckIsUnique(username);
            }
            Helper.Print("\nEnter Password:");
            string password = Console.ReadLine();
            Helper.Print("\nConfirm Password :");
            string confirmationPassword = Console.ReadLine();
            while (password != confirmationPassword)
            {
                Helper.Print("Password did not match");
                Helper.Print("\nConfirm Password :");
                confirmationPassword = Console.ReadLine();
            }
            UserService.CreateNewUser(name, mobile, gender, username, password);
            Helper.Print("\n\t----------REGISTRATION SUCCESSFUL----------\n");
            Helper.Print("Please login for confirmation");
            LogIn();
        }

        public void LogIn()
        {
            Helper.Print("\n\t----------AUTHENTICATION REQUIRED----------\n");
            Helper.Print("Enter Your Username:");
            string name = Console.ReadLine();
            Helper.Print("\nEnter Password:");
            string password = Console.ReadLine();
            bool access = UserService.Authenticate(name, password, out SelectedUser);
            if(access)
            {
                MyOptions();
            }
            else
            {
                Helper.Print("/n\t----------Authentication Failed----------\n");
                Helper.Print($"Redirecting to main page");
            }
        }

        public void MyOptions()
        {
            Helper.Print("My Options:");
            Helper.Print("\t1)Profile\n\t2)Offer a Ride\n\t3)Book a Ride\n\t4)Bookings Approval\n\t5)Check status of recent booking\n\t6)Past Offers\n\t7)Past Bookings\n\t8)Logout");
            int Option = Convert.ToInt16(Console.ReadLine());
            if (Option >= 1 && Option <= 8)
            {
                switch (Option)
                {
                    case 1:
                        UserService.DisplayProfileDetails(SelectedUser);
                        MyOptions();
                        break;
                    case 2:
                        Helper.Print("\nPlease enter the Details of your Car");
                        Helper.Print("\nEnter car Name:");
                        string name = Console.ReadLine();
                        Helper.Print("\nSelect your car type:");
                        int index = 0;
                        foreach (string str in Enum.GetNames(typeof(CarTypes)))
                        {
                            Helper.Print($"{++index}) { str}");
                        }
                        int selectedType = Convert.ToInt16(Console.ReadLine());
                        CarTypes type = (CarTypes)selectedType;
                        Helper.Print("\nEnter seat Capacity:");
                        int seatCapacity = Convert.ToInt16(Console.ReadLine());
                        BookingService.CarDetails(name, type, seatCapacity);
                        Helper.Print("\nEnter Ride fair(per Km):");
                        decimal rideFairPerKm = Convert.ToDecimal(Console.ReadLine());
                        Helper.Print("Please enter the number of checkpoints");
                        int checkPoints = Convert.ToInt16(Console.ReadLine());
                        Helper.Print("\nPlease enter your start point, Checkpoints,Destination and their respective distances(in Kms) to next area");
                        BookingService.CreateRideOffer(rideFairPerKm, checkPoints, SelectedUser);
                        Helper.Print("\n\tYour Ride offer has been registered");
                        Helper.Print("\n\tcurrently no one has booked your Ride\n\tWe will notify you once booking has been done");
                        Helper.Print("\tRedirecting to Main page\n");
                        MyOptions();
                        break;
                    case 3:
                        Helper.Print("\nEnter Pickup Point:");
                        string pickPoint = Console.ReadLine();
                        Helper.Print("\nEnter Drop Point:");
                        string dropPoint = Console.ReadLine();
                        Helper.Print("\nEnter number of seats to book:");
                        int seats = Convert.ToInt16(Console.ReadLine());
                        BookingService.Book(pickPoint, dropPoint, seats, SelectedUser, out SelectedBooking);
                        bool available = BookingService.SearchAvailableRides(pickPoint, dropPoint, seats, SelectedUser, SelectedBooking);
                        if (available)
                        {
                            DisplayAvailableRides();
                            Helper.Print("\n\tSelect a option to book ride");
                            int rideOption = Convert.ToInt16(Console.ReadLine());
                            SelectedRide = BookingService.ReturnSelectedRideObject(rideOption);
                            DisplayConfirmationDetails();
                            BookingService.SaveBooking(SelectedRide, SelectedBooking, SelectedUser);
                            MyOptions();
                        }
                        else
                        {
                            Helper.Print("\n\tCurrently there are no ride offers available");
                            Helper.Print("\n\tplease check-in After sometime");
                            MyOptions();
                        }
                        break;
                    case 4:
                        if (SelectedUser.LastRideOffered == null)
                        {
                            Helper.Print("No ride offered previously");
                        }
                        else
                        {
                            if (SelectedUser.LastRideOffered.Bookings.Count == 0)
                            {
                                Helper.Print($"No one has booked your offer yet");
                            }
                            else
                            {
                                Helper.Print("Showing the bookings of ride offered");
                                int bookingIndex = 0;
                                foreach (Booking rideBooking in SelectedUser.LastRideOffered.Bookings)
                                {
                                    if (rideBooking.Status != BookingStatus.Rejected)
                                    {
                                        Helper.Print($" {++bookingIndex} )\n\t Name : {rideBooking.BookingUser.Name}");
                                        Helper.Print($"\t Mobile : {rideBooking.BookingUser.Mobileno}");
                                        Helper.Print($"\t Pickup Point : {rideBooking.StartPoint}");
                                        Helper.Print($"\t Drop Point : {rideBooking.Destination}");
                                        Helper.Print($"\t Ride Fair : {rideBooking.RideFair}");
                                        Helper.Print("----------------------------------------------------------");
                                    }
                                }
                                Helper.Print("Select the booking you want to approve or reject");
                                int bookingOption = Convert.ToInt16(Console.ReadLine());
                                Helper.Print("Do you want to approve or reject the booking:\n\t1)Approve\n\t2)Reject");
                                BookingStatus setStatus = (BookingStatus)Convert.ToInt16(Console.ReadLine());
                                Booking selectedBooking = BookingService.ReturnSelectedBooking(bookingOption, SelectedUser);
                                BookingService.SetStatusOfBooking(selectedBooking, setStatus, SelectedUser);
                            }
                        }
                        MyOptions();
                        break;
                    case 5:
                        if (SelectedUser.LastBooking == null)
                        {
                            Helper.Print("No pending bookings available");
                        }
                        else
                        {
                            Helper.Print($"Booked ride was {SelectedUser.LastBooking.Status}");
                        }
                        MyOptions();
                        break;
                    case 6:
                        DisplayPastOffers(SelectedUser);
                        MyOptions();
                        break;
                    case 7:
                        DisplayPastBookings(SelectedUser);
                        MyOptions();
                        break;
                    case 8:
                        Helper.Print("\t***YOU HAVE BEEN LOGOUT***");
                        break;
                }
            }
            else
            {
                Helper.Print($"Please enter a valid input");
                MyOptions();
            }
        }
        public void DisplayAvailableRides()
        {
            int index = 0;
            foreach (OfferRide i in BookingService.AvailableRides)
            {
                Helper.Print($" {++index} )\n\tRiders Name : {i.Host.Name}");
                Helper.Print($"\tMobile Number : {i.Host.Mobileno}");
                Helper.Print($"\tGender : { i.Host.Gender}");
                Helper.Print($"\tCar Details : \n\tCar Name : {i.Car.Name}");
                Helper.Print($"\tCar Type : {i.Car.TypeName}");
                Helper.Print($"\tNumber of seats available for booking : {i.AvailableSeats}");
                Helper.Print($"\tRide Fair per person : {i.TotalRideFair}");
                Helper.Print("-----------------------------------------------------------------");
            }
        }
        public void DisplayConfirmationDetails()
        {
            Helper.Print($"\n\tRiders Name : {SelectedRide.Host.Name}");
            Helper.Print($"\tMobile Number : {SelectedRide.Host.Mobileno}");
            Helper.Print($"\tGender : {SelectedRide.Host.Gender}");
            Helper.Print($"\tCar Details : \n\tCar Name : {SelectedRide.Car.Name}");
            Helper.Print($"\tCar Type : {SelectedRide.Car.TypeName}");
            Helper.Print($"\tNumber of seats available for booking : {SelectedRide.Car.SeatCapacity}");
            Helper.Print($"\tRide Fair per person : {SelectedRide.TotalRideFair}");
            Helper.Print("Selected ride has been booked");
        }

        public void DisplayPastBookings(User selectedUser)
        {
            int Index = 1;
            foreach (Booking Booking in BookingService.BookedRides)
            {
                if (Booking.BookingUser.Id == selectedUser.Id)
                {
                    Helper.Print($"\n{Index++} )\tFrom : {Booking.StartPoint}\tTo : {Booking.Destination}");
                    Helper.Print("\n\tRide Offered by :");
                    Helper.Print($"\tName : {Booking.Host.Name}");
                    Helper.Print($"\tMobile Number : {Booking.Host.Mobileno}");
                    Helper.Print($"\tRide Fair : {Booking.RideFair}");
                    Helper.Print($"\tBooking Id : {Booking.Id}");
                    Helper.Print($"\tRide booked was {Booking.Status} by Offer creator ");
                    Helper.Print("---------------------------------------------------------------------------------");
                }
            }
            if (Index == 1)
            {
                Helper.Print("\tNo bookings has been done Previously");
            }
        }
        public void DisplayPastOffers(User selectedUser)
        {
            int Index = 1;
            foreach (OfferRide Ride in BookingService.RideOffers)
            {
                if (selectedUser.Id == Ride.Host.Id)
                {
                    Helper.Print($"\n {Index++} )\t From : {Ride.Route.ElementAt(0).Key}\t To : {Ride.Route.ElementAt(Ride.Route.Count - 1).Key}");
                    foreach (Booking rideBooking in Ride.Bookings)
                    {
                        Helper.Print($"\t Name : {rideBooking.BookingUser.Name}");
                        Helper.Print($"\t Mobile : {rideBooking.BookingUser.Mobileno}");
                        Helper.Print($"\t Pickup Point : {rideBooking.StartPoint}");
                        Helper.Print($"\t Drop Point : {rideBooking.Destination}");
                        Helper.Print($"\t Ride Fair : {rideBooking.RideFair}");
                        Helper.Print($"\t Booking : {rideBooking.Status}");
                        Helper.Print("----------------------------------------------------------");
                    }
                }
            }
            if (Index == 1)
            {
                Helper.Print("\tNo Ride Offers given Previously");
            }
        }
    }
}
