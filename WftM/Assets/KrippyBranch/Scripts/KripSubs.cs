using System.Collections;
using TMPro;
using UnityEngine;

public class KripSubs : MonoBehaviour
{
    // Subtitle system, when calling, use: ShowSubtitle(the index of the subtitle line, duration you want the text to be shown for)
    // Example can be seen in KrippyBranch/Scenes/TestEnv

    [SerializeField] string[] subtitles;
    [SerializeField] TextMeshProUGUI subtitleText;
    private const float MIN_DURATION = 1f;

    public void ShowSubtitle(int index, float duration)
    {
        if (duration < 0f)
        {
            //duration = CalculateDuration(subtitles[index]);
            duration = Mathf.Max(duration, MIN_DURATION);
        }
        subtitleText.text = subtitles[index];
        subtitleText.enabled = true;
        StartCoroutine(HideSubtitle(duration));
    }

    //private float CalculateDuration(string subtitle)
    //{
    //    float wordsPerMinute = 225f;  // assuming average reading speed of 225 WPM
    //    float duration = subtitle.Split(' ').Length / wordsPerMinute * 60f;
    //    return duration;
    //}

    private IEnumerator HideSubtitle(float duration)
    {
        yield return new WaitForSeconds(duration);
        subtitleText.enabled = false;
    }
}
