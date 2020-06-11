﻿using Colorful;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Console = Colorful.Console;

namespace assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            bool to_continue = true;

            /*FigletFont font = FigletFont.Load("epic.flf");
            Figlet figlet = new Figlet(font);

            Console.WriteLine(figlet.ToAscii("Belvedere"), ColorTranslator.FromHtml("#8AFFEF"));
            Console.WriteAscii("test", Color.FromArgb(0, 255, 0));*/
            string menu_text_ascii = @"  __  __                  
 |  \/  |                 
 | \  / | ___ _ __  _   _ 
 | |\/| |/ _ \ '_ \| | | |
 | |  | |  __/ | | | |_| |
 |_|  |_|\___|_| |_|\__,_| 
                                     ";

            
            while (to_continue)
            {
                Console.WriteLine(menu_text_ascii);
                Console.WriteLine("(1) Pay");
                Console.WriteLine("(2) Months");
                Console.WriteLine("(3) Exam");

                Console.WriteLine("Exit");

                string menu = Console.ReadLine();
                switch (menu)
                {
                    case "1":
                        Console.Write("Please enter the number of hours worked. Valid input is between 0 and 60: ");
                        string read_line = Console.ReadLine();
                        int time_worked;
                        bool success = Int32.TryParse(read_line, out time_worked);
                        if (success != true)
                        {
                            Console.WriteLine("'{0}' is not a valid number.", read_line);
                            break;
                        }

                        decimal pay_amount = Pay_calc(time_worked);
                        Console.WriteLine(pay_amount.ToString("C2"));

                        break;

                    case "2":
                        int chosen_month_int = get_months();
                        int length_of_month = Months(chosen_month_int);
                        break;

                    case "3":
                        exam();
                        break;

                    default:
                        to_continue = false;
                        break;
                }
                //Console.Clear();
            }

        }

        // 
        // $27/hour
        // tax rate 20% after 5 hours
        // 35% after 45 hours
        // no more than 60h worked
        //
        static decimal Pay_calc(decimal hours_worked)
        {
            Console.Clear();
            decimal forty_five = 45;
            decimal five = 5;



            if (hours_worked >= 60)
            {
                Console.WriteLine("Unable to pay more than 60h.");
                return (0);
            }

            else if (hours_worked < 0)
            {
                Console.WriteLine("Please enter a positive number.");
                return (0);
            }

            else if(hours_worked > 45)
            {
                decimal val = (tax_calc((hours_worked - 45), 35, 27));
                return (val + Pay_calc(forty_five));
            }

            else if (hours_worked > 5)
            {
                decimal val = (tax_calc((hours_worked - 5), 20, 27));
                return (val + Pay_calc(five));
            }

            else if (hours_worked <= 5)
            {
                return (tax_calc(hours_worked, 0, 27));

            }

            else
            {
                Console.WriteLine("Some undefined error");
                return 0;
            }


        }

        // 
        // returns the amount earned with tax deducted
        // 

        static decimal tax_calc(decimal hours, decimal percentage_tax_rate, decimal pay_rate)
        {
            return (hours*pay_rate - (hours * pay_rate * (percentage_tax_rate / 100)));
        }


        static int Months(int month_type)
        {
            string[] months_array = { "January", "Febuary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            int[] months_length = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            if (month_type == 2)
            {
                int year = 0;
                while (year < 1)
                {
                    Console.WriteLine("What year are you looking for?");
                    year = int.Parse(Console.ReadLine());
                    if (year < 1)
                    {
                        Console.WriteLine("Year is before 0 please try again");
                    }
                    else
                    {
                        continue;
                    }
                }

                if (((year % 4) == 0) & ((year % 100) != 0) || ((year % 400) == 0))
                {
                    Console.WriteLine("yeeees");
                    return (29);
                }
                else
                {
                    Console.WriteLine("Not a leap year");
                    return (28);
                }
                
            }

            else
            {
                Console.WriteLine(string.Join(",", months_length));
                Console.WriteLine(months_length[month_type]);
                return (months_length[month_type]);
            }
            
            

            //return 0;
        }

        static int get_months()
        {
            string months_text_ascii = @"  __  __             _   _         
 |  \/  |           | | | |        
 | \  / | ___  _ __ | |_| |__  ___ 
 | |\/| |/ _ \| '_ \| __| '_ \/ __|
 | |  | | (_) | | | | |_| | | \__ \
 |_|  |_|\___/|_| |_|\__|_| |_|___/
                                   
                                   ";

            Console.Clear();

            string[] months_array = { "January", "Febuary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            Console.WriteLine(months_text_ascii);

            for (int i = 0; i < months_array.Length; i++)
            {
                Console.WriteLine("{0} {1}", i+1, months_array[i]);
            }

            int month_chosen = int.Parse(Console.ReadLine());
            return month_chosen;
        }

        static void exam()
        {
            string exam_text_ascii = @"  ______                     
 |  ____|                    
 | |__  __  ____ _ _ __ ___  
 |  __| \ \/ / _` | '_ ` _ \ 
 | |____ >  < (_| | | | | | |
 |______/_/\_\__,_|_| |_| |_|
                             
                             ";

            Console.Clear();

            Console.WriteLine(exam_text_ascii);

            string marks_file = "marks.txt";
            string file_name = Directory.GetCurrentDirectory();

            if (File.Exists(marks_file))
            {
                //
                // reads file "marks_file" into a list where every line is a element
                //
                string[] lines = System.IO.File.ReadAllLines(marks_file);

                //
                // creates list names that contains the objects students which have
                // name and grade
                //

                List<Student> names = new List<Student>();


                foreach (string line in lines)
                {
                    //
                    // splits every line into "name" and "grade"
                    //

                    var words = line.Split(' ');
                    names.Add(new Student(words[1], Int16.Parse(words[0]), ""));

                }
                //
                // sorts the students by their grade
                //
                var result1 = names.OrderByDescending(a => a.Score).Reverse();
                if (File.Exists(marks_file)) { File.Delete("distinctionList.txt"); }
                foreach (Student stu in result1)
                {
                    
                    if ((stu.Score > 100) | (stu.Score < 0)) { stu.Grade = "Error"; }
                    else if (stu.Score > 85) { stu.Grade = "Distinction"; }
                    else if (stu.Score >= 75) { stu.Grade = "Credit"; }
                    else if (stu.Score > 50) { stu.Grade = "Pass"; }
                    else { stu.Grade = "Fail"; }

                    
                    string to_file = (stu.Grade + " " + stu.Score + "% " + stu.Name);
                    Console.WriteLine(to_file);
                    if (stu.Grade == "Distinction")
                    {
                        File.AppendAllText("distinctionList.txt", to_file + Environment.NewLine);
                    }
                    
                }
                
                
                

                Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);


            }
            else { Console.WriteLine("File 'marks.txt' does not exist\nPlease re-run the program once this file exists"); }






        }


    }

    public class Student
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public string Grade { get; set; }

        public Student(string name, int score, string grade)
        {
            Name = name;
            Score = score;
            Grade = grade;
        }
    }
}
