//created by Jack Duncan 
//last modified 26/11/2025 by Jack Duncan
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuizSystem
{
    internal class Student : User
    {
        //varibles
        private string status;
        //getter and setters
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        //constructors
        public Student(string status, int id, string userName, string password, string email, string role) : base(id,userName,password,email,role)
        {
            this.status = status;
        }
    }
}
