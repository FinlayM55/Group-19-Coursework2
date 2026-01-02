using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OnlineQuizSystem
{
    internal class QuizSystem
    {
        //Variables
        private List<Admin> adminUsers = new List<Admin>();
        private List<Student> studentUser = new List<Student>();
        private List<Quiz> quiz = new List<Quiz>();
        private List<Category> categories = new List<Category>();

        //Properties
        public List<Admin> AdminUsers => adminUsers;
        public List<Student> StudentUsers => studentUser;
        public List<Quiz> Quiz => quiz;
        public List<Category> Categories => categories;

        //methods

        public void ShowMainMenu() // Displays the main menu
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Online Quiz System ===");
                Console.WriteLine("1. Admin login");
                Console.WriteLine("2. Student login");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AuthenticateAdmin();
                        break;
                    case "2":
                        AuthenticateStudent();
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void LoadQuestionsFromCSV(string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, fileName);

            if (!File.Exists(path))
            {
                Console.WriteLine("questions.csv not found in working directory.");
                Console.ReadKey();
                return;
            }

            quiz.Clear();

            string[] lines = File.ReadAllLines(path);

            if (lines.Length <= 1)
            {
                Console.WriteLine("questions.csv contains only a header (no data).");
                Console.ReadKey();
                return;
            }

            // Ensure there is at least one category to attach the quiz to
            Category defaultCategory = categories.FirstOrDefault();
            if (defaultCategory == null)
            {
                defaultCategory = new Category(1, "General", "Auto-created category");
                categories.Add(defaultCategory);
            }

            // Create a quiz to hold all questions from the CSV
            Quiz defaultQuiz = new Quiz(
                quizID: 1,
                quizTitle: "General Quiz",
                quizDescription: "Loaded from questions.csv",
                quizCategory: defaultCategory,
                quizDate: DateTime.Today
            );

            for (int i = 1; i < lines.Length; i++) // skip header
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                List<string> cols = SplitCsvLine(lines[i]); // comma separated 

                // checks expected number of columns
                if (cols.Count < 8)
                    continue;

                int questionId = int.Parse(cols[0]);
                string questionText = cols[1];

                List<string> options = new List<string>
        {
            cols[2], cols[3], cols[4], cols[5]
        };

                string correctAnswer = cols[6];
                string difficulty = cols[7];

                defaultQuiz.AddQuestion(
                    new Question(questionId, questionText, options, correctAnswer, difficulty)
                );
            }

            quiz.Add(defaultQuiz);

            Console.WriteLine($"Questions loaded into quiz: {defaultQuiz.QuizTitle} ({defaultQuiz.QuizQuestions.Count} questions)");

        }




        private List<string> SplitCsvLine(string line)
        {
            List<string> result = new List<string>();
            bool inQuotes = false;
            string current = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current += '"';
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current);
                    current = "";
                }
                else
                {
                    current += c;
                }
            }

            result.Add(current);
            return result;
        }

        public void LoadCategoriesFromCSV(string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, fileName);

            if (!File.Exists(path))
            {
                Console.WriteLine("categories.csv not found in working directory.");
                Console.ReadKey();
                return;
            }

            categories.Clear();

            string[] lines = File.ReadAllLines(path);

            // Start from index 1 to skip header
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                List<string> cols = SplitCsvLine(lines[i]);

                if (cols.Count < 3)
                    continue;

                int categoryId = int.Parse(cols[0]);
                string categoryName = cols[1];
                string categoryDescription = cols[2];

                categories.Add(
                    new Category(categoryId, categoryName, categoryDescription)
                );
            }

            Console.WriteLine("Categories loaded from CSV.");
        }

        public void SaveCategoriesToCSV(string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, fileName);

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine("CategoryID,CategoryName,CategoryDescription");

                foreach (var c in categories)
                {
                    writer.WriteLine(
                        $"{c.CategoryID}," +
                        $"{CsvEscape(c.CategoryName)}," +
                        $"{CsvEscape(c.CategoryDescription)}"
                    );
                }
            }
        }

        public void SaveQuestionsToCSV(string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, fileName);

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                // Header matches existing questions.csv
                writer.WriteLine("QuestionID,QuestionText,Option1,Option2,Option3,Option4,CorrectAnswer,Difficulty");

                foreach (var qz in quiz)
                {
                    foreach (var q in qz.QuizQuestions)
                    {
                        string o1 = q.QuestionOptions.Count > 0 ? q.QuestionOptions[0] : "";
                        string o2 = q.QuestionOptions.Count > 1 ? q.QuestionOptions[1] : "";
                        string o3 = q.QuestionOptions.Count > 2 ? q.QuestionOptions[2] : "";
                        string o4 = q.QuestionOptions.Count > 3 ? q.QuestionOptions[3] : "";

                        writer.WriteLine(
                            $"{q.QuestionID}," +
                            $"{CsvEscape(q.QuestionText)}," +
                            $"{CsvEscape(o1)}," +
                            $"{CsvEscape(o2)}," +
                            $"{CsvEscape(o3)}," +
                            $"{CsvEscape(o4)}," +
                            $"{CsvEscape(q.QuestionCorrectAnswer)}," +
                            $"{CsvEscape(q.QuestionDifficultyLevel)}"
                        );
                    }
                }
            }
        }


        // CSV escaping helper (commas/quotes) safe TRY TO EXPLAIN BETTER
        private string CsvEscape(string value)
        {
            if (value == null) return "";

            bool mustQuote = value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r");
            if (!mustQuote) return value;

            return $"\"{value.Replace("\"", "\"\"")}\"";
        }


        //Sample users for testing
        public void LoadSampleUsers()
        {
            adminUsers.Add(new Admin(DateTime.Now, 1, "admin", "admin123", "admin@example.com", "Admin"));
            studentUser.Add(new Student("Active", 1, "student", "stud123", "student@example.com", "Student"));
        }

        //Admin authentication
        public void AuthenticateAdmin()
        {
            Console.Clear();
            Console.WriteLine(" Admin Login ");
            Console.Write("Username: ");
            string? username = Console.ReadLine();
            Console.Write("Password: ");
            string? password = Console.ReadLine();

            Admin? admin = adminUsers.Find(a => a.UserName == username && a.Password == password);

            if (admin != null)
            {
                admin.LoginDate = DateTime.Now;
                Console.WriteLine($"Welcome, {admin.UserName}!");
                Console.ReadKey();
                DisplayAdminMenu(admin);
            }
            else
            {
                Console.WriteLine("Invalid admin credentials.");
                Console.ReadKey();
            }
        }

        //Student authentication
        public void AuthenticateStudent()
        {
            Console.Clear();
            Console.WriteLine("=== Student Login ===");
            Console.Write("Username: ");
            string? username = Console.ReadLine();
            Console.Write("Password: ");
            string? password = Console.ReadLine();

            Student? student = studentUser.Find(s => s.UserName == username && s.Password == password);

            if (student != null)
            {
                Console.WriteLine($"Welcome, {student.UserName}!");
                Console.ReadKey();
                DisplayStudentMenu(student);
            }
            else
            {
                Console.WriteLine("Invalid student credentials.");
                Console.ReadKey();
            }
        }

        private void DisplayAdminMenu(Admin admin)
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Admin Menu ===");
                Console.WriteLine("1. Show all questions");
                Console.WriteLine("2. Add question");
                Console.WriteLine("3. Remove question");
                Console.WriteLine("4. Update question");
                Console.WriteLine("5. Add quiz");
                Console.WriteLine("6. Manage categories");
                Console.WriteLine("7. Manage users");
                Console.WriteLine("8. Save to CSV");
                Console.WriteLine("9. Update profile");
                Console.WriteLine("10. Logout");
                Console.Write("Choose an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllQuestions();
                        break;

                    case "2":
                        AddQuestion();
                        break;

                    case "3":
                        RemoveQuestion();
                        break;

                    case "4":
                        UpdateQuestion();
                        break;

                    case "5":
                        AddQuiz();
                        break;

                    case "6":
                        ManageCategories();
                        break;

                    case "7":
                        ManageUsers();
                        break;

                    case "8":
                        SaveCategoriesToCSV("categories.csv");
                        SaveQuestionsToCSV("questions.csv");
                        Console.WriteLine("Saved.");
                        Console.ReadKey();
                        break;

                    case "9":
                        admin.UpdateProfile();
                        break;

                    case "10":
                        back = true;
                        break;


                    default:
                        Console.WriteLine("Invalid option. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void DisplayStudentMenu(Student student)
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Student Menu ===");
                Console.WriteLine("1. Play quiz");
                Console.WriteLine("2. View results");
                Console.WriteLine("3. Logout");
                Console.Write("Choose an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PlayQuiz(student);
                        break;
                    case "2":
                        student.ViewResults();
                        break;
                    case "3":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void PlayQuiz(Student student)
        {
            // Ensure we have quizzes loaded
            if (quiz.Count == 0)
            {
                Console.WriteLine("No quizzes are available to play.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Available Quizzes ===");

                for (int i = 0; i < quiz.Count; i++)
                {
                    string categoryName = quiz[i].QuizCategory != null
                        ? quiz[i].QuizCategory.CategoryName
                        : "Unknown Category";

                    Console.WriteLine($"{i + 1}. {quiz[i].QuizTitle} ({categoryName})");
                }

                Console.WriteLine("\n0. Back");
                Console.Write("Select a quiz number: ");

                string? input = Console.ReadLine();

                // Back
                if (input == "0")
                    return;

                // Validation for user input / selection
                if (!int.TryParse(input, out int choice) || choice < 1 || choice > quiz.Count)
                {
                    Console.WriteLine("Invalid selection. Press any key to try again...");
                    Console.ReadKey();
                    continue;
                }

                Quiz selectedQuiz = quiz[choice - 1];

                // If the quiz has no questions, don't try to play it 
                if (selectedQuiz.QuizQuestions == null || selectedQuiz.QuizQuestions.Count == 0)
                {
                    Console.WriteLine("That quiz has no questions yet.");
                    Console.WriteLine("Press any key to select another quiz...");
                    Console.ReadKey();
                    continue;
                }

                // Call the Student's PlayQuiz method
                student.PlayQuiz(selectedQuiz);

                // After quiz completes, ask what to do next
                // helpful for navigation around menus
                Console.WriteLine("\n1. Play another quiz");
                Console.WriteLine("0. Back to student menu");
                Console.Write("Choose an option: ");

                string? next = Console.ReadLine();
                if (next == "0")
                    return;

                // otherwise loops back and shows quiz list again
            }
        }

        public void ShowAllQuestions()
        {
            Console.Clear();
            Console.WriteLine("=== All Questions ===\n");

            if (quiz.Count == 0)
            {
                Console.WriteLine("No quizzes loaded.");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

            foreach (var qz in quiz)
            {
                string categoryName = qz.QuizCategory != null ? qz.QuizCategory.CategoryName : "Unknown Category";
                Console.WriteLine($"Quiz [{qz.QuizID}] {qz.QuizTitle} ({categoryName})");

                if (qz.QuizQuestions == null || qz.QuizQuestions.Count == 0)
                {
                    Console.WriteLine("  (No questions in this quiz)");
                }
                else
                {
                    foreach (var q in qz.QuizQuestions)
                    {
                        Console.WriteLine($"  Q{q.QuestionID}: {q.QuestionText}");

                        // Show options for admin checking
                        if (q.QuestionOptions != null && q.QuestionOptions.Count > 0)
                        {
                            for (int i = 0; i < q.QuestionOptions.Count; i++)
                            {
                                Console.WriteLine($"     {i + 1}. {q.QuestionOptions[i]}");
                            }
                        }

                        Console.WriteLine($"     Answer: {q.QuestionCorrectAnswer}");
                        Console.WriteLine($"     Difficulty: {q.QuestionDifficultyLevel}");
                    }
                }

                Console.WriteLine(); // blank line between quizzes
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private void ManageCategories()
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Manage Categories ===");
                Console.WriteLine("1. View categories");
                Console.WriteLine("2. Add category");
                Console.WriteLine("3. Remove category");
                Console.WriteLine("0. Back");
                Console.Write("Choose an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewCategories();
                        break;
                    case "2":
                        AddCategory();
                        break;
                    case "3":
                        RemoveCategory();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ViewCategories()
        {
            Console.Clear();
            Console.WriteLine("=== Categories ===\n");

            if (categories.Count == 0)
            {
                Console.WriteLine("No categories loaded.");
            }
            else
            {
                foreach (var c in categories)
                {
                    Console.WriteLine($"[{c.CategoryID}] {c.CategoryName}");
                    Console.WriteLine($"    {c.CategoryDescription}\n");
                }
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private void AddCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Add Category ===");

            int newId = GenerateNextCategoryId();

            Console.Write("Category name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Category description: ");
            string desc = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Category name cannot be empty.");
                Console.ReadKey();
                return;
            }

            categories.Add(new Category(newId, name, desc));

            SaveCategoriesToCSV("categories.csv");

            Console.WriteLine($"\nCategory added with ID {newId}.");
            Console.ReadKey();
            
        }


        private void RemoveCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Remove Category ===");

            Console.Write("Enter Category ID to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            bool categoryInUse = quiz.Any(q =>
                q.QuizCategory != null &&
                q.QuizCategory.CategoryID == id);

            if (categoryInUse)
            {
                Console.WriteLine("Category is used by a quiz and cannot be removed.");
                Console.ReadKey();
                return;
            }

            int removed = categories.RemoveAll(c => c.CategoryID == id);

            SaveCategoriesToCSV("categories.csv");


            Console.WriteLine(removed > 0 ? "Category removed." : "Category not found.");
            Console.ReadKey();
        }

        private int GenerateNextCategoryId()
        {
            return categories.Count == 0 ? 1 : categories.Max(c => c.CategoryID) + 1;
        }

        private void AddQuestion()
        {
            if (quiz.Count == 0)
            {
                Console.WriteLine("No quizzes exist. Add a quiz first.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("=== Add Question ===\n");

            Quiz selectedQuiz = SelectQuiz();
            if (selectedQuiz == null) return;

            int newQuestionId = GenerateNextQuestionId(selectedQuiz);

            Console.Write("Question text: ");
            string text = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Question text cannot be empty.");
                Console.ReadKey();
                return;
            }

            List<string> options = new List<string>();
            for (int i = 1; i <= 4; i++)
            {
                Console.Write($"Option {i}: ");
                options.Add(Console.ReadLine() ?? "");
            }

            Console.Write("Correct answer (type exactly one of the options): ");
            string correct = Console.ReadLine() ?? "";

            Console.Write("Difficulty (Easy/Medium/Hard): ");
            string difficulty = Console.ReadLine() ?? "Easy";

            Question q = new Question(newQuestionId, text, options, correct, difficulty);

            selectedQuiz.AddQuestion(q);

            
            SaveQuestionsToCSV("questions.csv");

            Console.WriteLine("\nQuestion added and saved.");
            Console.ReadKey();
        }

        private void RemoveQuestion()
        {
            if (quiz.Count == 0)
            {
                Console.WriteLine("No quizzes loaded.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("=== Remove Question ===\n");

            Quiz selectedQuiz = SelectQuiz();
            if (selectedQuiz == null) return;

            if (selectedQuiz.QuizQuestions.Count == 0)
            {
                Console.WriteLine("This quiz has no questions.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Question ID to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int qid))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            int before = selectedQuiz.QuizQuestions.Count;
            selectedQuiz.RemoveQuestion(qid);

            if (selectedQuiz.QuizQuestions.Count < before)
            {
                SaveQuestionsToCSV("questions.csv");
                Console.WriteLine("Question removed and saved.");
            }
            else
            {
                Console.WriteLine("Question ID not found.");
            }

            Console.ReadKey();
        }

        private void UpdateQuestion()
        {
            if (quiz.Count == 0)
            {
                Console.WriteLine("No quizzes loaded.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("=== Update Question ===\n");

            Quiz selectedQuiz = SelectQuiz();
            if (selectedQuiz == null) return;

            if (selectedQuiz.QuizQuestions.Count == 0)
            {
                Console.WriteLine("This quiz has no questions.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Question ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int qid))
            {
                Console.WriteLine("Invalid ID.");
                Console.ReadKey();
                return;
            }

            Question question = selectedQuiz.QuizQuestions.Find(q => q.QuestionID == qid);
            if (question == null)
            {
                Console.WriteLine("Question not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nCurrent text: {question.QuestionText}");
            Console.Write("New text (leave blank to keep): ");
            string newText = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(newText))
                question.QuestionText = newText;

            Console.WriteLine($"\nCurrent correct answer: {question.QuestionCorrectAnswer}");
            Console.Write("New correct answer (leave blank to keep): ");
            string newCorrect = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(newCorrect))
                question.QuestionCorrectAnswer = newCorrect;

            // used for saving after updating
            // didn't test properly as scared it would break things but seems to work
            SaveQuestionsToCSV("questions.csv");

            Console.WriteLine("\nQuestion updated and saved.");
            Console.ReadKey();
        }

        private Quiz SelectQuiz()
        {
            Console.WriteLine("Select a quiz:\n");
            for (int i = 0; i < quiz.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {quiz[i].QuizTitle}");
            }

            Console.Write("\nEnter quiz number (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > quiz.Count)
                return null;

            if (choice == 0) return null;

            return quiz[choice - 1];
        }

        private int GenerateNextQuestionId(Quiz qz)
        {
            if (qz.QuizQuestions == null || qz.QuizQuestions.Count == 0)
                return 1;

            return qz.QuizQuestions.Max(q => q.QuestionID) + 1;
        }


        private void AddQuiz()
        {
            Console.Clear();
            Console.WriteLine("=== Add Quiz ===\n");

            if (categories.Count == 0)
            {
                Console.WriteLine("No categories exist. Add a category first.");
                Console.ReadKey();
                return;
            }

            int newQuizId = GenerateNextQuizId();

            Console.Write("Quiz title: ");
            string title = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Quiz title cannot be empty.");
                Console.ReadKey();
                return;
            }

            Console.Write("Quiz description (optional): ");
            string description = Console.ReadLine() ?? "";

            Category selectedCategory = SelectCategory();
            if (selectedCategory == null) return;

            DateTime quizDate = DateTime.Today; 
            Quiz newQuiz = new Quiz(newQuizId, title, description, selectedCategory, quizDate);

            // make sure there are no quizzes with no questions as our CSV format doesnt allow it
            Console.WriteLine("\nAdd at least ONE question now so the quiz can be saved.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            AddQuestionToSpecificQuiz(newQuiz);

            // Add quiz to system list
            quiz.Add(newQuiz);

            // Save questions.csv so the new quiz is saved
            SaveQuestionsToCSV("questions.csv");

            Console.WriteLine("\nQuiz created and saved.");
            Console.ReadKey();
        }

        private int GenerateNextQuizId()
        {
            if (quiz.Count == 0) return 1;
            return quiz.Max(qz => qz.QuizID) + 1;
        }

        private Category SelectCategory()
        {
            Console.WriteLine("\nSelect a category:\n");

            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. [{categories[i].CategoryID}] {categories[i].CategoryName}");
            }

            Console.Write("\nEnter category number (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > categories.Count)
            {
                Console.WriteLine("Invalid choice.");
                Console.ReadKey();
                return null;
            }

            if (choice == 0) return null;

            return categories[choice - 1];
        }


        //think this works but can't remember testing
        private void AddQuestionToSpecificQuiz(Quiz targetQuiz)
        {
            int newQuestionId = GenerateNextQuestionId(targetQuiz);

            Console.Clear();
            Console.WriteLine($"=== Add Question to: {targetQuiz.QuizTitle} ===\n");

            Console.Write("Question text: ");
            string text = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Question text cannot be empty.");
                Console.ReadKey();
                return;
            }

            List<string> options = new List<string>();
            for (int i = 1; i <= 4; i++)
            {
                Console.Write($"Option {i}: ");
                options.Add(Console.ReadLine() ?? "");
            }

            Console.Write("Correct answer (type exactly one of the options): ");
            string correct = Console.ReadLine() ?? "";

            Console.Write("Difficulty (Easy/Medium/Hard): ");
            string difficulty = Console.ReadLine() ?? "Easy";

            Question q = new Question(newQuestionId, text, options, correct, difficulty);
            targetQuiz.AddQuestion(q);

            Console.WriteLine("\nQuestion added.");
            Console.ReadKey();
        }


        //lines up with our UML and is good for seeing things working well
        private void ManageUsers()
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== Manage Users ===");
                Console.WriteLine("1. View all users");
                Console.WriteLine("2. Add student");
                Console.WriteLine("3. Add admin");     
                Console.WriteLine("4. Remove user");
                Console.WriteLine("0. Back");
                Console.Write("Choose an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllUsers();
                        break;
                    case "2":
                        AddStudent();
                        break;
                    case "3":
                        AddAdmin();          
                        break;
                    case "4":
                        RemoveUserByUsername();
                        break;
                    case "0":
                        back = true;
                        break;
                }
            }
        }

        //same here
        private void ViewAllUsers()
        {
            Console.Clear();
            Console.WriteLine("=== All Users ===\n");

            Console.WriteLine("-- Admins --");
            if (adminUsers.Count == 0)
            {
                Console.WriteLine("(none)\n");
            }
            else
            {
                foreach (var a in adminUsers)
                {
                    Console.WriteLine($"ID: {a.ID} | Username: {a.UserName} | Email: {a.Email} | Role: {a.Role} | Last login: {a.LoginDate}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("-- Students --");
            if (studentUser.Count == 0)
            {
                Console.WriteLine("(none)\n");
            }
            else
            {
                foreach (var s in studentUser)
                {
                    Console.WriteLine($"ID: {s.ID} | Username: {s.UserName} | Email: {s.Email} | Role: {s.Role} | Status: {s.Status}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private void AddStudent()
        {
            Console.Clear();
            Console.WriteLine("=== Add Student ===\n");

            int newId = GenerateNextUserId();

            Console.Write("Username: ");
            string userName = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Username cannot be empty.");
                Console.ReadKey();
                return;
            }

            // prevent duplicates across admins & students
            if (UsernameExists(userName))
            {
                Console.WriteLine("That username already exists.");
                Console.ReadKey();
                return;
            }

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Status (Active/Suspended): ");
            string status = Console.ReadLine() ?? "Active";

            // role is fixed for students (splits the two user types essentially)
            string role = "Student";

            studentUser.Add(new Student(status, newId, userName, password, email, role));

            Console.WriteLine($"\nStudent added with ID {newId}.");
            Console.ReadKey();
        }

        private void AddAdmin()
        {
            Console.Clear();
            Console.WriteLine("=== Add Admin ===\n");

            int newId = GenerateNextUserId();

            Console.Write("Username: ");
            string userName = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Username cannot be empty.");
                Console.ReadKey();
                return;
            }

            if (UsernameExists(userName))
            {
                Console.WriteLine("That username already exists.");
                Console.ReadKey();
                return;
            }

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            string role = "Admin";

            adminUsers.Add(
                new Admin(DateTime.Now, newId, userName, password, email, role)
            );

            Console.WriteLine($"\nAdmin added with ID {newId}.");
            Console.ReadKey();
        }


        private void RemoveUserByUsername()
        {
            Console.Clear();
            Console.WriteLine("=== Remove User ===\n");

            Console.Write("Enter username to remove: ");
            string userName = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Username cannot be empty.");
                Console.ReadKey();
                return;
            }

            int removedAdmins = adminUsers.RemoveAll(a => a.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            int removedStudents = studentUser.RemoveAll(s => s.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (removedAdmins + removedStudents > 0)
                Console.WriteLine("User removed.");
            else
                Console.WriteLine("User not found.");

            Console.ReadKey();
        }

        private bool UsernameExists(string userName)
        {
            return adminUsers.Any(a => a.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase))
                || studentUser.Any(s => s.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        private int GenerateNextUserId()
        {
            int maxAdminId = adminUsers.Count == 0 ? 0 : adminUsers.Max(a => a.ID);
            int maxStudentId = studentUser.Count == 0 ? 0 : studentUser.Max(s => s.ID);
            return Math.Max(maxAdminId, maxStudentId) + 1;
        }





    }


}






    



