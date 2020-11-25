using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
/// <summary>
/// Author: Paulo Renato Conceição Mendes
/// Defines a subtitle fragment, which is a block of text that appears at a certain time and disappears
/// </summary>
public class SubtitleFragment
{
    public float begin, duration;
    public string text;
    /// <summary>
    /// Constructor of the fragment
    /// </summary>
    /// <param name="begin">time of begin in seconds</param>
    /// <param name="duration">duration in seconds</param>
    /// <param name="text">text of the fragment</param>
    public SubtitleFragment(float begin, float duration, string text)
    {
        this.begin = begin;
        this.duration = duration;
        this.text = text;
    }
    /// <summary>
    /// Readable information about the object
    /// </summary>
    /// <returns>string with information about the object</returns>
    public override string ToString()
    {
        return "Start Time: " + begin + " Duration: " + duration + "\n" + text;
    }
}
/// <summary>
/// Author: Paulo Renato Conceição Mendes
/// This class reads a srt file and converts it to Subtitles Fragments
/// </summary>
public class SubtitleReader
{
    /// <summary>
    /// Reads a srt file and returns it as a list of subtitles fragments
    /// </summary>
    /// <param name="file_path">path of the srt file</param>
    /// <returns>list of subtitles fragments</returns>
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

            float begin = (float)t_start.TotalSeconds;
            float duration = (float)(t_end.TotalSeconds - t_start.TotalSeconds);

            string text = s.Replace(first_line + "\n", "");
            text = text.Replace("<font color", "<color");
            text = text.Replace("</font>", "</color>");
            Console.WriteLine("Text:" + text);

            subtitleFragments[i++] = new SubtitleFragment(begin: begin, duration: duration, text: text);
        }

        return subtitleFragments;
    }
}