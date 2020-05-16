using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1 {
    class Program {
        //results does not need to be global, make this private/parameter
        static string[] results = new string[50];
        static Tuple<string, string> names;

        static void Main(string[] args) {
            char key;

            //Give user option to exit program, loop until exit parameters are fulfilled
            do {
                //Clean up the console window from any previous loops.
                Console.Clear();
                //Display Main Menu Options to user.
                Console.WriteLine("Press c to get categories");
                Console.WriteLine("Press r to get random jokes");
                Console.WriteLine("Press x to exit");
                key = GetEnteredKey();
                if (key == 'c') {
                    //Get category list, and display it to user
                    GetCategories();
                    PrintResults();
                } else if (key == 'r') {
                    //Generate 1-9 random jokes for the user
                    Console.WriteLine("Want to use a random name? y/n");

                    if (GetEnteredKey() == 'y') {
                        //Regenerate random name list every time
                        GetNames();
                    }
                    Console.WriteLine("Want to specify a category? y/n");
                    if (GetEnteredKey() == 'y') {
                        //Remove duplication of code
                        Console.WriteLine("How many jokes do you want? (1-9)");
                        int n = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Enter a category;");
                        GetRandomJokes(Console.ReadLine(), n);
                        PrintResults();
                    } else {
                        Console.WriteLine("How many jokes do you want? (1-9)");
                        int n = Int32.Parse(Console.ReadLine());
                        GetRandomJokes(null, n);
                        PrintResults();
                    }
                } else if (key == 'x') {
                    //Exit the program cleanly.
                    System.Environment.Exit(0);
                }
                Console.WriteLine("Press Any Key to Start Again..");
                Console.ReadKey(true);

                names = null;
            } while (key != 'x');


        }


        private static void PrintResults() {
            Console.WriteLine("[" + string.Join(",", results) + "]");
        }

        /// <summary>
        /// Waits for the user to enter a key, then converts it into the lower case value.
        /// </summary>
        /// <returns>Returns the key pressed as lowercase invariant</returns>
        private static char GetEnteredKey() {

            char key = Char.ToLowerInvariant(Console.ReadKey().KeyChar);
            //Add a newline after our entered key to make formatting look better.
            Console.WriteLine();
            return key;
        }

        private static void GetRandomJokes(string category, int number) {
            new JsonFeed("https://api.chucknorris.io", number);
            //Loop the random joke generator for the number of jokes requested.
            results = JsonFeed.GetRandomJokes(names?.Item1, names?.Item2, category);
        }

        /// <summary>
        /// Calls API for the list of categories, and sets the information into the results variable.
        /// </summary>
        private static void GetCategories() {
            new JsonFeed("https://api.chucknorris.io/jokes/categories", 0);
            results = JsonFeed.GetCategories();
        }

        private static void GetNames() {
            //Update uri to include query string ?amount= (allows up to 500) to allow each joke to have random name
            new JsonFeed("https://names.privserv.com/api/", 0);
            dynamic result = JsonFeed.Getnames();
            names = Tuple.Create(result.name.ToString(), result.surname.ToString());
        }
    }
}
