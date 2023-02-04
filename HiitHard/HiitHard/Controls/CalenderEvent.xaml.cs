using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HiitHard.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalenderEvent : ContentView
    {
        public static BindableProperty CalenderEventCommandProperty =
            BindableProperty.Create(nameof(CalenderEventCommand), typeof(ICommand), typeof(CalenderEvent), null);

        public CalenderEvent()
        {
            InitializeComponent();
        }

        public ICommand CalenderEventCommand
        {
            get => (ICommand)GetValue(CalenderEventCommandProperty);
            set => SetValue(CalenderEventCommandProperty, value);
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            if (BindingContext is AdvancedEventModel eventModel)
                CalenderEventCommand?.Execute(eventModel);
        }
    }

    public class AdvancedEventModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Starting { get; set; }
    }
}