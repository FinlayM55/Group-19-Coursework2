//created by Jack Duncan 
//last modified 26/11/2025 by Jack Duncan
//edited by Mark Stewart 08/12/2025
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

        public Admin() : base()
        {

        }
        public Admin(DateTime loginDate, int id, string userName, string password, string email, string role) : base(id, userName, password, email, role)
        {
            this.loginDate = loginDate;
        }

        //methods

        public void AddQuestion(Quiz quiz, Question question)
        {
            quiz.AddQuestion(question);
        }

        public void UpdateQuestion(Question question, string newText)
        {
            question.QuestionText = newText;
        }

        public void RemoveQuestion(Quiz quiz, int questionId)
        {
            quiz.RemoveQuestion(questionId);
        }

        public void ShowAllQuestions(QuizSystem system)
        {
            system.ShowAllQuestions();
        }

        public void AddUser(QuizSystem system, User user)
        {
            if (user is Admin a) system.AdminUsers.Add(a);
            if (user is Student s) system.StudentUsers.Add(s);
        }


    }
}
