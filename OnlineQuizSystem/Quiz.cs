using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Created by Finlay Morgan 01/12/2025
//reviewed by Mark Stewart 08/12/2025  

namespace OnlineQuizSystem
{
    internal class Quiz
    {

        private int quizID;
        private string quizTitle;
        private string quizDescription;
        private Category quizCategory;
        private List<Question> quizQuestions;
        private DateTime quizDate;

        //getters and setters   
        public int QuizID
        {
            get { return quizID; } //no public setter
            
        }
        public string QuizTitle
        {
            get { return quizTitle; }
            set { quizTitle = value; }
        }
        public string QuizDescription
        {
            get { return quizDescription; }
            set { quizDescription = value; }
        }
        public Category QuizCategory
        {
            get { return quizCategory; }
            set { quizCategory = value; }
        }
        public List<Question> QuizQuestions
        {
            get { return quizQuestions; }
            set { quizQuestions = value; }
        }
        public DateTime QuizDate
        {
            get { return quizDate; }
            set { quizDate = value; }
        }

        //Default constructor

        public Quiz()
        {
            quizQuestions = new List<Question>();
        }


        //Custom constructor

        public Quiz(int quizID, string quizTitle, string quizDescription, Category quizCategory, DateTime quizDate)
        {
            this.quizID = quizID;
            this.quizTitle = quizTitle;
            this.quizDescription = quizDescription;
            this.quizCategory = quizCategory;
            this.quizDate = quizDate;
            this.quizQuestions = new List<Question>();
        }

        //methods
        public void AddQuestion(Question question)
        {
            quizQuestions.Add(question);
        }

        public void RemoveQuestion(int questionId)
        {
            quizQuestions.RemoveAll(q => q.QuestionID == questionId);
        }

        /*public List<Question> GetQuestions() //returns the list of questions in Question.cs
        {
            return new List<Question>(quizQuestions);
        }
        // Displays the supplied list or, if null, the default OOP questions
        public void DisplayQuestions(List<Question> questions = null)
        {
            var list = questions ?? GetQuestions();

            if (list == null || list.Count == 0)
            {
                Console.WriteLine("No questions to display.");
                return;
            }

            foreach (var q in list)
            {
                Console.WriteLine($"Q{q.QuestionID}: {q.QuestionText} ({q.QuestionDifficultyLevel})");

                var options = q.QuestionOptions ?? new List<string>();
                for (int i = 0; i < options.Count; i++)
                {
                    char letter = (char)('A' + i);
                    Console.WriteLine($"  {letter}. {options[i]}");
                }

                Console.WriteLine(); // blank line between questions
            }
        } */
        //removed for redundancy have to check

    }
}

