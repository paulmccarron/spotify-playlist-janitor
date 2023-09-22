using SpotifyPlaylistJanitorAPI.Models.Auth;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyPlaylistJanitorAPI.SwaggerExamples.Auth
{
    /// <summary>
    /// Swagger example.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserLoginExample : IExamplesProvider<JWTModel>
    {
        /// <summary>
        /// Implementation of Swashbuckle GetExamples() IExamplesProvider method.
        /// </summary>
        /// <returns>Example <see cref="JWTModel"/>ErrorResponseModel.</returns>
        public JWTModel GetExamples()
        {
            return new JWTModel
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6InVzZXJuYW1lIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2OTUxMTk3MTksImlzcyI6Im1vY2tDbGllbnRJZCIsImF1ZCI6Im1vY2tDbGllbnRJZCJ9.Vt2V5v225XfYhI7StUzesp1k3Ua8d3sK01bzF9w4ciA",
                TokenType = "Bearer",
                ExpiresIn = (int)TimeSpan.FromHours(1).TotalMilliseconds,
                RefreshToken = "YRjxLpsjRqL7zYuKstXogqioA_P3Z4fiEuga0NCVRcDSc8cy_9msxg",
            };
        }
    }
}
