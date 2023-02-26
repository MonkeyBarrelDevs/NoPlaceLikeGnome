using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeHandler : MonoBehaviour
{
    private float secondsSinceLastTick;
    private int gameMinutesPerTick;
    private DateTime dateTime;

    [SerializeField]
    private int startMinute = 0, startHour = 6, startDay = 1, startWeek = 1, startYear = 1;

    [Header("Time Controls")]
    [SerializeField]
    private int realMinuteDayLength = 10;
    [SerializeField]
    private int secondsBetweenTicks = 1;

    [SerializeField] [Range(0, 23)]
    private int morningLowerBound, morningUpperBound;
    [SerializeField] [Range(0, 23)]
    private int afternoonLowerBound, afternoonUpperBound;
    [SerializeField] [Range(0, 23)]
    private int eveningLowerBound, eveningUpperBound;

    public static UnityAction<DateTime> OnTimeChanged;
    public static UnityAction<DateTime> OnDayChanged;

    private void Awake() {
        int gameSecondsPerRealSecond = 86_400 / (realMinuteDayLength * 60); // Game seconds per 1 real second 
        gameMinutesPerTick = (gameSecondsPerRealSecond * secondsBetweenTicks) / 60; // Game seconds per tick

        dateTime = new(startDay, startWeek, startYear, startHour, startMinute,
                       morningLowerBound, morningUpperBound, afternoonLowerBound, afternoonUpperBound, eveningLowerBound, eveningUpperBound);
    }

    void Start() { }
    void Update() {
        secondsSinceLastTick += Time.deltaTime;

        if (secondsSinceLastTick >= secondsBetweenTicks) {
            secondsSinceLastTick = 0;
            Tick();
        }
    }

    private void Tick() {
        AdvanceTime();
        Debug.Log(dateTime.ToString());
    }

    private void AdvanceTime() {
        dateTime.AdvanceMinutes(gameMinutesPerTick);
        OnTimeChanged?.Invoke(dateTime);

        if (dateTime.IsNight()) {
            dateTime.AdvanceToTime(6, 0); // Advance to 6AM
            OnDayChanged?.Invoke(dateTime);
        }
    }

    public void SkipDay() {
        dateTime.AdvanceToTime(6, 0);
        OnDayChanged?.Invoke(dateTime);
    }

    public struct DateTime {
        #region Serialized Time Controls
        // Morning starts at lower bound and ends at upper bound.
        private int morningLowerBound, morningUpperBound;

        private int afternoonLowerBound, afternoonUpperBound;
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

        #region Constructors
        public DateTime(int day, int week, int year, int hour, int minutes, 
                        int morningLower, int morningUpper, int afternoonLower, int afternoonUpper, int eveningLower, int eveningUpper) {

            this.day = day;
            this.week = week;
            this.year = (year > 0) ? year : 1; // Year semantics for this game starts at year 1; the ternary here guards against a year 0
            this.hour = hour;
            this.minutes = minutes;
            this.weekday = Days.Sunday; // TODO: Interpolate the weekday from the parameter day

            // Day partition defaults, because for some reaason I am writing this on C#9.
            // Morning: 6AM-12PM
            this.morningLowerBound = morningLower;
            morningUpperBound = morningUpper;
            // Afternoon: 12PM-5PM
            afternoonLowerBound = afternoonLower;
            afternoonUpperBound = afternoonUpper;
            // Evening: 5PM - 10PM
            eveningLowerBound =  eveningLower;
            eveningUpperBound = eveningUpper;
        }
        #endregion

        #region Boolean Checks
        public bool IsMorning() { return hour >= morningLowerBound && hour < morningUpperBound; }
        public bool IsAfternoon() { return hour >= afternoonLowerBound && hour < afternoonUpperBound; }
        public bool IsEvening() { return hour >= eveningLowerBound && hour < eveningUpperBound; }
        public bool IsNight() { return hour >= eveningUpperBound; }
        #endregion

        #region Time Management
        public void AdvanceToTime(int hour, int minutes) {
            if ((hour < 0 || hour > 23) || (minutes < 0 || minutes > 59)) {
                Debug.Log("Tried to advance to an invalid time!");
                return;
            }

            if (hour < this.hour || (hour == this.hour && minutes <= this.minutes)) {
                AdvanceDay();
            }

            this.hour = hour;
            this.minutes = minutes;
        }

        public void AdvanceMinutes(int minutesToAdvance) {
            if (minutes + minutesToAdvance >= 60) {
                int hoursAdvanced = (minutes + minutesToAdvance) / 60;
                minutes = (minutes + minutesToAdvance) % 60;
                AdvanceHour(hoursAdvanced);
            } else {
                minutes += minutesToAdvance;
            }
        }

        public void AdvanceHour(int hoursToAdvance) {
            if (hour + hoursToAdvance >= 24) {
                AdvanceDay();
                hour = hour + hoursToAdvance - 24;
            } else {
                hour += hoursToAdvance;
            }
        }

        public void AdvanceDay() {
            if (++day % 8 == 0) { // 7 = Saturday, n*8 = new weeks/Sunday
                week++;
                weekday = Days.Sunday;
            }

            weekday = (Days)(day % 7);

            if (day % 365 == 0) {
                year++;
            }
        }
        #endregion

        #region Strings
        public override string ToString() {
            return $"Time: {TimeToString()}, Weekday: {weekday}, Day: {day}, Week: {week}, Year: {year}";
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

        #region enums
        public enum Days {
            Sunday = 1,
            Monday = 2,
            Tuesday = 3,
            Wednesday = 4,
            Thursday = 5,
            Friday = 6,
            Saturday = 7
        }
        #endregion
    }
}