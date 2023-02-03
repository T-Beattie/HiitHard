using HiitHard.Objects;
using Plugin.Connectivity;
using Plugin.Toast;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HiitHard.Managers
{
    class SignInManager
    {
        private static SignInManager _instance = new SignInManager();
        private ProfileManager profile_manager = ProfileManager.Instance();
        SpotifyManager spotifyManager = SpotifyManager.Instance();

        private DBUser db_user = new DBUser();

        public SignInManager()
        {
            MessagingCenter.Subscribe<SpotifyManager>(this, "Authentication_Success", async (sender) =>
            {
                // Do something whenever the "Hi" message is received
                Console.WriteLine("Message Recieved!");
                await SpotifySignIn();
            });
        }

        //Static method which allows the instance creation  
        static internal SignInManager Instance()
        {
            //All you need to do it is just return the  
            //already initialized which is thread safe  
            return _instance;
        }

        public async void SignInNative(string email, string password)
        {
            //page.Navigation.PushModalAsync(new NativeSignInPage());
            if (await db_user.DoesUsernameExistAsync(email))
            {
                User user = await db_user.GetDBUser(email, password);
            }
        }

        public async Task OpenPage()
        {
            //await App.Current.MainPage.Navigation.PushModalAsync(new NavPage());
        }

        private async Task<bool> SpotifySignIn()
        {
            if (DoIHaveInternet())
            {
                PrivateUser spotify_user = spotifyManager.userProfile;

                if (await db_user.DoesUsernameExistAsync(spotify_user.Email))
                {
                    User retrieved_user = await db_user.GetDBUser(spotify_user.Email, "NA");
                    profile_manager.SetLocalProfile(retrieved_user);

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        //await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
                    });

                    return true;
                }
                else
                {
                    User new_user = new User()
                    {
                        email = spotify_user.Email,
                        password = "NA",
                        display_name = spotify_user.DisplayName,
                        spotify_id = spotify_user.Id
                    };

                    bool success = await db_user.CreateUser(new_user);
                    if (success)
                    {
                        profile_manager.SetLocalProfile(new_user);

                        Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                        {
                            //await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
                        });
                        return true;
                    }
                }
               
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error!", "No internet connection could be found", "Ok");
                return false;
            }
            return false;
        }

        public void SignInSpotify()
        {
            if (DoIHaveInternet())
            {
                spotifyManager.Authenticate();
            }
        }

        public void Register(ContentPage page)
        {
            //page.Navigation.PushModalAsync(new NativeRegisterPage());
        }

        public async Task<bool> AutoSignIn(User user)
        {
            if (user == null)
            {
                return false;
            }

            if (user.email != null && user.password != null)
            {
                User retrieved_user = await db_user.GetDBUser(user.email, user.password);

                if(retrieved_user.spotify_id != null)
                {
                    spotifyManager.Authenticate();
                }

                profile_manager.user = retrieved_user;
                return true;
            }
            return false;
        }

        public bool DoIHaveInternet()
        {
            return CrossConnectivity.Current.IsConnected;
        }
    }
}
