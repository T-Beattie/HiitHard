using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HiitHard.Objects
{
    public struct Exercise
    {
        public string Name { get; set; }               //Name of Exercise
        public int Duration { get; set; }              //Duration in seconds
        public bool countdown { get; set; }            //Enable beep countdown after 5 seconds
        public int circuitCount { get; set; }
        public int exerciseCount { get; set; }



        public Exercise(string name, int duration, bool enableCountdown)
        {
            Name = name;
            Duration = duration;
            countdown = enableCountdown;
            circuitCount = 0;
            exerciseCount = 0;
        }

    }

    public class ExerciseUI : Frame
    {
        public string Name { get; set; }               //Name of Exercise
        public int Duration { get; set; }              //Duration in seconds
        public bool countdown { get; set; }            //Enable beep countdown after 5 seconds

        public ExerciseUI(string name, int duration, bool enableCountdown)
        {
            Name = name;
            Duration = duration;
            countdown = enableCountdown;

            BackgroundColor = Color.LightGray;

            CreateUI();

        }


        void CreateUI()
        {
            var nameLabel = new Label()
            {
                Text = Name,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            var durationLabel = new Label()
            {
                Text = Duration.ToString() + " secs",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            var enableCountdownCheck = new CheckBox()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            var layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    nameLabel,
                    durationLabel,
                    enableCountdownCheck
                }
            };

            Content = layout;
        }
    }
}
