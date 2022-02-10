using UnityEngine;
using System;
using System.Text;
using System.Collections;

public class TimeUtil : MonoBehaviour
{
    const float _minute = 60f;
    const float _hour = 60f * 60f;
    const float _day = 24f * 60f * 60f;

    public static string FormatPackage(DateTime start, DateTime end)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("{0}.{1}.{2} {3:00}:{4:00} ~ {5}.{6}.{7} {8:00}:{9:00}", start.Year, start.Month, start.Day, start.Hour, start.Minute,
            end.Year, end.Month, end.Day, end.Hour, end.Minute);
        return sb.ToString();
    }

    // Time display format suitable for racing games
    public static string FormatRacing(float secondsTotal, bool showMilliseconds = false)
    {
        float hours = Mathf.Floor(secondsTotal / _hour);
        float secondsRemainder = secondsTotal % _hour;
        float minutes = Mathf.Floor(secondsRemainder / _minute);
        float seconds = secondsRemainder % _minute;

        if (secondsTotal < _minute)
        {
            return String.Format(showMilliseconds ? "{0:0.00}" : "{0:0}", seconds);
        }
        else if (secondsTotal < _hour)
        {
            return String.Format(showMilliseconds ? "{0:0}:{1:00.00}" : "{0:0}:{1:00}", minutes, seconds);
        }
        else
        {
            return String.Format("{0:0}:{1:00}:{2:00}", hours, minutes, Mathf.Ceil(seconds));
        }
    }

    // Time display format suitable for "builder" games
    public static string FormatBuilder(float secondsTotal, int num = 2)
    {
        int days = Mathf.FloorToInt(secondsTotal / _day);
        float daysRemainder = secondsTotal % _day;

        int hours = Mathf.FloorToInt(daysRemainder / _hour);
        float secondsRemainder = daysRemainder % _hour;

        int minutes = Mathf.FloorToInt(secondsRemainder / _minute);
        int seconds = Mathf.FloorToInt(secondsRemainder % _minute);

        var sb = new StringBuilder();

        // we only show two time elements at the same time
        int count = 0;

        if (days > 0)
        {
            sb.Append(String.Format("{0}d ", days));
            count++;
        }

        if (hours > 0)
        {
            sb.Append(String.Format("{0}h ", hours));
            count++;
        }

        if (minutes > 0 && count < num)
        {
            sb.Append(String.Format("{0}m ", minutes));
            count++;
        }

        if ((seconds > 0 && count < num) || (seconds == 0 && count == 0))
        {
           sb.Append(String.Format("{0}s", seconds));
        }

        return sb.ToString();
    }

    public static string Format(DateTime date)
    {
        DateTime today = DateTime.Now;
        DateTime yesterday = today.AddDays(-1);

        if (date.Date == today.Date)
        {
            return "txt_otherToday";
        }

        if (date.Date == yesterday.Date)
        {
            return "txt_otherYesterday";
        }

        TimeSpan elapsed = today.Date - date.Date;

        if (elapsed.TotalDays >= 365)
        {
            return "txt_otherOverAYear";
        }

        if (elapsed.TotalDays >= 7)
        {
            int weeks = Mathf.FloorToInt((float)elapsed.TotalDays / 7);
            string weeksString = weeks > 1 ? "txt_otherWeeksAgo" : "txt_otherWeekAgo";
            //return string.Format(weeksString.Localize(), weeks.ToString());
            return string.Format(weeksString, weeks.ToString());
        }

        double days = Mathf.FloorToInt((float)elapsed.TotalDays);
        string daysString = days > 1 ? "txt_otherDaysAgo" : "txt_otherDayAgo";
        //return string.Format(daysString.Localize(), days.ToString());
        return string.Format(daysString, days.ToString());
    }


}
