using HiitHard.Objects;
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
        public EditWorkoutPage(Workout workout)
        {
            InitializeComponent();

            nameLabel.Text = workout.Name;
            typeLabel.Text = workout.Type;

            for (int i = 0; i < 5; i++)
            {
                Exercise newExercise = new Exercise("test", 30, true);
                //exerciseStack.Children.Add(newExercise);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}