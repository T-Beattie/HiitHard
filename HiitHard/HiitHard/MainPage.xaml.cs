using HiitHard.Managers;
using HiitHard.Objects;
using HiitHard.PopupPages;
using Plugin.DeviceInfo;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HiitHard
{
    public partial class MainPage : ContentPage
    {
        WorkoutManager workoutManager = WorkoutManager.Instance();
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            workoutManager.LoadStash();
            Console.WriteLine("Id: " + CrossDeviceInfo.Current.Id);

            for (int i = 0; i < workoutManager.workoutStash.Count; i++)
            {
                WorkoutUI newWorkoutUI = new WorkoutUI(workoutManager.workoutStash[i], WorkoutStack);
                WorkoutStack.Children.Add(newWorkoutUI);
            }

            
        }

        private async void AddWorkout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new AddWorkoutPopUpPage(this));            
        }

        public void RefreshStack()
        {
            WorkoutStack.Children.Clear();
            for (int i = 0; i < workoutManager.workoutStash.Count; i++)
            {
                WorkoutUI newWorkoutUI = new WorkoutUI(workoutManager.workoutStash[i], WorkoutStack);
                WorkoutStack.Children.Add(newWorkoutUI);
            }
        }
    }


}
