using HiitHard.Objects;
using HiitHard.PopupPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HiitHard.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditWorkoutPage : ContentPage
    {
        private Workout myWorkout;
        public EditWorkoutPage(Workout workout)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            myWorkout = workout;

            nameLabel.Text = workout.Name;
            typeLabel.Text = workout.Type;

            for (int i = 0; i < workout.CircuitList.Count; i++)
            {
                AddCircuitUI newCircuitUI = new AddCircuitUI(workout.CircuitList[i], 0);
                circuitStack.Children.Add(newCircuitUI);
            }
        }
    }
}