using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DetermineGender
{
    class Program
    {
        static void Main(string[] args)
        {
            StartProgram();
        }

        private static void StartProgram()
        {
            int numberInput;
            string name;

            while (true)
            {
                Console.WriteLine("1. Determine the gender of a single name.");
                Console.WriteLine("2. Determine the genders of multiple names.");
                Console.WriteLine("3. Determine the genders of multiple names from a file.");
                Console.Write("What would you like to do? ");
                string userInput = Console.ReadLine();

                bool isNumber = Int32.TryParse(userInput, out numberInput);

                if (isNumber) break;
                else Console.WriteLine("Not a valid input.");
            }

            switch (numberInput)
            {
                case 1:
                    Console.Write("What's the name you would like to determine the gender of? ");
                    name = Console.ReadLine();

                    string data = GetAPIData(name);
                    List<String> info = ParseData(data);
                    
                    double probability = Convert.ToDouble(info[1]) * 100;

                    Console.Clear();
                    Console.WriteLine("The name '" + name + "' has a " + probability + "% chance of being a " + info[0] + ".");

                    StartProgram();
                    break;

                case 2:
                    List<String> listOfNames = new List<String>();

                    while (true)
                    {
                        Console.Write("Enter name (-1 to stop): ");
                        string aName = Console.ReadLine();
                        if (aName != "-1") listOfNames.Add(aName);
                        else break;
                    }

                    MultiName(listOfNames);
                    break;

                case 3:
                    Console.Write("Specify the location of the file: ");
                    string location = Console.ReadLine();
                    string[] names = null;

                    try
                    {
                        names = System.IO.File.ReadAllLines(location);
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid location.");
                        StartProgram();
                    }

                    MultiName(names);
                    break;
            }
        }

        private static void MultiName(string[] allNames)
        {
            List<String> dataToSave = new List<String>();

            foreach (string n in allNames)
            {
                string apiData = GetAPIData(n);
                List<String> thisName = ParseData(apiData);
                double prob = Convert.ToDouble(thisName[1]) * 100;
                string thisData = null;
                if (!String.IsNullOrEmpty(thisName[0])) thisData = "The name '" + n + "' has a " + prob + "% chance of being a " + thisName[0] + ".";
                else continue;
                dataToSave.Add(thisData);
                Console.WriteLine(thisData);
            }

            Console.Write("Would you like to output this data to a text file? (y/n): ");
            string saveData = Console.ReadLine();

            if (saveData.Contains("y"))
            {
                Console.Write("Enter a destination for the file: ");
                string destination = Console.ReadLine();
                try
                {
                    foreach (string nameData in dataToSave)
                    {
                        System.IO.File.AppendAllText(destination, nameData + Environment.NewLine);
                    }
                }
                catch
                {
                    Console.WriteLine("Error occured saving data.");
                }
            }

            Console.Clear();
            StartProgram();
        }

        private static void MultiName(List<String> allNames)
        {
            List<String> dataToSave = new List<String>();

            foreach (string n in allNames)
            {
                string apiData = GetAPIData(n);
                List<String> thisName = ParseData(apiData);
                double prob = Convert.ToDouble(thisName[1]) * 100;
                string thisData = null;
                if (!String.IsNullOrEmpty(thisName[0])) thisData = "The name '" + n + "' has a " + prob + "% chance of being a " + thisName[0] + ".";
                else continue;
                dataToSave.Add(thisData);
                Console.WriteLine(thisData);
            }

            Console.Write("Would you like to output this data to a text file? (y/n): ");
            string saveData = Console.ReadLine();

            if (saveData.Contains("y"))
            {
                Console.Write("Enter a destination for the file: ");
                string destination = Console.ReadLine();
                try
                {
                    foreach (string nameData in dataToSave)
                    {
                        System.IO.File.AppendAllText(destination, nameData + Environment.NewLine);
                    }
                }
                catch
                {
                    Console.WriteLine("Error occured saving data.");
                }
            }

            Console.Clear();
            StartProgram();
        }

        private static string GetAPIData(string name)
        {
            WebRequest wr = WebRequest.Create("https://api.genderize.io/?name=" + name);
            WebResponse response = wr.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            System.IO.StreamReader dataReader = new System.IO.StreamReader(stream);
            string allData = dataReader.ReadToEnd();
            return allData;
        }

        private static List<String> ParseData(string data)
        {
            List<String> information = new List<String>();
            dynamic jsonData = JObject.Parse(data);
            string probableGender = jsonData.gender;
            string probability = jsonData.probability;

            information.Add(probableGender);
            information.Add(probability);

            return information;
        }
    }
}
