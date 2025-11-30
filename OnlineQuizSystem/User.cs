//created by Jack Duncan 
//last modified 25/11/2025 by Jack Duncan
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizSystem
{
    internal class User
    {
        //varibles
        private int id;
        private string userName;
        private string password;
        private string email;
        private string role;

        //getters and setters
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Role
        {
            get { return role; }
            set { role = value; }
        }
        //constructors
        public User() { }
        public User(int id,string userName, string password, string email, string role)
        {
            this.id = id;
            this.userName = userName;
            this.password = password;
            this.email = email;
            this.role = role;
        }

        //methods
        public void updateProfile()
        {

        }
    }
}
