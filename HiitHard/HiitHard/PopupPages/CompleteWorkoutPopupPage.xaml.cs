using HiitHard.Managers;
using PCLStorage;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HiitHard.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompleteWorkoutPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private string[] motivationalQuotes;
        public CompleteWorkoutPopupPage()
        {
            InitializeComponent();
            ReadMotivation();

            Tuple<string, string> quote = GetQuote();
            quoteLabel.Text = quote.Item1;
            authorLabel.Text = quote.Item2;
        }


        private void ReadMotivation()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(CompleteWorkoutPopupPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("HiitHard.MotivationalQuotes.txt");
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            motivationalQuotes = text.Split('\n'); //Split the content after a new row.
        }

        private Tuple<string, string> GetQuote()
        {
            string Quote = "";
            string Author = "";

            Random r = new Random();
            int rInt = r.Next(0, motivationalQuotes.Length-1);

            string mQuote = motivationalQuotes[rInt];

            string[] components = mQuote.Split(new string[] { "|" }, StringSplitOptions.None);

            if(components.Length > 0)
            {
                Quote = components[0];
                if(components.Length > 1)
                {
                    Author = components[1];
                }
            }

            return Tuple.Create(Quote,Author);
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        private async void endWorkoutButton_Clicked(object sender, EventArgs e)
        {
            
            var pages = Navigation.NavigationStack.ToList();
            foreach (var page in pages)
            {
                if (page.GetType() != typeof(MainPage))
                    Navigation.RemovePage(page);
            }

            await Navigation.PopPopupAsync();
        }
    }
}