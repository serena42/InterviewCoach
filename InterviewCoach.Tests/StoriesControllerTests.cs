using System.Net;
using InterviewCoach.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterviewCoach.Tests
{
    public class StoriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public StoriesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private async Task<(HttpClient client, string username)> LoginUserAsync(string username)
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            var content = new FormUrlEncodedContent([new KeyValuePair<string, string>("username", username)]);
            await client.PostAsync("/Auth/Login", content);
            return (client, username);
        }

        [Fact]
        public async Task Index_ShowsPlaceholderForUserWithoutStory()
        {
            // Arrange
            var (client, _) = await LoginUserAsync("newuser");

            // Act
            var response = await client.GetAsync("/Stories");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Verify "Start Story" button exists (placeholder logic)
            Assert.Contains("Start Story", content);
        }

        [Fact]
        public async Task Index_ShowsExistingStoryForUser()
        {
            // Arrange
            var (client, username) = await LoginUserAsync("existinguser");
            
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InterviewCoachContext>();
                var user = await db.Users.FirstAsync(u => u.Username == username);
                db.Stories.Add(new Story 
                { 
                    UserId = user.Id, 
                    QuestionId = 1, 
                    Situation = "My real story", 
                    AuthorName = username,
                    CreatedAt = DateTime.UtcNow 
                });
                await db.SaveChangesAsync();
            }

            // Act
            var response = await client.GetAsync("/Stories");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains("Edit Story", content);
            Assert.Contains("My real story", content);
        }

        [Fact]
        public async Task Edit_WithQuestionId_CreatesNewStoryIfDoesNotExist()
        {
            // Arrange
            var (client, username) = await LoginUserAsync("testuser_edit");

            // Act: Visit Edit page for Question ID 1
            var response = await client.GetAsync("/Stories/Edit?questionId=1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Verify database contains the record now
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<InterviewCoachContext>();
            var story = await db.Stories.FirstOrDefaultAsync(s => s.QuestionId == 1 && s.AuthorName == username);
            Assert.NotNull(story);
        }

        [Fact]
        public async Task Details_WithQuestionId_CreatesNewStoryIfDoesNotExist()
        {
            // Arrange
            var (client, username) = await LoginUserAsync("testuser_details");

            // Act: Visit Details page for Question ID 2
            var response = await client.GetAsync("/Stories/Details?questionId=2");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Verify database contains the record
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<InterviewCoachContext>();
            var story = await db.Stories.FirstOrDefaultAsync(s => s.QuestionId == 2 && s.AuthorName == username);
            Assert.NotNull(story);
        }

        [Fact]
        public async Task Edit_Post_UpdatesExistingStory()
        {
            // Arrange
            var (client, username) = await LoginUserAsync("updateuser");
            int storyId;
            int userId;

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InterviewCoachContext>();
                var user = await db.Users.FirstAsync(u => u.Username == username);
                userId = user.Id;
                var story = new Story 
                { 
                    UserId = userId, 
                    Situation = "Old Situation", 
                    AuthorName = username,
                    CreatedAt = DateTime.UtcNow 
                };
                db.Stories.Add(story);
                await db.SaveChangesAsync();
                storyId = story.Id;
            }

            var postContent = new FormUrlEncodedContent([
                new KeyValuePair<string, string>("Id", storyId.ToString()),
                new KeyValuePair<string, string>("UserId", userId.ToString()),
                new KeyValuePair<string, string>("Situation", "Updated Situation"),
                new KeyValuePair<string, string>("Task", "New Task"),
                new KeyValuePair<string, string>("Action", "New Action"),
                new KeyValuePair<string, string>("Result", "New Result"),
                new KeyValuePair<string, string>("AuthorName", username)
            ]);

            // Act
            var response = await client.PostAsync($"/Stories/Edit/{storyId}", postContent);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InterviewCoachContext>();
                var updatedStory = await db.Stories.FindAsync(storyId);
                Assert.Equal("Updated Situation", updatedStory?.Situation);
                Assert.Equal("New Task", updatedStory?.Task);
            }
        }
    }
}