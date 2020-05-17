using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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
    }
}
