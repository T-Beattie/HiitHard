using System;
using System.Collections.Generic;
using System.Text;

namespace HiitHard.Objects
{
    public class Circuit
    {
        public int Repeat { get; set; }
        public int RestInterval { get; set; }
        public List<Exercise> exerciseList { get; set; }

        public int Duration { get; set; }

        public int ExerciseCount { get; set; }


        public Circuit()
        {
            exerciseList = new List<Exercise>();
            Repeat = 1;
            RestInterval = 10;
            Duration = 0;
        }

        public void CalculateExerciseStats()
        {
            ExerciseCount = exerciseList.Count;

            foreach (var exercise in exerciseList)
            {
                Duration += exercise.Duration + RestInterval;
            }

            Duration = Duration * Repeat - RestInterval;
        }
    }
}
