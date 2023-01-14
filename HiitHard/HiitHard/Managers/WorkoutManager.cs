using HiitHard.Objects;
using HiitHard.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HiitHard.Managers
{
    class WorkoutManager
    {
        private static WorkoutManager _instance = new WorkoutManager();
        public List<Workout> workoutStash;

        private string workout_stash_file = "workout_stash.json";

        public WorkoutManager()
        {
            workoutStash = new List<Workout>();
        }

        public void LoadStash()
        {
            string contents = DependencyService.Get<INativeFileService>().ReadJson(workout_stash_file);

            if (contents != "No File present")
            {
                workoutStash = JsonConvert.DeserializeObject<List<Workout>>(contents);
            }
        }

        public void SaveStash()
        {
            string stash_string = JsonConvert.SerializeObject(workoutStash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(stash_string, workout_stash_file);
        }

        public void AddToStash(Workout newWorkout)
        {
            workoutStash.Add(newWorkout);
            string my_string = JsonConvert.SerializeObject(workoutStash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(my_string, workout_stash_file);
        }

        public void RemoveFromStash(Workout workoutToDelete)
        {
            workoutStash.Remove(workoutToDelete);
            string my_string = JsonConvert.SerializeObject(workoutStash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(my_string, workout_stash_file);
        }

        public void ClearStash()
        {
            List<Workout> empty_stash = new List<Workout>();
            string my_string = JsonConvert.SerializeObject(empty_stash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(my_string, workout_stash_file);
            workoutStash = empty_stash;
        }

        //Static method which allows the instance creation  
        static internal WorkoutManager Instance()
        {
            //All you need to do it is just return the  
            //already initialized which is thread safe  
            return _instance;
        }
    }
}
