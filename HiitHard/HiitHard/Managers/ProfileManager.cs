using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using HiitHard.Objects;
using HiitHard.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HiitHard.Managers
{
    class ProfileManager
    {
        public User user = new User();

        private static ProfileManager _instance = new ProfileManager();
        private string profile_file = "user_profile.json";

        CognitoAWSCredentials credentials = new CognitoAWSCredentials(
          "eu-west-1:e05c2052-782a-47ae-b859-3eaebaf68377", // Identity pool ID
            RegionEndpoint.EUWest1 // Region
        );

        //Static method which allows the instance creation  
        static internal ProfileManager Instance()
        {
            //All you need to do it is just return the  
            //already initialized which is thread safe  
            return _instance;
        }

        public ProfileManager()
        {

        }

        public User GetLocalProfile()
        {
            string contents = DependencyService.Get<INativeFileService>().ReadJson(profile_file);
            if (contents != "No File present")
            {
                user = JsonConvert.DeserializeObject<User>(contents);
            }

            return user;
        }

        public void SetLocalProfile(User user_to_set)
        {
            string user_string = JsonConvert.SerializeObject(user_to_set, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(user_string, profile_file);
            user = user_to_set;

        }

        public void DeleteLocalProfile()
        {
            DependencyService.Get<INativeFileService>().DeleteFile(profile_file);
            user = new User();
        }

        public async Task<User> RetrieveUserAsync(string email)
        {
            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUWest1);
            DynamoDBContext context = new DynamoDBContext(client);

            User retrieved_user = await context.LoadAsync<User>(email);
            return retrieved_user;
        }

        private async Task UpdateUserAsync(User update_user)
        {
            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUWest1);
            DynamoDBContext context = new DynamoDBContext(client);

            await context.SaveAsync<User>(update_user);
        }

        public async Task UpdateMyProfile()
        {
            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUWest1);
            DynamoDBContext context = new DynamoDBContext(client);

            await context.SaveAsync<User>(user);
        }

        public async Task RefreshProfile()
        {
            var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUWest1);
            DynamoDBContext context = new DynamoDBContext(client);

            user = await context.LoadAsync<User>(user.email);
        }

    }
}
