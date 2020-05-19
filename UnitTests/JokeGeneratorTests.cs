using JokeGenerator;
using NUnit.Framework;
using System.Net.Http;
using Moq;
using System.Net;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests {
    public class JokeGeneratorTests {
        JokeGenerator.JokeGenerator _sut;


        [Test]
        public void GetCategories_CallMethod_ReturnsAtLeastOneCategory() {
            //Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[\"animal\",\"career\",\"celebrity\",\"dev\","+
                    "\"explicit\",\"fashion\",\"food\",\"history\",\"money\",\"movie\",\"music\","+
                    "\"political\",\"religion\",\"science\",\"sport\",\"travel\"]")
                    
                });

            var client = new HttpClient(handlerMock.Object);
            _sut = new JokeGenerator.JokeGenerator(client);


            //Act
            var actual = _sut.GetCategories();

            //Assert
            Assert.Greater(actual.Count, 0);
        }

        [Test]
        public void GetCategories_BadRequest_ReturnsEmptyList() {
            //Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.NotFound,

                });

            var client = new HttpClient(handlerMock.Object);
            _sut = new JokeGenerator.JokeGenerator(client);


            //Act
            var actual = _sut.GetCategories();

            //Assert
            Assert.AreEqual(0,actual.Count);
        }


        [Test]
        public void GetRandomJokes_CallMethod_ReturnsJoke() {
            //Arrange
            var name = new List<Tuple<string, string>>();
            string category = null;
            int numJokes = 1;

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"categories\":[],\"created_at\":\"2020-01-05 13:42:26.447675\","+
                    "\"icon_url\":\"https://assets.chucknorris.host/img/avatar/chuck-norris.png\",\"id\":\"fivi0z5lt8gp_6vt3cc8pw\"," +
                    "\"updated_at\":\"2020-01-05 13:42:26.447675\",\"url\":\"https://api.chucknorris.io/jokes/fivi0z5lt8gp_6vt3cc8pw\"," +
                    "\"value\":\"If Chuck Norris writes code with bugs, the bugs fix themselves.\"}")

                });

            var client = new HttpClient(handlerMock.Object);
            _sut = new JokeGenerator.JokeGenerator(client);


            //Act
            var actual = _sut.GetRandomJokes(name,category, numJokes);

            //Assert
            Assert.AreEqual(numJokes, actual.Count);
        }

        [Test]
        public void GetRandomJokes_CallWithNameMethod_ReturnsJokeWithReplacedName() {
            //Arrange
            var name = new List<Tuple<string, string>>() { Tuple.Create("Spencer", "Billings") };
            string category = null;
            int numJokes = 1;

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"categories\":[],\"created_at\":\"2020-01-05 13:42:26.447675\"," +
                    "\"icon_url\":\"https://assets.chucknorris.host/img/avatar/chuck-norris.png\",\"id\":\"fivi0z5lt8gp_6vt3cc8pw\"," +
                    "\"updated_at\":\"2020-01-05 13:42:26.447675\",\"url\":\"https://api.chucknorris.io/jokes/fivi0z5lt8gp_6vt3cc8pw\"," +
                    "\"value\":\"If Chuck Norris writes code with bugs, the bugs fix themselves.\"}")

                });

            var client = new HttpClient(handlerMock.Object);
            _sut = new JokeGenerator.JokeGenerator(client);


            //Act
            var actual = _sut.GetRandomJokes(name, category, numJokes);

            //Assert
            StringAssert.Contains($"{name.First().Item1} {name.First().Item2}", actual.FirstOrDefault());
        }

        [Test]
        public void GetRandomJokes_CallMethod_ReturnsMultipleJokes() {
            //Arrange
            var name = new List<Tuple<string, string>>();
            string category = null;
            int numJokes = 3;

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"categories\":[],\"created_at\":\"2020-01-05 13:42:26.447675\"," +
                    "\"icon_url\":\"https://assets.chucknorris.host/img/avatar/chuck-norris.png\",\"id\":\"fivi0z5lt8gp_6vt3cc8pw\"," +
                    "\"updated_at\":\"2020-01-05 13:42:26.447675\",\"url\":\"https://api.chucknorris.io/jokes/fivi0z5lt8gp_6vt3cc8pw\"," +
                    "\"value\":\"If Chuck Norris writes code with bugs, the bugs fix themselves.\"}")

                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"categories\":[],\"created_at\":\"2020-01-05 13:42:26.447675\"," +
                    "\"icon_url\":\"https://assets.chucknorris.host/img/avatar/chuck-norris.png\",\"id\":\"fivi0z5lt8gp_6vt3cc8pw\"," +
                    "\"updated_at\":\"2020-01-05 13:42:26.447675\",\"url\":\"https://api.chucknorris.io/jokes/fivi0z5lt8gp_6vt3cc8pw\"," +
                    "\"value\":\"If Chuck Norris writes code with bugs, the bugs fix themselves.\"}")

                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"categories\":[],\"created_at\":\"2020-01-05 13:42:26.447675\"," +
                    "\"icon_url\":\"https://assets.chucknorris.host/img/avatar/chuck-norris.png\",\"id\":\"fivi0z5lt8gp_6vt3cc8pw\"," +
                    "\"updated_at\":\"2020-01-05 13:42:26.447675\",\"url\":\"https://api.chucknorris.io/jokes/fivi0z5lt8gp_6vt3cc8pw\"," +
                    "\"value\":\"If Chuck Norris writes code with bugs, the bugs fix themselves.\"}")

                })
                ;

            var client = new HttpClient(handlerMock.Object);
            _sut = new JokeGenerator.JokeGenerator(client);


            //Act
            var actual = _sut.GetRandomJokes(name, category, numJokes);

            //Assert
            Assert.AreEqual(numJokes, actual.Count);
        }

        [Test]
        public void GetRandomJokes_BadRequest_ReturnsEmptyList() {
            //Arrange
            var name = new List<Tuple<string, string>>();
            string category = null;
            int numJokes = 1;
            int expected = 0;

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.BadRequest,

                });

            var client = new HttpClient(handlerMock.Object);
            _sut = new JokeGenerator.JokeGenerator(client);


            //Act
            var actual = _sut.GetRandomJokes(name, category, numJokes);

            //Assert
            Assert.AreEqual(expected, actual.Count);
        }
    }
}