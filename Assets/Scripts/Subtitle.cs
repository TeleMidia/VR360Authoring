using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

public class SubtitleFragment
{
    public float start_time, duration;
    public string text;
    public SubtitleFragment(float start_time, float duration, string text)
    {
        this.start_time = start_time;
        this.duration = duration;
        this.text = text;
    }

    public override string ToString()
    {
        return "Start Time: " + start_time + " Duration: " + duration + "\n" + text;
    }
}

public class SubtitleReader
{
    public static SubtitleFragment[] ReadSubtitles(string file_path)
    {
        string strTargetString = System.IO.File.ReadAllText(file_path);

        string strReplacedString = Regex.Replace(strTargetString, @"(\n\s\n[0-9]+)", "\n");
        strReplacedString = Regex.Replace(strReplacedString, @"(^[a-zA-Z0-9]*)1\s\n", "");

        string[] splitString = Regex.Split(strReplacedString, @"\n\s\n");

        Thread.CurrentThread.CurrentCulture = new CultureInfo("hr-HR");

        SubtitleFragment[] subtitleFragments = new SubtitleFragment[splitString.Length];

        int i = 0;
        foreach (string s in splitString)
        {
            string first_line = s.Split('\n')[0];

            string[] durations = Regex.Split(first_line, " --> ");

            TimeSpan t_start = TimeSpan.Parse(durations[0]);
            TimeSpan t_end = TimeSpan.Parse(durations[1]);

            Console.WriteLine("Start: " + t_start.TotalSeconds + " Duration: " + (t_end.TotalSeconds - t_start.TotalSeconds));

            float start_time = (float)t_start.TotalSeconds;
            float duration = (float)(t_end.TotalSeconds - t_start.TotalSeconds);

            string text = s.Replace(first_line + "\n", "");
            text = text.Replace("<font color", "<color");
            text = text.Replace("</font>", "</color>");
            Console.WriteLine("Text:" + text);

            subtitleFragments[i++] = new SubtitleFragment(start_time: start_time, duration: duration, text: text);
        }

        return subtitleFragments;
    }
}