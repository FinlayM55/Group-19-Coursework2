//created by Jack Duncan 
//last modified 25/11/2025 by Jack Duncan
//edited by Mark Stewart 08/12/2025
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
        protected bool isLoggedIn;

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
        public User() 
        { 
        
        }
        public User(int id,string userName, string password, string email, string role)
        {
            this.id = id;
            this.userName = userName;
            this.password = password;
            this.email = email;
            this.role = role;
            this.isLoggedIn = false;
        }

        //methods
        public void updateProfile()
        {

        }

        public virtual void Login()
        {
            isLoggedIn = true;
        }

        public virtual void Logout()
        {
            isLoggedIn = false;
        }

        public void UpdateProfile()
        {
            Console.Clear();
            Console.WriteLine("=== Update Profile ===");
            Console.WriteLine("(Press Enter to keep current value)\n");

            Console.WriteLine($"Current username: {userName}");
            Console.Write("New username: ");
            string? newUserName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newUserName))
                userName = newUserName.Trim();

            Console.WriteLine($"\nCurrent email: {email}");
            Console.Write("New email: ");
            string? newEmail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newEmail))
                email = newEmail.Trim();

            Console.Write("\nNew password: ");
            string? newPassword = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPassword))
                password = newPassword;

            Console.WriteLine("\nProfile updated. Press any key...");
            Console.ReadKey();
        }


    }
}
