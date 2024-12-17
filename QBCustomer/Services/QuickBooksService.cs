using Azure.Core;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QBCustomer.Models;
using QBCustomer.Utils;
using System.Security.Claims;
using Task = System.Threading.Tasks.Task;

namespace QBCustomer.Services
{
    public class QuickBooksService
    {

        private SmartBooksContext _db;
        private readonly SbUsersService _sbUsersService;


        public static string? clientid = AppConfig.AppSettings["appSettings:clientid"];
        public static string? clientsecret = AppConfig.AppSettings["appSettings:clientsecret"];
        public static string? redirectUrl = AppConfig.AppSettings["appSettings:redirectUrl"];
        public static string? environment = AppConfig.AppSettings["appSettings:appEnvironment"];

        public static OAuth2Client auth2Client = new OAuth2Client(clientid, clientsecret, redirectUrl, environment);

        public QuickBooksService(SmartBooksContext db, SbUsersService sbUsersService)
        {
            _db = db;
            _sbUsersService = sbUsersService;
            
        }

        //reference to QuickBook authorization 

       
        public string InitiateAuth()
        {
            List<OidcScopes> scopes = new List<OidcScopes>();
            scopes.Add(OidcScopes.Accounting);
            string authorizeUrl = auth2Client.GetAuthorizationURL(scopes);
            Console.WriteLine("Authorization URL: " + authorizeUrl);
            return authorizeUrl;
        }

        public async Task SaveToken(string code, string realmId,string userId)
        {
            var sbUser= await _sbUsersService.GetUser(userId);
            var customerToken = await GetAuthToken(code, realmId);
            customerToken.User = sbUser;
            // save the new CustomerToken in the db
            _db.CustomerTokens.Add(customerToken);
            await _db.SaveChangesAsync();
        }

        //get the token
        public async Task<CustomerToken> GetAuthToken(string code, string realmId)
        {

            var tokenResponse = await auth2Client.GetBearerTokenAsync(code);

            var access_token = tokenResponse.AccessToken;
            var access_token_expires_at = tokenResponse.AccessTokenExpiresIn;

            var refresh_token = tokenResponse.RefreshToken;
            var refresh_token_expires_at = tokenResponse.RefreshTokenExpiresIn;
            try
            {

                var customerToken = new CustomerToken
                {
                    RealmId = realmId,
                    Token = access_token,
                    AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(access_token_expires_at),
                    RefreshToken = refresh_token,
                    RefreshTokenExpiresAt = DateTime.UtcNow.AddSeconds(refresh_token_expires_at),
                };
                return customerToken;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }


        public async Task RefreshAccessTokenAsync(CustomerToken customerToken)
        {
            var tokenResponse = await auth2Client.RefreshTokenAsync(customerToken.RefreshToken);
            customerToken.Token = tokenResponse.AccessToken;
            customerToken.AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.AccessTokenExpiresIn);
            customerToken.RefreshToken = tokenResponse.RefreshToken;
            customerToken.RefreshTokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.RefreshTokenExpiresIn);
            _db.SaveChanges();
        }



    }
}
