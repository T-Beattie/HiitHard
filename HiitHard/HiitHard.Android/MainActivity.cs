using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Views;
using Android.Media.Session;
using Android.Content;
using Android.Media;
using Xamarin.Forms;
using MediaManager;
using Android.Bluetooth;
using Plugin.CurrentActivity;
using System.Threading.Tasks;

using Xamarin.Essentials;

namespace HiitHard.Droid
{
    [Activity(Label = "HiitHard", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, AudioManager.IOnAudioFocusChangeListener
    {
        private Handler handler = new Handler();
        private const int DELAY = 50;
        AudioManager audioManager;

        bool music_active = false;
        private void StartListening()
        {

            handler.PostDelayed(() =>
            {               
                if (music_active != audioManager.IsMusicActive)
                {
                    // Handle music active
                    Console.WriteLine("Is Music Active: " + audioManager.IsMusicActive);
                    MessagingCenter.Send<object, bool>(this, "musicActive", audioManager.IsMusicActive);
                }

                StartListening();
            }, DELAY);

            music_active = audioManager.IsMusicActive;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Window.SetStatusBarColor(Android.Graphics.Color.Rgb(182, 146, 19));
            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            AdvancedTimer.Forms.Plugin.Droid.AdvancedTimerImplementation.Init();
            audioManager = (AudioManager)GetSystemService(Context.AudioService);

            StartListening();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnAudioFocusChange([GeneratedEnum] AudioFocus focusChange)
        {
           
        }      
    }
}