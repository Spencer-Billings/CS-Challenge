using JokeGenerator;
using NUnit.Framework;
using System.Net.Http;
using Moq;
using System.Net;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Security.Cryptography;
using System.Linq;

namespace UnitTests {
    public class NameGeneratorTests {
        NameGenerator _sut;


        [Test]
        public void GetNames_RequestOneName_ReturnsOneName() {
            //Arrange
            Tuple<string, string> expected = Tuple.Create("Spencer", "Billings");

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"name\":\"{expected.Item1}\",\"surname\":\"{expected.Item2}\",\"gender\":\"male\",\"region\":\"Canada\"}}")
                    
                });

            var client = new HttpClient(handlerMock.Object);
            _sut = new NameGenerator(client);


            //Act
            var actual = _sut.GetNames(1);

            //Assert
            Assert.AreEqual(expected.Item1, actual.FirstOrDefault().Item1);
            Assert.AreEqual(expected.Item2, actual.FirstOrDefault().Item2);
        }

        [Test]
        public void GetNames_RequestMultipleNames_ReturnsMoreThanOneName() {
            //Arrange
            int expected = 3;

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{\"name\":\"Spencer\",\"surname\":\"Billings\",\"gender\":\"male\",\"region\":\"Canada\"}," +
                    "{\"name\":\"Spencer\",\"surname\":\"Billings\",\"gender\":\"male\",\"region\":\"Canada\"}," +
                    "{\"name\":\"Spencer\",\"surname\":\"Billings\",\"gender\":\"male\",\"region\":\"Canada\"}]")

                });

            var client = new HttpClient(handlerMock.Object);
            _sut = new NameGenerator(client);


            //Act
            var actual = _sut.GetNames(expected);

            //Assert
            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public void GetNames_SendBadRequest_ReturnsEmptyList() {
            //Arrange
            int expected = 0;

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("{\"error\":\"Region or language not found\"}")

                });

            var client = new HttpClient(handlerMock.Object);
            _sut = new NameGenerator(client);


            //Act
            var actual = _sut.GetNames(expected);

            //Assert
            Assert.AreEqual(expected, actual.Count);
        }
    }
}