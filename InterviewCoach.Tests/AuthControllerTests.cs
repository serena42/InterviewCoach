using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using Xunit;

namespace InterviewCoach.Tests
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Login_Get_ReturnsView()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Auth/Login");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_Post_ValidUsername_RedirectsToDashboard()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "testuser")
            });
            var response = await client.PostAsync("/Auth/Login", content);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Home/Dashboard", response.Headers.Location?.ToString());
        }

        [Fact]
        public async Task Login_Post_InvalidUsername_ReturnsViewWithError()
        {
            var client = _factory.CreateClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "")
            });
            var response = await client.PostAsync("/Auth/Login", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Stays on login page
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Username is required", responseString);
        }
    }
}