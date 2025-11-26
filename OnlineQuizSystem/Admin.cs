//created by Jack Duncan 
//last modified 26/11/2025 by Jack Duncan
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizSystem
{
    internal class Admin : User
    {
        //varibles
        private DateTime loginDate;
        //getter and setters
        public DateTime LoginDate
        {
            get { return loginDate; }
            set { loginDate = value; }
        }
        //constructors
        public Admin(DateTime loginDate, int id, string userName, string password, string email, string role) : base(id, userName, password, email, role)
        {
            this.loginDate = loginDate;
        }
    }
}
