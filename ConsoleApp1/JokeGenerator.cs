﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JokeGenerator
{
    class JokeGenerator
    {
        private const string kBaseURL = "https://api.chucknorris.io";
        HttpClient _client;

        public JokeGenerator(HttpClient client) {
            _client = client;
        }

        /// <summary>
        /// Gathers a list of available joke categories
        /// </summary>
        /// <returns>Returns all allowed categories as a List</returns>
        public List<string> GetCategories() {
            _client.BaseAddress = new Uri(kBaseURL);
            var categoryList = new List<string>();
            

            try {
                string response = _client.GetStringAsync("/jokes/categories").Result;
                var categories = JsonConvert.DeserializeObject<dynamic>(response);
                
                if (categories.Type == JTokenType.Array) {
                    //If getting more than a single name, it is returned as an array, so iterate through and build your name list.
                    foreach (JToken category in categories) {
                        categoryList.Add(category.ToString());
                    }
                } else {
                    categoryList.Add(categories.ToString());
                }
            } catch (HttpRequestException ex) {
                //TODO:Implement logging/retry depending on error?
            } catch (Exception ex) {
                //TODO:Implement logging
            }

            return categoryList;
        }

        /// <summary>
        /// Generates random Chuck Norris jokes given a list of names, a category and the number of jokes to generate.
        /// </summary>
        /// <param name="nameList">A list of First/Last name pairings to substitute for Chuck Norris.</param>
        /// <param name="category">An optional category of jokes.</param>
        /// <param name="numJokes">The number of jokes to read.</param>
        /// <returns></returns>
        public List<string> GetRandomJokes(List<Tuple<string,string>> nameList, string category, int numJokes = 1) {
            _client.BaseAddress = new Uri(kBaseURL);
            List<string> jokes = new List<string>();

            string url = "jokes/random";
            if (category != null) {
                if (url.Contains('?'))
                    url += "&";
                else url += "?";
                url += "category=";
                url += category;
            }

            for (int it = 0; it < numJokes; it++) {
                string firstName = nameList.FirstOrDefault().Item1;
                string lastName = nameList.FirstOrDefault().Item2;

                if(it < nameList.Count) {
                    firstName = nameList[it].Item1;
                    lastName = nameList[it].Item2;
                }

                try {
                    string joke = _client.GetStringAsync(url).Result;

                    if (firstName != null && lastName != null) {
                        joke = joke.Replace("Chuck Norris", firstName + " " + lastName);
                        
                    }
                    JObject jokeJson = JObject.Parse(joke);
                    jokes.Add(jokeJson["value"].ToString());
                } catch (HttpRequestException ex) {
                    //TODO:Implement logging/retry depending on error?
                } catch (Exception ex) {
                    //TODO:Implement logging
                }
            }


            return jokes;
        }
    }
}
