using GradeBook.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        public RankedGradeBook(string name) : base(name)
        {
            Type = GradeBookType.Ranked;
        }

        public override char GetLetterGrade(double averageGrade)
        {
            if (Students.Count < 5)
                throw new InvalidOperationException("Ranked-grading requires a minimum of 5 students to work");

            if (averageGrade == 0)
                return 'F';
            
            // sort grades descending and create a list of grades
            List<Student> studentsSorted = Students.OrderByDescending(x => x.AverageGrade).ToList();
            List<double> averageGrades = new List<double>();

            foreach (var student in studentsSorted)
            {
                averageGrades.Add(student.AverageGrade);
            }

            // create chunk of grades every 20% in a list
            int chunk = Students.Count / 5;
            int indexChunk = chunk;
            int j = 0;
            List<List<double>> chunksOfGrades = new List<List<double>>();           
            for(int i = 0; i < 5; i++)
            {
                List<double> sublist = new List<double>();
                for (; j < indexChunk; j++)
                {                    
                    sublist.Add(averageGrades[j]);
                }
                chunksOfGrades.Add(sublist);
                indexChunk += chunk;
            }
            
            // get a letter by grades
            char letterGrade = (char) 65;
            foreach (List<double> sublist in chunksOfGrades)
            {
                foreach(double grade in sublist)
                {
                    if(averageGrade < grade)
                    {
                        letterGrade++;
                        break;
                    }
                }
            }

            return letterGrade;
        }
    }
}