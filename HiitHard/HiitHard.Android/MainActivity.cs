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

namespace HiitHard.Droid
{
    [Activity(Label = "HiitHard", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.SetStatusBarColor(Android.Graphics.Color.Rgb(37, 37, 37));

            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            AdvancedTimer.Forms.Plugin.Droid.AdvancedTimerImplementation.Init();
            LoadApplication(new App());

            var am = (AudioManager)this.GetSystemService(AudioService);
            var componentName = new ComponentName(PackageName, new MyMediaButtonBroadcastReceiver().ComponentName);
            am.RegisterMediaButtonEventReceiver(componentName);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool DispatchKeyEvent(Android.Views.KeyEvent e)
        {
            Console.WriteLine(e.KeyCode.ToString());
            return base.DispatchKeyEvent(e);
        }

    }

    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionMediaButton })]
    public class MyMediaButtonBroadcastReceiver
    : BroadcastReceiver
    {
        public string ComponentName { get { return Class.Name; } }

        public override void OnReceive(Context context, Intent intent)
        {
            Console.WriteLine(intent.Action);
            if (intent.Action != Intent.ActionMediaButton)
                return;

            var evt = (KeyEvent)intent.GetParcelableExtra(Intent.ExtraKeyEvent);
            if (evt == null)
                return;

            //Console.WriteLine(evt.KeyCode);

            MessagingCenter.Send(Xamarin.Forms.Application.Current, "Notification");
        }
    }
}