using HiitHard.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HiitHard.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationFlyout : ContentPage
    {
        public ListView ListView;

        public NavigationFlyout()
        {
            InitializeComponent();

            BindingContext = new NavigationFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class NavigationFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<NavigationFlyoutMenuItem> MenuItems { get; set; }

            public NavigationFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<NavigationFlyoutMenuItem>(new[]
                {
                    new NavigationFlyoutMenuItem { Id = 0, Title = "Home", Img = "home.png" ,TargetType = typeof(NavigationDetail)},
                    new NavigationFlyoutMenuItem { Id = 1, Title = "Workouts", Img = "dumbbell.png" ,TargetType = typeof(MainPage)},
                    new NavigationFlyoutMenuItem { Id = 2, Title = "Playlists",  Img = "headphones.png" ,TargetType = typeof(PlaylistsPage)},
                    new NavigationFlyoutMenuItem { Id = 3, Title = "Settings",  Img = "cog.png" ,TargetType = typeof(NavigationDetail)},
                    new NavigationFlyoutMenuItem { Id = 4, Title = "Help",  Img = "help.png" ,TargetType = typeof(NavigationDetail)},
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}