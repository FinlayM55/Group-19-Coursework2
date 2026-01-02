//created by Jack Duncan 
//last modified 26/11/2025 by Jack Duncan
//edited by Mark Stewart 08/12/2025
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

        public Student() : base()
        {

        }

        public Student(string status, int id, string userName, string password, string email, string role) : base(id, userName, password, email, role)
        {
            this.status = status;
        }

        private List<string> quizResults = new List<string>();


        //methods

        public void PlayQuiz(Quiz quiz)
        {
            Console.Clear();
            Console.WriteLine($"=== Quiz: {quiz.QuizTitle} ===");

            int score = 0;

            foreach (var question in quiz.QuizQuestions)
            {
                Console.WriteLine($"\n{question.QuestionText}");
                var options = question.GetOptions();
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {options[i]}");
                }

                Console.Write("Your answer (number): ");
                string? input = Console.ReadLine();

                string chosen = "";
                if (int.TryParse(input, out int idx) &&
                    idx >= 1 && idx <= options.Count)
                {
                    chosen = options[idx - 1];
                }

                if (question.ValidateAnswer(chosen))
                {
                    Console.WriteLine("Correct!");
                    score++;
                }
                else
                {
                    Console.WriteLine($"Wrong. Correct answer: {question.QuestionCorrectAnswer}"); 
                }
            }

            string result = $"{DateTime.Now:yyyy-MM-dd HH:mm} | {quiz.QuizTitle} | Score: {score}/{quiz.QuizQuestions.Count}";
            quizResults.Add(result);

            Console.WriteLine($"\nYour score: {score}/{quiz.QuizQuestions.Count}");
            Console.WriteLine("Result saved.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

        }
        
        

        public void ViewResults()
        {
            Console.Clear();
            Console.WriteLine("=== Your Results ===\n");

            if (quizResults.Count == 0)
            {
                Console.WriteLine("No results yet. Play a quiz first.");
            }
            else
            {
                foreach (var result in quizResults)
                {
                    Console.WriteLine(result);
                }
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }


    }
}

