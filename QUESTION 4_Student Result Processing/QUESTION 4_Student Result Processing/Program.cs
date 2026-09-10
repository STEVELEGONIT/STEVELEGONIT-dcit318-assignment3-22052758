using System;
using System.Collections.Generic;
using System.IO;

namespace StudentResults
{
    public class Student
    {
        public int Id;
        public string FullName;
        public int Score;

        public string GetGrade()
        {
            if (Score >= 80) return "A";
            if (Score >= 70) return "B";
            if (Score >= 60) return "C";
            if (Score >= 50) return "D";
            return "F";
        }
    }

    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message)
            : base(message)
        {
        }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message)
            : base(message)
        {
        }
    }

    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string path)
        {
            List<Student> students = new List<Student>();

            using (StreamReader sr = new StreamReader(path))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] data = line.Split(',');

                    if (data.Length != 3)
                    {
                        throw new MissingFieldException(
                            "Missing field detected");
                    }

                    int score;

                    if (!int.TryParse(data[2], out score))
                    {
                        throw new InvalidScoreFormatException(
                            "Invalid score");
                    }

                    students.Add(new Student
                    {
                        Id = int.Parse(data[0]),
                        FullName = data[1],
                        Score = score
                    });
                }
            }

            return students;
        }

        public void WriteReportToFile(
            List<Student> students,
            string outputPath)
        {
            using (StreamWriter sw = new StreamWriter(outputPath))
            {
                foreach (Student s in students)
                {
                    sw.WriteLine(
                        string.Format(
                            "{0} (ID: {1}): Score = {2}, Grade = {3}",
                            s.FullName,
                            s.Id,
                            s.Score,
                            s.GetGrade()));
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string studentsPath = Path.Combine(baseDir, "students.txt");
                string reportPath = Path.Combine(baseDir, "report.txt");

                if (!File.Exists(studentsPath))
                {
                    File.WriteAllLines(studentsPath, new[]
                    {
                        "1,John Doe,85",
                        "2,Jane Smith,72",
                        "3,Bob Brown,48"
                    });
                    Console.WriteLine($"Created sample students file: {studentsPath}");
                }

                StudentResultProcessor processor = new StudentResultProcessor();

                List<Student> students = processor.ReadStudentsFromFile(studentsPath);

                processor.WriteReportToFile(students, reportPath);

                Console.WriteLine("Report generated.");
                Console.WriteLine($"Report path: {reportPath}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}