using HiitHard.Managers;
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
    public partial class AddWorkoutPopUpPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        WorkoutManager workoutManager = WorkoutManager.Instance();

        MainPage _mainPage;
        public AddWorkoutPopUpPage(MainPage mainPage)
        {
            InitializeComponent();
            _mainPage = mainPage;
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

        private async void addWorkoutButton_Clicked(object sender, EventArgs e)
        {
            Workout newWorkout = new Workout(workoutName.Text, workoutType.Text, Convert.ToInt32(circuitRestInterval.Text));

            for (int i = 1; i < AddCircuitStack.Children.Count; i++)
            {
                if (AddCircuitStack.Children[i] != null)
                {
                    var newCircuit = (AddCircuitUI)AddCircuitStack.Children[i];

                    newWorkout.AddCircuit(newCircuit.myCircuit);
                }
               
            }

            workoutManager.AddToStash(newWorkout);

            _mainPage.RefreshStack();

            await Navigation.PopPopupAsync();

        }

        public void AddNewCircuit(Circuit newCircuit)
        {
            AddCircuitUI newCircuitUI = new AddCircuitUI(newCircuit, 0);
            AddCircuitStack.Children.Add(newCircuitUI);
        }

        private async void AddCircuit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new AddCircuitPopUpPage(this));
        }
    }


    class AddCircuitUI : Frame
    {
        private int circuitNumCounter;
        public Circuit myCircuit;
        public AddCircuitUI(Circuit circuit ,int circuitNumberCounter)
        {
            circuitNumCounter = circuitNumberCounter;
            myCircuit = circuit;
            CreateUI();
        }

        void CreateUI()
        {
            BackgroundColor = Color.DarkSlateGray;

            var circuitNumberLabel = new Label()
            {
                Text = "#" + circuitNumCounter.ToString(),
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            var restIntervalLabel = new Label()
            {
                Text = "Rest Interval: " + myCircuit.RestInterval.ToString(),
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Start
            };

            var repeatLabel = new Label()
            {
                Text = "Repeat x" + myCircuit.Repeat.ToString(),
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.End
            };

            var exerciseLabel = new Label()
            {
                Text = "Exercises",
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            var exerciseListLabel = new Label()
            {
                Text = GetExerciseOrder(),
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            var optionsLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    restIntervalLabel,
                    repeatLabel
                }
            };

            var scroll = new ScrollView()
            {
                Content = exerciseListLabel,
                Orientation = ScrollOrientation.Horizontal
            };

            var mainLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    circuitNumberLabel,
                    optionsLayout,
                    exerciseLabel,
                    scroll
                }
            };

            Content = mainLayout;
        }

        string GetExerciseOrder()
        {
            string exerciseOrder = "";

            for (int i = 0; i < myCircuit.exerciseList.Count; i++)
            {
                if (i < myCircuit.ExerciseCount - 1)
                {
                    exerciseOrder += myCircuit.exerciseList[i].Name + " > ";
                }
                else
                {
                    exerciseOrder += myCircuit.exerciseList[i].Name;
                }
            }

            return exerciseOrder;
        }
    }

  }