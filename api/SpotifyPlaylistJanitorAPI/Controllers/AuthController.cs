using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpotifyAPI.Web;
using SpotifyPlaylistJanitorAPI.Infrastructure;
using SpotifyPlaylistJanitorAPI.Models;
using SpotifyPlaylistJanitorAPI.Models.Auth;
using SpotifyPlaylistJanitorAPI.Services.Interfaces;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpotifyPlaylistJanitorAPI.Controllers
{
    /// <summary>
    /// Controller for authorising application with Spotify API to make calls on behalf of the user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthController : Controller
    {
        private readonly ISpotifyService _spotifyService;
        private readonly SpotifyOption _spotifyOptions;
        private static string STATE = $"{Guid.NewGuid()}{Guid.NewGuid()}".Replace("-", "");

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="spotifyService">The Spotify Service.</param>
        /// <param name="spotifyOptions">The Spotify access credentials read from environment vars.</param>
        public AuthController(ISpotifyService spotifyService, IOptions<SpotifyOption> spotifyOptions)
        {
            _spotifyService = spotifyService;
            _spotifyOptions = spotifyOptions.Value;
        }

        /// <summary>
        /// Default view for AuthController.
        /// </summary>
        /// <returns>Default view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Auth view for AuthController.
        /// </summary>
        /// <returns>Auth view.</returns>
        [HttpGet()]
        public IActionResult Auth()
        {
            _spotifyService.CheckSpotifyCredentials();
            var baseUrl = Request?.GetTypedHeaders()?.Referer?.ToString() ?? "";
            var loginRequest = new LoginRequest(new Uri($"{baseUrl.TrimEnd('/')}/auth/callback"), _spotifyOptions.ClientId, LoginRequest.ResponseType.Code)
            {
                Scope = new[] {
                    Scopes.UserReadEmail,
                    Scopes.PlaylistReadPrivate,
                    Scopes.PlaylistReadCollaborative,
                    Scopes.PlaylistModifyPublic,
                    Scopes.PlaylistModifyPrivate,
                    Scopes.UserReadPlaybackState,
                    Scopes.UserReadCurrentlyPlaying,
                },
                State = STATE,
            };
            var uri = loginRequest.ToUri();
            return Redirect(uri.ToString());
        }

        /// <summary>
        /// Callback view for AuthController.
        /// </summary>
        /// <param name="code">Code returned from first part of Authorization flow, used to request Oauth token.</param>
        /// <param name="state">Additional state parameter to vaidate callback information from first part of Authorization flow.</param>
        /// <returns>Callback view.</returns>
        [HttpGet()]
        public async Task<IActionResult> Callback(string code, string state)
        {
            if (!state.Equals(STATE))
            {
                return Error();
            }

            var baseUrl = Request?.GetTypedHeaders()?.Referer?.ToString() ?? "";

            var spotifyClient = await _spotifyService.CreateClient(code, $"{baseUrl.TrimEnd('/')}/auth/callback");
            
            ////check attempted login against databse users
            //var userProfile = await spotifyClient.UserProfile.Current();
            //var userEmail = userProfile.Email;

            _spotifyService.SetClient(spotifyClient);

            return Redirect("~/");
        }

        [HttpPost("/register")]
        public IActionResult Register([FromBody] UserLoginRequest newUser)
        {
            //add username and password hash to database
            return Ok();
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] UserLoginRequest login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        [HttpGet]
        [Route("/admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi you are an {currentUser.Role}");
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_spotifyOptions.ClientSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userInfo.Username),
                new Claim(ClaimTypes.Role,userInfo.Role)
            };

            var token = new JwtSecurityToken(
                _spotifyOptions.ClientId,
                _spotifyOptions.ClientId,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserLoginRequest login)
        {
            UserModel user = null;

            //Validate the User Credentials
            //Demo Purpose, I have Passed HardCoded User Information
            if (login.Username == "paulcmccarron@gmail.com")
            {
                user = new UserModel { Username = "paulcmccarron@gmail.com", Role = "Admin" };
            }
            if (login.Username == "paulcmccarron@hotmail.com")
            {
                user = new UserModel { Username = "paulcmccarron@hotmail.com", Role = "User" };
            }
            return user;
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

        /// <summary>
        /// Error view for AuthController.
        /// </summary>
        /// <returns>Error view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}