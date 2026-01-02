using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//created by Mark Stewart 01/12/2025

namespace OnlineQuizSystem
{
    internal class Category
    {
        private int categoryID;
        private string categoryName;
        private string categoryDescription;

        //getters and setters

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }   

        public string CategoryDescription
        {
            get { return categoryDescription; }
            set { categoryDescription = value; }
        }

        //Default constructor
        public Category()
        {

        }
        //Custom constructor
        public Category(int categoryID, string categoryName, string categoryDescription)
        {
            this.categoryID = categoryID;
            this.categoryName = categoryName;
            this.categoryDescription = categoryDescription;
        }

        //methods
        

        public void updateDescription(string description)  //updates category description
        {
            categoryDescription = description;
        }

        //removed get description method as the menu already displays this information (redundant)
    }
}
