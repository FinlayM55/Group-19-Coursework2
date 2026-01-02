namespace OnlineQuizSystem
{
    //Created by Mark Stewart 08/12/2025
    internal class Program
    {
        static void Main(string[] args)
        {

            QuizSystem quizSystem = new QuizSystem();

            //intro message
            Console.WriteLine("Welcome to Group 19's Online Quiz System");
            Console.ReadKey();

            //load categories first (from working directory)
            quizSystem.LoadCategoriesFromCSV("categories.csv");


            // then load questions from CSV in working directory
            quizSystem.LoadQuestionsFromCSV("questions.csv");

            //load 2 sample users for testing
            quizSystem.LoadSampleUsers();

            
            // Start the main menu loop
            quizSystem.ShowMainMenu();


        }

    }
}

