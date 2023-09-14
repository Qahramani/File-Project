using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// namespaces for File 
using System.Text;
using System.IO;
using System.Net.Http.Headers;

namespace File_exercise
{
    internal class Program
    {
        const string MyDBDirectory = @"C:\MyDB"; // or "C:\\MyDB" ; @ shows that there is no escape char like slash \
                                                 // and takes evething as string
        const string DateFormat = "yyyy-MM-dd";
        static void Main(string[] args)
        {
            Console.WriteLine("What do u want to do ?\n1-Create student\n2-See all registered students");

            int option = int.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                    var student = CreateStudent();
                    RegisterStudentInDB(student);
                    break;
                case 2:
                    var students = GetStudents();
                    PrintStudents(students);
                    break;
                default:
                    throw new ArgumentException(nameof(option));
            }
            Console.ReadKey();
        }

        public static Student CreateStudent()
        {

            Console.WriteLine("Registering a student....");
            Console.WriteLine();

            var student = new Student();

            Console.WriteLine("Name : ");
            student.Name = Console.ReadLine();

            Console.WriteLine("Surename : ");
            student.Surname = Console.ReadLine();

            Console.WriteLine($"Date of Birth ({DateFormat}) : ");
            student.DateOfBirth = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Your Id: ");
            student.Id = int.Parse(Console.ReadLine());
            //student.Id = Guid.NewGuid();

            return student;
        }

        public static void RegisterStudentInDB(Student student)
        {
            var file = Path.Combine(MyDBDirectory, student.Id + ".txt");
            if(File.Exists(file))
            {
                Console.WriteLine("This student has already been registered");
            }
            else
            {
                //File.Create(file);
                var filetext = $"Name: {student.Name}\nSurename: {student.Surname}\nDateOfBirth: {student.DateOfBirth.ToString(DateFormat)}";
                File.WriteAllText(file,filetext);

                Console.WriteLine("Student is succesfully registered");
            }
        }
        public static Student[] GetStudents()
        {
            var directory  = new DirectoryInfo(MyDBDirectory);
            var files = directory.GetFiles();

            var students = new Student[100];
            var i = 0;
            foreach (var file in files)
            {
                var fileLines = File.ReadAllLines(file.FullName);
                var student = new Student();

                student.Name = fileLines[0].Split(':')[1].Trim();
                student.Surname = fileLines[1].Split(':')[1].Trim(); 
                student.DateOfBirth = DateTime.Parse(fileLines[2].Split(':')[1].Trim());

                students[i]= student;
                i++;
            }
            return students;
        }
        public static void PrintStudents(Student[] students)
        {
            foreach (var student in students)
            {
                if (student is null) break;
                PrintStudent(student);
                Console.WriteLine();
                Console.WriteLine("---------------------");
                Console.WriteLine();
            }
        }
        public static void PrintStudent(Student student)
        {
            Console.WriteLine($"{nameof(student.Name)}: {student.Name}");
            Console.WriteLine($"{nameof(student.Surname)}: {student.Surname}");
            Console.WriteLine($"{nameof(student.DateOfBirth)}: {student.DateOfBirth.ToString(DateFormat)}");
        }

    }


    public class Student
        {
            //public Guid Id { get; set; }// -> everytime gives new value
            public int Id { get; set; } 
            public string Name { get; set; }
            public string Surname { get; set; } 
            public DateTime DateOfBirth { get; set; }   
        }
}
