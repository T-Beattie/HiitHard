using AdvancedTimer.Forms.Plugin.Abstractions;
using HiitHard.Managers;
using HiitHard.Objects;
using HiitHard.PopupPages;
using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Extensions;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HiitHard.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunWorkoutPage : ContentPage
    {
        SpotifyManager spotifyManager = SpotifyManager.Instance();
        private Workout myWorkout;
        private Exercise currentExercise;
        private int exerciseCount;
        private int maxExercises;
        private bool isPaused = false;
        private int countdownSeconds;

        private List<Exercise> exerciseList;
        FullTrack currentTrack = new FullTrack();


        ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;

        IAdvancedTimer timer = DependencyService.Get<IAdvancedTimer>();
        public RunWorkoutPage(Workout workout)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            DeviceDisplay.KeepScreenOn = true;

            myWorkout = workout;
            exerciseList = new List<Exercise>();
                        

            exerciseList = myWorkout.GetFullExerciseList();
            maxExercises = exerciseList.Count;

            //WelcomeMessage();

            var firstExercise = exerciseList[0];
            var nextExercise = exerciseList[1];

            currentExercise = firstExercise;

            exerciseName.Text = firstExercise.Name;
            timeCounter.Text = currentExercise.Duration.ToString();
            countdownSeconds = currentExercise.Duration;
            circuit_count_label.Text = currentExercise.circuitCount.ToString();
            exercise_count_label.Text = currentExercise.exerciseCount.ToString();
            nextExerciseLabel.Text = nextExercise.Name;

            timer.initTimer(1000, timerElapsed, true);

            LoadPlaylists();

            //spotify_info.Text = track.Name;
            //spotify_info_artist.Text = track.Artists[0].Name;
            //backgroundImage.Source = track.Album.Images[0].Url;

            MessagingCenter.Subscribe<Xamarin.Forms.Application>(Xamarin.Forms.Application.Current, "Notification", (sender) =>
            {
                Console.WriteLine("Received Notification...");
                startButton_Clicked(null, null);
            });
        }

        private void timerElapsed(object sender, EventArgs e)
        {

            countdownSeconds--;
                
            Console.WriteLine("Timer Elapsed - count " + countdownSeconds);
            exercise_progress_ring.Progress = GetProgress(countdownSeconds, currentExercise.Duration);

            if (countdownSeconds < 1 && exerciseCount < maxExercises - 1)
            {
                player.Load(GetStreamFromFile("beep_2.wav"));
                player.Play();
            }

            if (countdownSeconds < 4 && countdownSeconds > 0)
            {
                player.Load(GetStreamFromFile("beep_1.wav"));
                player.Play();
            }

            if (countdownSeconds == 9 && exerciseCount < maxExercises - 1)
            {
                UpNextMessage(exerciseList[exerciseCount+1].Name);
            }


            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                timeCounter.Text = countdownSeconds.ToString();
                GetCurrentTrack();


                if (countdownSeconds < 1 && exerciseCount < maxExercises - 1)
                {
                    countdownSeconds = NextExercise();
                }
                if (countdownSeconds < 1 && exerciseCount == maxExercises - 1)
                {
                    Console.WriteLine("Stopping Timer");
                    timer.stopTimer();
                    player.Load(GetStreamFromFile("beep_2.wav"));
                    player.Play();

                    ShowCompleteMessage();
                }

            });
        }

        async void ShowCompleteMessage()
        {
            await Navigation.PushPopupAsync(new CompleteWorkoutPopupPage());
            await Task.Delay(1000);
            await TextToSpeech.SpeakAsync("Workout Completed");
        }

        async void WelcomeMessage()
        {
            await TextToSpeech.SpeakAsync("Welcome to your " + myWorkout.Name + " workout. When ready hit the start button!");
        }

        async void UpNextMessage(string exerciseName)
        {
            await TextToSpeech.SpeakAsync("Up next. " + exerciseName);
        }
        async void CurrentExerciseMessage(string exerciseName)
        {
            await TextToSpeech.SpeakAsync(exerciseName);
        }

        private void startButton_Clicked(object sender, EventArgs e)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                timer.startTimer();
                GetCurrentTrack();
            }
            else
            {
                timer.stopTimer();
                GetCurrentTrack();
            }            
        }

        int NextExercise()
        {
            exerciseCount++;

            currentExercise = exerciseList[exerciseCount];
            
            if(exerciseCount < maxExercises)
            {
                exerciseName.Text = currentExercise.Name;
                circuit_count_label.Text = currentExercise.circuitCount.ToString();
                exercise_count_label.Text = currentExercise.exerciseCount.ToString();
                if (exerciseCount < exerciseList.Count-1) { 

                    var nextExercise = exerciseList[exerciseCount + 1];
                    nextExerciseLabel.Text = exerciseList[exerciseCount + 1].Name;
                }
            }
            return currentExercise.Duration;
        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("HiitHard." + filename);
            return stream;
        }

        void LoadPlaylists()
        {

            foreach (var playlist in spotifyManager.playlists)
            {
                playlists_stack.Children.Add(CreatePlaylistButton(playlist));
            }

            GetCurrentTrack();
        }

        async Task GetCurrentTrack()
        {
            FullTrack track = await spotifyManager.GetCurrentlyPlaying();


            if (track != null )
            {
                spotify_current_song.Text = track.Name;
                spotify_current_artist.Text = track.Artists[0].Name;
                backgroundImage.Source = track.Album.Images[0].Url;
            }
            
        }

        private Frame CreatePlaylistButton(SimplePlaylist playlist)
        {
            Frame playlistFrame = new Frame();
            playlistFrame.BackgroundColor = Color.FromHex("#46BF71");
            playlistFrame.Margin = 0;
            playlistFrame.Padding = 0;
            playlistFrame.HeightRequest = 75;

            var playlistImage = new Xamarin.Forms.Image();
            playlistImage.HeightRequest = 70;
            playlistImage.WidthRequest = 70;
            if (playlist.Images.Count > 0)
                playlistImage.Source = playlist.Images[0].Url;

            AbsoluteLayout.SetLayoutBounds(playlistImage, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(playlistImage, AbsoluteLayoutFlags.All);

            var playlistName = new Label()
            {
                Text = playlist.Name,
                FontFamily = "Montserrat",
                TextColor = Color.White,
                FontSize=8,
                WidthRequest=50
            };
            playlistName.Text = playlist.Name;
            AbsoluteLayout.SetLayoutBounds(playlistName, new Rectangle(0.5, 1, 1, 1));
            AbsoluteLayout.SetLayoutFlags(playlistName, AbsoluteLayoutFlags.All);

            AbsoluteLayout stack = new AbsoluteLayout()
            {
                Children =
                {
                    playlistImage,
                    playlistName
                },
                Padding=0,
                Margin=0
            };

            playlistFrame.Content = stack;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                PlayPlaylist(playlist.Uri);
                spotify_current_playlist.Text = playlist.Name;

            };
            playlistFrame.GestureRecognizers.Add(tapGestureRecognizer);

            return playlistFrame;
        }

        private async void play_pause_Btn_Clicked(object sender, EventArgs e)
        {
            await OpenSpotifyAsync();
        }

        public async Task OpenSpotifyAsync()
        {
            await Launcher.OpenAsync("spotify:playlist:37i9dQZF1EIV02pzpKNRk8");
        }

        private async void PlayPlaylist(string uri)
        {
            await spotifyManager.GetPlaylistTracks(uri);

            await GetCurrentTrack();
        }

        private float GetProgress(int currentTime, int maximumTime)
        {
            var progress = (float)currentTime / (float)maximumTime;
            return progress;
        }

        private void startpauseButton_Clicked(object sender, EventArgs e)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                timer.startTimer();
                GetCurrentTrack();
            }
            else
            {
                timer.stopTimer();
                GetCurrentTrack();
            }
        }

        private void previousTrackButton_Clicked(object sender, EventArgs e)
        {
            spotifyManager.PlayPreviousTrack();
            GetCurrentTrack();
        }

        private void playTrackButton_Clicked(object sender, EventArgs e)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                playTrackButton.ImageSource = "pause.png";
            }
            else
            {
                playTrackButton.ImageSource = "play.png";
            }
            spotifyManager.PlayMusic();
            GetCurrentTrack();
        }

        private void nextTrackButton_Clicked(object sender, EventArgs e)
        {
            spotifyManager.PlayNextTrack();
            GetCurrentTrack();
        }
    }

}