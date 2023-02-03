using HiitHard.PopupPages;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Plugin.DeviceInfo;

namespace HiitHard.Managers
{
    class SpotifyManager : ContentPage
    {
        private static SpotifyManager _instance = new SpotifyManager();
        private SpotifyClient spotify;
        private static EmbedIOAuthServer _server;

        public List<SimplePlaylist> playlists;
        public string token;
        public string device_Id = "";

        public PrivateUser userProfile;

        public SpotifyManager()
        {
            //Authenticate();
        }

        public async void Authenticate()
        {
            // Make sure "http://localhost:5000/callback" is in your spotify application as redirect uri!
            _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);
            await _server.Start();

            _server.AuthorizationCodeReceived += OnAuthorizationCodeReceived;
            _server.ErrorReceived += OnErrorReceived;

            var request = new LoginRequest(_server.BaseUri, "afbb77ad955d43dd814ccc02aab9001f", LoginRequest.ResponseType.Code)
            {
                Scope = new List<string> { Scopes.UserReadEmail, Scopes.PlaylistReadPrivate, Scopes.UserReadCurrentlyPlaying, Scopes.UserReadPlaybackState, Scopes.UserModifyPlaybackState }
            };

            //await Browser.OpenAsync(request.ToUri(), BrowserLaunchMode.SystemPreferred);

            await Navigation.PushPopupAsync(new SpotifyAuthPopUpPage(request.ToUri()));
            //BrowserUtil.Open(request.ToUri());
        }

        private async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response)
        {
            await _server.Stop();

            var config = SpotifyClientConfig.CreateDefault();
            var tokenResponse = await new OAuthClient(config).RequestToken(
              new AuthorizationCodeTokenRequest(
                "afbb77ad955d43dd814ccc02aab9001f", "0c06177646704e5f90c00e044e3345b1", response.Code, new Uri("http://localhost:5000/callback")
              )
            );

            spotify = new SpotifyClient(tokenResponse.AccessToken);

            userProfile = await spotify.UserProfile.Current();
            Console.WriteLine($"Hello there {userProfile.DisplayName}");

            token = tokenResponse.AccessToken;
            GetMyPlaylists();
            GetMyDeviceID();
            // do calls with Spotify and save token?

            MessagingCenter.Send<SpotifyManager>(this, "Authentication_Success");

            

            await Navigation.PopPopupAsync();

        }


        public async void GetMyDeviceID()
        {
            var devices = await spotify.Player.GetAvailableDevices();

            devices.Devices.ForEach(device => Console.WriteLine(device.Id + " " + device.Name));
            Console.WriteLine("My Device Name: " + DeviceInfo.Name);
            foreach (var device in devices.Devices)
            {
                if (DeviceInfo.Name.Contains(device.Name))
                {
                    device_Id = device.Id;
                }
            }
        }

        private static async Task OnErrorReceived(object sender, string error, string state)
        {
            Console.WriteLine($"Aborting authorization, error received: {error}");
            await _server.Stop();
        }

        public async void GetMyPlaylists()
        {
            playlists = new List<SimplePlaylist>();
            spotify = new SpotifyClient(token);

            await foreach (var playlist in spotify.Paginate(await spotify.Playlists.CurrentUsers()))
            {
                Console.WriteLine(playlist.Name);

                playlists.Add(playlist);
            }
        }

        public async Task<List<string>> GetPlaylistTracks(string uri)
        {
            var uris = new List<string>();
            spotify = new SpotifyClient(token);
            GetMyDeviceID();

            var res = await spotify.Playlists.GetItems(uri.Replace("spotify:playlist:", ""));

            foreach (var item in res.Items)
            {
                IPlayableItem s = item.Track;
                if (s is FullTrack track1)
                {
                    uris.Add(track1.Uri);
                    Console.WriteLine(track1.Uri);
                }                    
            }
            Console.WriteLine("Device ID: " + device_Id);
            if (device_Id != "")
            {
                try
                {
                    await spotify.Player.ResumePlayback(new PlayerResumePlaybackRequest() { DeviceId = device_Id, Uris = uris, OffsetParam = null, PositionMs = 0, });
                }
                catch (SpotifyAPI.Web.APIException)
                {
                    Console.WriteLine("Open Spotify");
                }
            }                
            else
            {
                Console.WriteLine("URI: " + uri);
                await Launcher.OpenAsync(uri);
            }

            return uris;
        }

        public async Task<FullTrack> GetCurrentlyPlaying()
        {
            try
            {
                spotify = new SpotifyClient(token);


                var current = new PlayerCurrentlyPlayingRequest(PlayerCurrentlyPlayingRequest.AdditionalTypes.Track);
                var currentlyplaying = await spotify.Player.GetCurrentlyPlaying(current);

                if (currentlyplaying.IsPlaying)
                {
                    IPlayableItem s = currentlyplaying.Item;
                    if (s is FullTrack track1)
                    {
                        //Console.WriteLine(track1.Name);
                        //Console.WriteLine(track1.Artists[0].Name);
                        //Console.WriteLine(track1.Album.Images[0].Url);

                        return track1;
                    }
                }
            }
            catch (SpotifyAPI.Web.APIException)
            {
                return new FullTrack();
            } 
            

            return new FullTrack();
        }

        public async void PlayMusic()
        {
            var current = new PlayerCurrentlyPlayingRequest(PlayerCurrentlyPlayingRequest.AdditionalTypes.Track);
            var currentlyplaying = await spotify.Player.GetCurrentlyPlaying(current);

            if (currentlyplaying.IsPlaying)
            {
                await spotify.Player.PausePlayback();
            }
            else
            {
                await spotify.Player.ResumePlayback();
            }
        }

        public async void PlayNextTrack()
        {
            try
            {
                await spotify.Player.SkipNext();
            }
            catch(SpotifyAPI.Web.APIException)
            {
                
            }
           
        }

        public async void PlayPreviousTrack()
        {
            try
            {
                await spotify.Player.SkipPrevious();
            }
            catch (SpotifyAPI.Web.APIException)
            {

            }

        }


        //Static method which allows the instance creation  
        static internal SpotifyManager Instance()
        {
            //All you need to do it is just return the  
            //already initialized which is thread safe  
            return _instance;
        }
    }
}
