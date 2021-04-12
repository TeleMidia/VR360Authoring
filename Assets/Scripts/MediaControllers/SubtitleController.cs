using System;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// Inherits MediaControllerAbstract. This class is the script that controls the sutitle media object.
/// </summary>
class SubtitleController : MediaControllerAbstract
{
    /// True if the subtitle has already started.
    private bool hasPlayedBefore;
    /// Subtitle texts, begin times and durations.
    private SubtitleFragment[] subtitleFragments;
    /// The index of the current subtitle fragment.
    private int current_index;
    /// The MeshRenderer of the subtitle prefab.
    private TextMesh textMesh;

    /// <summary>
    /// Inherited from MediaControllerAbstract.
    ///  Loads the text to the TextMesh component.
    /// </summary>
    public override void Load()
    {
        this.hasPlayedBefore = false;
        this.subtitleFragments = SubtitleReader.ReadSubtitles(file_path);
        this.current_index = 0;
        this.textMesh = GetComponent<TextMesh>();
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract.
    ///  Playing for the subtitle consists in invoking all the text changes in the first time. The next times, it only activates its MeshRenderer.
    /// </summary>
    public override void PlayMedia()
    {

        if (!this.hasPlayedBefore)
        {
            this.hasPlayedBefore = true;

            for(int i = 0; i<this.subtitleFragments.Length;i++)
            {
                Invoke("NextText", subtitleFragments[i].begin);
                float finish_time = subtitleFragments[i].begin + subtitleFragments[i].duration;

                if (i+1 >= this.subtitleFragments.Length || finish_time < subtitleFragments[i+1].begin)
                {
                    Invoke("RemoveText", finish_time);
                }
            }
        }
        GetComponent<MeshRenderer>().enabled = true;
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract.
    ///  Stopping for the subtitle consists in disabling its MeshRenderer.
    /// </summary>
    public override void StopMedia()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    /// <summary>
    /// Changes the text of the subtitle for the next text.
    /// </summary>
    private void NextText()
    {
        if(this.current_index < this.subtitleFragments.Length)
        {
            this.textMesh.text = this.prefix + this.subtitleFragments[this.current_index++].text;
        }
    }
    /// <summary>
    /// Removes the text of the subtitle
    /// </summary>
    private void RemoveText()
    {
        this.textMesh.text = String.Empty;
    }
}

