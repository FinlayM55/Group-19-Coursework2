using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//created by Mark Stewart 30/11/2025


namespace OnlineQuizSystem
{
    internal class Question
    {
        // variables
        private int questionID;
        private string questionText;
        private List<string> questionOptions;
        private string questionsCorrectAnswer;
        private string questionDifficultyLevel;

        // getters and setters
        public int QuestionID
        {
            get { return questionID; }
            set { questionID = value; }
        }

        public string QuestionText
        {
            get { return questionText; }
            set { questionText = value; }
        }

        public List<string> QuestionOptions
        {
            get { return questionOptions; }
            set { questionOptions = value; }
        }

        public string QuestionsCorrectAnswer
        {
            get { return questionsCorrectAnswer; }
            set { questionsCorrectAnswer = value; }
        }

        public string QuestionDifficultyLevel
        {
            get { return questionDifficultyLevel; }
            set { questionDifficultyLevel = value; }
        }

        // constructor
        public Question(int questionID, string questionText, List<string> questionOptions, string questionsCorrectAnswer, string questionDifficultyLevel)
        {
            this.questionID = questionID;
            this.questionText = questionText;
            this.questionOptions = questionOptions;
            this.questionsCorrectAnswer = questionsCorrectAnswer;
            this.questionDifficultyLevel = questionDifficultyLevel;
        }

        // methods
        //used trim and ignore case to validate answer incase of variance in user input
        public bool ValidateAnswer(string userAnswer)
        {
            return userAnswer.Trim().Equals(questionsCorrectAnswer.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public List<string> GetOptions()
        {
            return questionOptions;
        }

        //creating and populating the list of questions

        List<Question> oopQuestions = new List<Question>
{
    new Question(1, "What does OOP stand for?",
        new List<string> { "Object-Oriented Programming", "Operational Output Processing", "Open Order Protocol", "Overloaded Operator Procedure" },
        "Object-Oriented Programming", "Easy"),

    new Question(2, "Which of the following is NOT a core principle of OOP?",
        new List<string> { "Encapsulation", "Polymorphism", "Abstraction", "Compilation" },
        "Compilation", "Easy"),

    new Question(3, "What is encapsulation in object-oriented programming?",
        new List<string> { "Binding data and methods", "Inheritance", "Overloading", "Creating objects" },
        "Binding data and methods", "Medium"),

    new Question(4, "Which keyword is used in C# to inherit a class?",
        new List<string> { "extends", "inherits", ":", "base" },
        ":", "Medium"),

    new Question(5, "What is the purpose of a constructor in a class?",
        new List<string> { "To destroy objects", "To initialize objects", "To inherit methods", "To override properties" },
        "To initialize objects", "Easy"),

    new Question(6, "Which concept allows multiple methods with the same name but different parameters?",
        new List<string> { "Inheritance", "Polymorphism", "Overloading", "Encapsulation" },
        "Overloading", "Medium"),

    new Question(7, "What is the base class for all classes in C#?",
        new List<string> { "System.Object", "BaseClass", "RootClass", "MainClass" },
        "System.Object", "Hard"),

    new Question(8, "What is the difference between a class and an object?",
        new List<string> { "Class is an instance, object is a blueprint", "Class is a blueprint, object is an instance", "They are the same", "Object inherits class" },
        "Class is a blueprint, object is an instance", "Medium"),

    new Question(9, "Which access modifier makes a member accessible only within its own class?",
        new List<string> { "public", "private", "protected", "internal" },
        "private", "Easy"),

    new Question(10, "What is polymorphism in OOP?",
        new List<string> { "Ability to hide data", "Ability to inherit methods", "Ability to take many forms", "Ability to override constructors" },
        "Ability to take many forms", "Medium")
        };

        //see where to put foreach loop to display questions (probably in quiz class? i.e. getQuestions())

    }

}
