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

        public string QuestionCorrectAnswer
        {
            get { return questionsCorrectAnswer; }
            set { questionsCorrectAnswer = value; }
        }

        public string QuestionDifficultyLevel
        {
            get { return questionDifficultyLevel; }
            set { questionDifficultyLevel = value; }
        }

        //Default constructor

        public Question()
        {
            questionOptions = new List<string>();
        }

        //Custom constructor
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

        
    }

}
