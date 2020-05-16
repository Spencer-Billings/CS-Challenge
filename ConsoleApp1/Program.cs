using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        //results does not need to be global, make this private/parameter
        static string[] results = new string[50];
        static Tuple<string, string> names;
        static ConsolePrinter printer = new ConsolePrinter();

        static void Main(string[] args)
        {
            char key;

            printer.Value("Press ? to get instructions.").ToString();
            //If user doesn't enter '?' program exits. Remove this since only workflow is to get instructions
            if (Console.ReadLine() == "?")
            {
                //Give user option to exit program, loop until exit parameters are fulfilled
                while (true)
                {
                    printer.Value("Press c to get categories").ToString();
                    printer.Value("Press r to get random jokes").ToString();
                    key = GetEnteredKey();
                    if (key == 'c')
                    {
                        //Get category list, and display it to user
                        getCategories();
                        PrintResults();
                    }
                    if (key == 'r')
                    {
                        //Generate 1-9 random jokes for the user
                        printer.Value("Want to use a random name? y/n").ToString();
                        
                        if (GetEnteredKey() == 'y') {
                            //Regenerate random name list every time
                            GetNames();
                        }
                        printer.Value("Want to specify a category? y/n").ToString();
                        if (GetEnteredKey() == 'y')
                        {
                            //Remove duplication of code
                            printer.Value("How many jokes do you want? (1-9)").ToString();
                            int n = Int32.Parse(Console.ReadLine());
                            printer.Value("Enter a category;").ToString();
                            GetRandomJokes(Console.ReadLine(), n);
                            PrintResults();
                        }
                        else
                        {
                            printer.Value("How many jokes do you want? (1-9)").ToString();
                            int n = Int32.Parse(Console.ReadLine());
                            GetRandomJokes(null, n);
                            PrintResults();
                        }
                    }
                    names = null;
                }
            }

        }


        private static void PrintResults()
        {
            printer.Value("[" + string.Join(",", results) + "]").ToString();
        }

        /// <summary>
        /// Waits for the user to enter a key, then converts it into the lower case value.
        /// </summary>
        /// <returns>Returns the key pressed as lowercase invariant</returns>
        private static char GetEnteredKey()
        {
            
            char key = Char.ToLowerInvariant(Console.ReadKey().KeyChar);
            return key;
        }

        private static void GetRandomJokes(string category, int number)
        {
            new JsonFeed("https://api.chucknorris.io", number);
            //Loop the random joke generator for the number of jokes requested.
            results = JsonFeed.GetRandomJokes(names?.Item1, names?.Item2, category);
        }

        private static void getCategories()
        {
            //Correct endpoint for this is: https://api.chucknorris.io/jokes/categories
            new JsonFeed("https://api.chucknorris.io", 0);
            results = JsonFeed.GetCategories();
        }

        private static void GetNames()
        {
            //Update uri to include query string ?amount= (allows up to 500) to allow each joke to have random name
            new JsonFeed("https://names.privserv.com/api/", 0);
            dynamic result = JsonFeed.Getnames();
            names = Tuple.Create(result.name.ToString(), result.surname.ToString());
        }
    }
}
