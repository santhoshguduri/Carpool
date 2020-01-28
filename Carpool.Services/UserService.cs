using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpool
{
    public class UserServices
    {
        public static List<User> Users = new List<User>();
        //storing user details
        public void CreateNewUser(string name, string mobile, char gender, string username, string password)
        {
            User User = new User
            {
                Name = name,
                Mobileno = mobile,
                Gender = gender,
                Username = username,
                Password = password,
                Id = Guid.NewGuid().ToString()
            };
            Users.Add(User);
        }
        public bool CheckIsUnique(string username)
        {
            bool isUnique = true;
            bool isExists = Users.Exists(us => us.Username == username);
            if(!isExists)
            {
                isUnique = false;
            }
            return isUnique;
        }
        public bool Authenticate(string name, string password, out User selectedUser)
        {
            bool access = false;
            selectedUser = null;
            bool isExists = Users.Exists(us => us.Username == name && us.Password == password);
            if(isExists)
            {
                selectedUser = Users.Single(us => us.Username == name && us.Password == password);
                access = true;
            }
            return access;
        }
        public void DisplayProfileDetails( User selectedUser)
        {
            Helper.Print("\tName : " + selectedUser.Name);
            Helper.Print("\tMobile : " + selectedUser.Mobileno);
            Helper.Print("\tGender : " + selectedUser.Gender);
            Helper.Print("\tId Number : " + selectedUser.Id);
        }
    }
}
