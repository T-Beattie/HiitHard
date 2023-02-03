using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HiitHard.Objects
{
    class DBUser
    {
        CognitoAWSCredentials credentials = new CognitoAWSCredentials(
             "eu-west-1:e05c2052-782a-47ae-b859-3eaebaf68377", // Identity pool ID
               RegionEndpoint.EUWest1 // Region
             );

        private AmazonDynamoDBClient client;
        private DynamoDBContext context;

        public DBUser()
        {
            client = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUWest1);
            context = new DynamoDBContext(client);
        }

        public async Task<User> GetDBUser(string user_identifier, string password)  //Email or Spotify ID number
        {
            User retrievedUser = await context.LoadAsync<User>(user_identifier);
            if (retrievedUser == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Account Does not exist!", "OK");
            }
            else
            {
                if (password == "NA" || password != retrievedUser.password)
                {
                    if (retrievedUser.spotify_id == null)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Password Incorrect", "OK");
                        retrievedUser = null;
                    }
                    else
                    {
                        return retrievedUser;
                    }                   
                }
                else
                {
                    return retrievedUser;
                }
            }

            return null;
        }

        public async Task<bool> CreateUser(User new_user)
        {
            await context.SaveAsync(new_user);

            return true;
        }

        public async Task<bool> DoesUsernameExistAsync(string email)
        {
            User retrievedUser = await context.LoadAsync<User>(email);
            if (retrievedUser is null)
            {
                retrievedUser = null;
                return false;
            }
            else
            {
                retrievedUser = null;
                return true;
            }
        }


    }

    [DynamoDBTable("hiithard_user_profiles")]
    public class User
    {
        [DynamoDBHashKey]    // Hash key.
        public string email { get; set; }

        public string password { get; set; }
        public string display_name { get; set; }
        public string spotify_id { get; set; }
    }
}
