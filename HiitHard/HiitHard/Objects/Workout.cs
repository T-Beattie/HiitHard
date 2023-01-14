using HiitHard.Effects;
using HiitHard.Managers;
using HiitHard.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HiitHard.Objects
{
    public class Workout
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Circuit> CircuitList { get; set; }

        public int CircuitRest { get; set; }

        public int ExerciseCount;
        public bool EnableWarmUp { get; set; }

        public Workout(string name, string type, int circuitRest)
        {
            Name = name;
            Type = type;
            CircuitRest = circuitRest;
            CircuitList = new List<Circuit>();
        }

        public int GetExerciseCount()
        {
            int count = 0;
            foreach (var circuit in CircuitList)
            {
                count += circuit.ExerciseCount;
                count = count * circuit.Repeat;
            }

            return count;
        }



        public void AddCircuit(Circuit circuitToAdd)
        {
            CircuitList.Add(circuitToAdd);
        }

        public void RemoveExercise(Circuit circuitToRemove)
        {
            CircuitList.Remove(circuitToRemove);
        }

        public List<Exercise> GetFullExerciseList()
        {
            List<Exercise> exerciseList = new List<Exercise>();

            if (EnableWarmUp)
            {
                var warmupExercise = new Exercise("Warm Up", 60, true);
                warmupExercise.circuitCount = 0;
                warmupExercise.exerciseCount = 0;
                exerciseList.Add(warmupExercise);
            }

            foreach (var circuit in CircuitList)
            {
                for (int i = 0; i < circuit.Repeat; i++)
                {
                    for (int j = 0; j < circuit.exerciseList.Count; j++)
                    {
                        if (j < circuit.exerciseList.Count - 1)
                        {
                            var exercise = circuit.exerciseList[j];
                            exercise.circuitCount = i+1;
                            exercise.exerciseCount = j+1;
                            exerciseList.Add(exercise);

                            var restExercise = new Exercise("Rest", circuit.RestInterval, true);
                            restExercise.circuitCount = i+1;
                            restExercise.exerciseCount = j+1;
                            exerciseList.Add(restExercise);
                        }
                        else
                        {
                            var exercise = circuit.exerciseList[j];
                            exercise.circuitCount = i+1;
                            exercise.exerciseCount = j+1;
                            exerciseList.Add(exercise);
                        }
                    }
                    if (i < circuit.Repeat - 1)
                    {
                        var exercise = new Exercise("Interval", CircuitRest, true);
                        exercise.circuitCount = i+1;
                        exercise.exerciseCount = 0;
                        exerciseList.Add(exercise);
                    }

                }

                if (circuit != CircuitList[CircuitList.Count - 1])
                {
                    var exercise = new Exercise("Interval", CircuitRest, true);
                    exercise.circuitCount = 0;
                    exercise.exerciseCount = 0;
                    exerciseList.Add(exercise);
                }
            }

            return exerciseList;
        }

       
    }

    public class WorkoutUI : Frame
    {
        public string Name { get; set; }
        public string Type { get; set; }
        List<Circuit> circuitList;
        int circuitCount = 0;

        private Workout myWorkout;
        private StackLayout _workoutStack;
        WorkoutManager workoutManager = WorkoutManager.Instance();

        private CheckBox warmupCheckbox;

        public WorkoutUI(Workout workout, StackLayout workoutStack)
        {
            myWorkout = workout;
            Name = workout.Name;
            Type = workout.Type;
            circuitList = workout.CircuitList;

            foreach (var circuit in circuitList)
            {
                circuitCount = circuitCount + (1 * circuit.Repeat);
            }

            _workoutStack = workoutStack;

            BackgroundColor = Color.FromHex("#1B1B1B");
            Margin = new Thickness(0);

            CreateUI();

            /*var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                Navigation.PushAsync(new EditWorkoutPage(myWorkout));
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);*/

        }

        string WorkoutTimeTotal()
        {
            int totalTimeSeconds = 0;

            List<Exercise> fullExercises = myWorkout.GetFullExerciseList();
            for (int i = 0; i < fullExercises.Count; i++)
            {
                totalTimeSeconds += fullExercises[i].Duration;
            }

            TimeSpan t = TimeSpan.FromSeconds(totalTimeSeconds);

            string totalTime = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);

            return totalTime;
        }

        void CreateUI()
        {
            AbsoluteLayout absoluteLayout = new AbsoluteLayout
            {
                Margin = new Thickness(0),
                HeightRequest=120
            };

            var frame = new Frame()
            {
                BackgroundColor = Color.FromHex("#252525"),
            };
            AbsoluteLayout.SetLayoutBounds(frame, new Rectangle(0, 0, 1, 0.75));
            AbsoluteLayout.SetLayoutFlags(frame, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(frame);

            var circleFrame = new Frame()
            {
                CornerRadius = 100,
                BackgroundColor = Color.FromHex("#252525"),
                Margin = 0
            };
            AbsoluteLayout.SetLayoutBounds(circleFrame, new Rectangle(0.5, 1, 0.22, 0.6));
            AbsoluteLayout.SetLayoutFlags(circleFrame, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(circleFrame);

            var circle2Frame = new Frame()
            {
                CornerRadius = 100,
                BackgroundColor = Color.FromHex("#B69213"),
                Margin = 0
            };
            AbsoluteLayout.SetLayoutBounds(circle2Frame, new Rectangle(0.5, 0.9, 0.18, 0.5));
            AbsoluteLayout.SetLayoutFlags(circle2Frame, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(circle2Frame);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += RunWorkoutButton_Clicked;
            circle2Frame.GestureRecognizers.Add(tapGestureRecognizer);

            warmupCheckbox = new CheckBox()
            {
                Color = Color.White,
                IsChecked = true
            };
            AbsoluteLayout.SetLayoutBounds(warmupCheckbox, new Rectangle(0.015, 0.37, 0.15, 0.15));
            AbsoluteLayout.SetLayoutFlags(warmupCheckbox, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(warmupCheckbox);

            var warmupLabel = new Label()
            {
                Text = "Warm Up",
                FontSize = 12,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                FontFamily = "Montserrat"
            };
            AbsoluteLayout.SetLayoutBounds(warmupLabel, new Rectangle(0.17, 0.37, 0.4, 0.15));
            AbsoluteLayout.SetLayoutFlags(warmupLabel, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(warmupLabel);

            var runWorkoutButton = new Button()
            {
                ImageSource = "play.png",
                BackgroundColor = Color.Transparent,
                Scale = 0.5,
                Margin = 0
            };
            runWorkoutButton.Clicked += RunWorkoutButton_Clicked;
            AbsoluteLayout.SetLayoutBounds(runWorkoutButton, new Rectangle(0.51, 0.9, 0.18, 0.5));
            AbsoluteLayout.SetLayoutFlags(runWorkoutButton, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(runWorkoutButton);

            var nameLabel = new Label()
            {
                Text = Name,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                FontFamily = "Montserrat"
            };
            AbsoluteLayout.SetLayoutBounds(nameLabel, new Rectangle(0.5, 0, 1, 0.25));
            AbsoluteLayout.SetLayoutFlags(nameLabel, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(nameLabel);

            var nameBox = new BoxView()
            {
                HeightRequest = 2,
                Color = Color.FromHex("#B69213"),
                WidthRequest = nameLabel.Text.Length * 15,
                HorizontalOptions = LayoutOptions.Center,
            };
            AbsoluteLayout.SetLayoutBounds(nameBox, new Rectangle(0.5, 0.2, 1, 0.017));
            AbsoluteLayout.SetLayoutFlags(nameBox, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(nameBox);

            var typeLabel = new Label()
            {
                Text = Type,
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                FontFamily = "Montserrat"
            };
            AbsoluteLayout.SetLayoutBounds(typeLabel, new Rectangle(0.5, 0.21, 1, 0.25));
            AbsoluteLayout.SetLayoutFlags(typeLabel, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(typeLabel);


            var deleteButton = new Button()
            {
                Text = "X",
                BackgroundColor = Color.Transparent,
                TextColor = Color.White,
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontFamily = "Montserrat"
            };
            deleteButton.Clicked += DeleteButton_Clicked;
            AbsoluteLayout.SetLayoutBounds(deleteButton, new Rectangle(1.1, 0, 0.3, 0.3));
            AbsoluteLayout.SetLayoutFlags(deleteButton, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(deleteButton);

            var durationImage = new Image()
            {
                Source = "time.png"
            };
            AbsoluteLayout.SetLayoutBounds(durationImage, new Rectangle(-0.015, 0.64, 0.15, 0.15));
            AbsoluteLayout.SetLayoutFlags(durationImage, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(durationImage);


            var durationLabel = new Label()
            {
                Text = WorkoutTimeTotal(),
                FontSize = 12,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                FontFamily = "Montserrat"
            };
            AbsoluteLayout.SetLayoutBounds(durationLabel, new Rectangle(0.2, 0.66, 0.5, 0.3));
            AbsoluteLayout.SetLayoutFlags(durationLabel, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(durationLabel);

            var circuitImage = new Image()
            {
                Source = "Circuit.png"
            };
            AbsoluteLayout.SetLayoutBounds(circuitImage, new Rectangle(0.7, 0.64, 0.15, 0.15));
            AbsoluteLayout.SetLayoutFlags(circuitImage, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(circuitImage);

            var numberCircuitsLabel = new Label()
            {
                Text = circuitCount.ToString(),
                FontSize = 12,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                FontFamily = "Montserrat"
            };
            AbsoluteLayout.SetLayoutBounds(numberCircuitsLabel, new Rectangle(1.42, 0.66, 0.5, 0.3));
            AbsoluteLayout.SetLayoutFlags(numberCircuitsLabel, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(numberCircuitsLabel);

            var exerciseImage = new Image()
            {
                Source = "dumbbell.png"
            };
            AbsoluteLayout.SetLayoutBounds(exerciseImage, new Rectangle(0.875, 0.64, 0.15, 0.15));
            AbsoluteLayout.SetLayoutFlags(exerciseImage, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(exerciseImage);

            var numberExercisesLabel = new Label()
            {
                Text = myWorkout.GetExerciseCount().ToString(),
                FontSize = 12,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                FontFamily = "Montserrat"
            };
            AbsoluteLayout.SetLayoutBounds(numberExercisesLabel, new Rectangle(1.72, 0.66, 0.5, 0.3));
            AbsoluteLayout.SetLayoutFlags(numberExercisesLabel, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(numberExercisesLabel);

            Content = absoluteLayout;
        }        

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            _workoutStack.Children.Remove(this);
            workoutManager.RemoveFromStash(myWorkout);
        }

        private async void RunWorkoutButton_Clicked(object sender, EventArgs e)
        {
            myWorkout.EnableWarmUp = warmupCheckbox.IsChecked;
            await Navigation.PushAsync(new RunWorkoutPage(myWorkout));
        }

    }

}
