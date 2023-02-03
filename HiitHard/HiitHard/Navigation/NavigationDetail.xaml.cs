using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HiitHard.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationDetail : ContentPage
    {
        public NavigationDetail()
        {
            InitializeComponent();
            DateTime date = DateTime.Now;
            calendar_tracker.Day = date.Day;
            calendar_tracker.Month = date.Month;
            calendar_tracker.Year = date.Year;
        }
    }
}