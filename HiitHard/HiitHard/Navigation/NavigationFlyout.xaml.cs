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
                    new NavigationFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new NavigationFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new NavigationFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new NavigationFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new NavigationFlyoutMenuItem { Id = 4, Title = "Page 5" },
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