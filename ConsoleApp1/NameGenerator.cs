using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace JokeGenerator {
    class NameGenerator {
        private const string kBaseURL = "https://names.privserv.com/api/";
        HttpClient _client;

        public NameGenerator(HttpClient client) {
            _client = client;
        }

        /// <summary>
        /// Generates a List of names (First Name, and Surname) for the given number of people.
        /// </summary>
        /// <param name="amount">The number of names to generate.</param>
        /// <returns>Returns a List of First/Last names as a Tuple</returns>
        public List<Tuple<string, string>> GetNames(int amount = 1) {
            _client.BaseAddress = new Uri(kBaseURL);
            var nameList = new List<Tuple<string, string>>();

            try {
                string response = _client.GetStringAsync("?amount=" + amount).Result;
                var names = JsonConvert.DeserializeObject<dynamic>(response);
                if (amount > 1) {
                    //If getting more than a single name, it is returned as an array, so iterate through and build your name list.
                    foreach (JToken name in names) {
                        nameList.Add(Tuple.Create(name.Value<string>("name"), name.Value<string>("surname")));
                    }
                } else {
                    nameList.Add(Tuple.Create(names.name.ToString(), names.surname.ToString()));
                }
            } catch (HttpRequestException ex) {
                //TODO:Implement logging/retry depending on error?
            } catch (Exception ex) {
                //TODO:Implement logging
            }

            return nameList;
        }

    }
}
