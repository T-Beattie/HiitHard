using HiitHard.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Plugin.Calendar.Models;

namespace HiitHard.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationDetail : ContentPage
    {
        SessionManager sessionManager = SessionManager.Instance();
        public NavigationDetail()
        {
            InitializeComponent();
            DateTime date = DateTime.Now;
            calendar_tracker.Day = date.Day;
            calendar_tracker.Month = date.Month;
            calendar_tracker.Year = date.Year;

            sessionManager.LoadSessions();

 

            AddSessionsToCalendar();
        }

        private void AddSessionsToCalendar()
        {
            List<Session> sessions = sessionManager.sessionStash;
            for (int i = 0; i < sessions.Count; i++)
            {
                DateTime sessionDateTime = DateTime.Parse(sessions[i]._date);
                AddEventToDay(sessionDateTime, sessions[i]._workoutName, sessions[i]._description);
            }
        }

        private void AddEventToDay(DateTime selectedDate, string name, string description)
        {
            if (calendar_tracker.Events.ContainsKey(selectedDate))
            {

                var selectedEvent = calendar_tracker.Events[selectedDate] as List<EventModel>;

                selectedEvent.Add(new EventModel { Name = name, Description = description });
            }
            else
            {
                var selectedEvents = new List<EventModel>
                {
                    new EventModel { Name = name, Description = description }
                };

                calendar_tracker.Events.Add(selectedDate, selectedEvents);        
            }
            
        }

        private IEnumerable<EventModel> GenerateEvents(int count, string name, string description)
        {
            return Enumerable.Range(1, count).Select(x => new EventModel
            {
                Name = name,
                Description = description
            });
        }
    }

    public class EventModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}