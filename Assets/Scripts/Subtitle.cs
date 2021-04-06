using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// Defines a subtitle fragment, which is a block of text that appears at a certain time and disappears.
/// </summary>
public class SubtitleFragment
{
    /// <summary>
    /// Time at which the fragment starts in seconds.
    /// </summary>
    public float begin;
    /// <summary>
    /// Duration of the fragment in seconds.
    /// </summary>
    public float duration;
    /// <summary>
    /// Text of the fragment.
    /// </summary>
    public string text;
    /// <summary>
    /// Constructor of the fragment.
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
    /// Readable information about the object.
    /// </summary>
    /// <returns>string with information about the object</returns>
    public override string ToString()
    {
        return "Start Time: " + begin + " Duration: " + duration + "\n" + text;
    }
}
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// This class reads a srt file and converts it to Subtitles Fragments.
/// </summary>
public class SubtitleReader
{
    /// <summary>
    /// Reads a srt file and returns it as a list of subtitles fragments.
    /// </summary>
    /// <param name="file_path">path of the srt file</param>
    /// <returns>list of subtitles fragments</returns>
    public static SubtitleFragment[] ReadSubtitles(string file_path)
    {
        string strTargetString = System.IO.File.ReadAllText(file_path);
        var content = new Regex(@"[a-zA-Z0-9]+");
        var pattern_sppliter = new Regex(@"\n(\s*\n)+");

        string[] splitString = pattern_sppliter.Split(strTargetString).Where(s => s != String.Empty && content.IsMatch(s)).ToArray();

        SubtitleFragment[] subtitleFragments = new SubtitleFragment[splitString.Length];

        int i = 0;
        foreach (string st in splitString)
        {
            string[] lines = Regex.Split(st, "\n");

            string[] durations = Regex.Split(lines[1], " --> ");

            TimeSpan t_start = TimeSpan.Parse(durations[0]);
            TimeSpan t_end = TimeSpan.Parse(durations[1]);

            //Debug.Log("Start: " + t_start.TotalSeconds + " Duration: " + (t_end.TotalSeconds - t_start.TotalSeconds));

            float begin = (float)t_start.TotalSeconds;
            float duration = (float)(t_end.TotalSeconds - t_start.TotalSeconds);

            string text = string.Join(Environment.NewLine, lines.Skip(2)).Trim();
            text = text.Replace("<font color", "<color");
            text = text.Replace("</font>", "</color>");
            Console.WriteLine("Text:" + text);

            subtitleFragments[i++] = new SubtitleFragment(begin: begin, duration: duration, text: text);
        }

        return subtitleFragments;
    }
}