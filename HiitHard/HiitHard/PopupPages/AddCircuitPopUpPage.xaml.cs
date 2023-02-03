using HiitHard.Objects;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HiitHard.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCircuitPopUpPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private AddWorkoutPopUpPage _addWorkoutPage;
        public AddCircuitPopUpPage(AddWorkoutPopUpPage addWorkoutPage)
        {
            InitializeComponent();

            _addWorkoutPage = addWorkoutPage;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
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

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }

        private void AddExercise_Clicked(object sender, EventArgs e)
        {
            AddExerciseUI newExercise = new AddExerciseUI();

            int count = AddExerciseStack.Children.Count;

            AddExerciseStack.Children.Insert(count, newExercise);
            newExercise.nameEntry.Focus();
        }

        private async void addCircuitButton_Clicked(object sender, EventArgs e)
        {
            Circuit newCircuit = new Circuit
            {
                Repeat = Convert.ToInt32(repeatEntry.Text),
                RestInterval = Convert.ToInt32(restIntervalEntry.Text)
            };

            for (int i = 1; i < AddExerciseStack.Children.Count; i++)
            {               
                var test = (AddExerciseUI)AddExerciseStack.Children[i];

                newCircuit.exerciseList.Add(test.GetExercise());

            }

            newCircuit.CalculateExerciseStats();

            _addWorkoutPage.AddNewCircuit(newCircuit);
            await Navigation.PopPopupAsync();
        }

        private void restIntervalEntry_Completed(object sender, EventArgs e)
        {
            repeatEntry.Focus();
        }

        private void repeatEntry_Completed(object sender, EventArgs e)
        {

        }
    }


    class AddExerciseUI : Frame
    {

        public Entry nameEntry;
        public Entry durationEntry;

        public AddExerciseUI()
        {
            CreateUI();

            nameEntry.Completed += NameEntry_Completed;
        }

        private void NameEntry_Completed(object sender, EventArgs e)
        {
            durationEntry.Focus();
        }

        public Exercise GetExercise()
        {
            int duration = Convert.ToInt32(durationEntry.Text);
            return new Exercise(nameEntry.Text, duration, true);
        }

        void CreateUI()
        {
            BackgroundColor = Color.DarkSlateGray;
            nameEntry = new Entry()
            {
                Placeholder = "Enter Exercise Name",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Keyboard = Keyboard.Text
            };

            durationEntry = new Entry()
            {
                Placeholder = "Duration",
                Keyboard = Keyboard.Numeric,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            var enableCountdownCheck = new CheckBox()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            var fiveSecondButton = new Button()
            {
                Text = "+5",
                WidthRequest = 40,
                HeightRequest = 30,
                FontSize = 8
            };
            fiveSecondButton.Clicked += FiveSecondButton_Clicked;

            var tenSecondButton = new Button()
            {
                Text = "+10",
                WidthRequest = 40,
                HeightRequest = 30,
                FontSize = 8
            };
            tenSecondButton.Clicked += TenSecondButton_Clicked; ;

            var thritySecondButton = new Button()
            {
                Text = "+30",
                WidthRequest = 40,
                HeightRequest = 30,
                FontSize = 8
            };
            thritySecondButton.Clicked += ThritySecondButton_Clicked;

            var sixtySecondButton = new Button()
            {
                Text = "+60",
                WidthRequest = 40,
                HeightRequest = 30,
                FontSize = 8
            };
            sixtySecondButton.Clicked += SixtySecondButton_Clicked;

            var timeStack = new StackLayout() 
            { 
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    fiveSecondButton,
                    tenSecondButton,
                    thritySecondButton,
                    sixtySecondButton
                }
            };


            var durationStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children = 
                {
                    durationEntry,
                    timeStack
                }
            };

            var layout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    nameEntry,
                    durationStack
                }
            };

            Content = layout;
        }

        private void SixtySecondButton_Clicked(object sender, EventArgs e)
        {
            int currentDuration = 0;
            if (durationEntry.Text != null)
            {
                currentDuration = Convert.ToInt32(durationEntry.Text);
            }

            int newDuration = currentDuration + 60;

            durationEntry.Text = newDuration.ToString();
        }

        private void ThritySecondButton_Clicked(object sender, EventArgs e)
        {
            int currentDuration = 0;
            if (durationEntry.Text != null)
            {
                currentDuration = Convert.ToInt32(durationEntry.Text);
            }

            int newDuration = currentDuration + 30;

            durationEntry.Text = newDuration.ToString();
        }

        private void TenSecondButton_Clicked(object sender, EventArgs e)
        {
            int currentDuration = 0;
            if (durationEntry.Text != null)
            {
                currentDuration = Convert.ToInt32(durationEntry.Text);
            }

            int newDuration = currentDuration + 10;

            durationEntry.Text = newDuration.ToString();
        }

        private void FiveSecondButton_Clicked(object sender, EventArgs e)
        {
            int currentDuration = 0;
            if (durationEntry.Text != null)
            {
                currentDuration = Convert.ToInt32(durationEntry.Text);               
            }

            int newDuration = currentDuration + 5;

            durationEntry.Text = newDuration.ToString();
        }
    }
}