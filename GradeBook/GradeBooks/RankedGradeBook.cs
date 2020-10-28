using GradeBook.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        public RankedGradeBook(string name, bool isWeighted) : base(name, isWeighted)
        {
            Type = GradeBookType.Ranked;
            IsWeighted = isWeighted;
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

        public override void CalculateStatistics()
        {
            if (Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a student's overall grade.");
                return;
            }
            base.CalculateStatistics();
        }

        public override void CalculateStudentStatistics(string name)
        {
            if (Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a student's overall grade.");
                return;
            }
            base.CalculateStudentStatistics(name);
        }
    }
}