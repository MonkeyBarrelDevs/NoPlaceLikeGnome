using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeHandler : MonoBehaviour
{

    [SerializeField]
    private int realMinuteDayLength;

    public static UnityAction<DateTime> OnTimeChanged;
    // Start is called before the first frame update



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public struct DateTime {
        #region Serialized Time Controls
        // Morning starts at lower bound and ends at upper bound.
        [SerializeField]
        private int morningLowerBound, morningUpperBound;

        [SerializeField]
        private int afternoonLowerBound, afternoonUpperBound;

        [SerializeField]
        private int eveningLowerBound, eveningUpperBound;
        #endregion

        #region Fields
        private int hour, minutes, day, week, year;
        private Days weekday;
        #endregion

        #region Properties
        
        public int Day => day;
        public Days Weekday => weekday;
        public int Week => week;
        public int Year => year;
        public int Hour => hour;
        public int Minute => minutes;
        #endregion

        public DateTime(int day, int week, int year, int hour, int minutes) {
            this.day = day;
            this.week = week;
            this.year = (year > 0) ? year : 1; // Year semantics for this game starts at year 1; the ternary here guards against a year 0
            this.hour = hour;
            this.minutes = minutes;
            this.weekday = Days.Sunday; // TODO: Interpolate the weekday from the parameter day

            // Day partition defaults, because for some reaason I am writing this on C#9.
            // Morning: 6AM-12PM
            morningLowerBound = 6;
            morningUpperBound = 12;
            // Afternoon: 12PM-5PM
            afternoonLowerBound = 12;
            afternoonUpperBound = 5;
            // Evening: 5PM - 10PM
            eveningLowerBound =  5;
            eveningUpperBound = 10;
        }

        public enum Days { 
            Sunday = 1,
            Monday = 2,
            Tuesday = 3,
            Wednesday = 4,
            Thursday = 5,
            Friday = 6,
            Saturday = 7
        }

        #region Boolean Checks
        public bool IsMorning() { return hour >= morningLowerBound && hour < morningUpperBound; }
        public bool IsAfternoon() { return hour >= afternoonLowerBound && hour < afternoonUpperBound; }
        public bool IsEvening() { return hour >= eveningLowerBound && hour < eveningUpperBound; }
        #endregion

        #region Time Management
        public void AdvanceMinutes(int minutesToAdvance) {
            if (minutes + minutesToAdvance >= 60) {
                int hoursAdvanced = minutes + minutesToAdvance / 60;
                minutes = (minutes + minutesToAdvance) % 60;
                AdvanceHour(hoursAdvanced);
            } else {
                minutes += minutesToAdvance;
            }
        }

        public void AdvanceHour(int hoursToAdvance) {
            if (hour + hoursToAdvance >= 24) {
                hour = hour + hoursToAdvance - 24;
            } else {
                hour++;
            }
        }

        public void AdvanceDay() {
            if (++day > 7) {
                week++;
                weekday = Days.Sunday;
            }

            if (day % 365 == 0) {
                year++;
            }
        }
        #endregion

        #region Strings
        public override string ToString() {
            return $"Day: {day}, Time: {TimeToString()}, Week: {week}, Year: {year}";
        }

        public string TimeToString() {
            int standardHour;
            if (hour == 0) { // 0 in raw time corresponds to 12AM
                standardHour = 12;
            } else if (hour > 12) { // >12 in raw time corresponds to 1PM-11PM
                standardHour  = hour - 12;
            } else {
                standardHour = hour;
            }

            // AM/PM = period, according to the Unicode standard. I just learned that while writing this.
            string period = (hour < 12) ? "AM" : "PM";

            // HH:MM(AM|PM)
            return $"{standardHour:D2}:{minutes:D2}{period}";
        }
        #endregion
    }
}