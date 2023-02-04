using HiitHard.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HiitHard.Managers
{
    class SessionManager
    {
        private static SessionManager _instance = new SessionManager();
        private string sessions_file = "sessions.json";

        public List<Session> sessionStash;

        //Static method which allows the instance creation  
        static internal SessionManager Instance()
        {
            //All you need to do it is just return the  
            //already initialized which is thread safe  
            return _instance;
        }

        public SessionManager()
        {
            sessionStash = new List<Session>();
        }

        public void LoadSessions()
        {
            string contents = DependencyService.Get<INativeFileService>().ReadJson(sessions_file);

            if (contents != "No File present")
            {
                sessionStash = JsonConvert.DeserializeObject<List<Session>>(contents);
            }
        }

        public void SaveSessions()
        {
            string stash_string = JsonConvert.SerializeObject(sessionStash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(stash_string, sessions_file);
        }

        public void AddSession(Session newSession)
        {
            sessionStash.Add(newSession);
            string my_string = JsonConvert.SerializeObject(sessionStash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(my_string, sessions_file);
        }


        public void ClearSessions()
        {
            List<Session> empty_stash = new List<Session>();
            string my_string = JsonConvert.SerializeObject(empty_stash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(my_string, sessions_file);
            sessionStash = empty_stash;
        }
    }

    public class Session
    {
        public string _date { get; set; }
        public string _workoutName { get; set; }
        public string _duration { get; set; }
        public string _description { get; set; }

        public Session(string date, string workoutName, string duration, string description)
        {
            _date = date;
            _workoutName = workoutName;
            _description = duration;
            _description = description;
        }
    }
}
