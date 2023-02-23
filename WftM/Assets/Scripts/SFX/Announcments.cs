using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Announcments : MonoBehaviour
{
    public AudioClip[] voiceLines;
    public int[] lineSubIndexes;

    public KripSubs subSystem;

    AudioSource audioSource;
    TextMeshProUGUI subtitleText;

    int lastIndex = 1;

    public float announcementSpacing = 20;
    public float lastAnnouncement = -10;

    bool start = true;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        subtitleText = GameObject.FindGameObjectWithTag("SubText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastAnnouncement > announcementSpacing)
        {
            lastAnnouncement = Time.time;
            if (start)
            {
                PlayAnnouncement(0);
                start = false;
            }
            else PlayAnnouncement(-1);
        }
    }

    void PlayAnnouncement(int i)
    {
        if (audioSource.isPlaying || (subtitleText.enabled && subSystem != null))
            return;

        int index;
        do
        {
            index = Random.Range(0, voiceLines.Length);
        } while (voiceLines.Length > 1 && index == lastIndex);

        if (i >= 0) index = i;

        lastIndex = index;

        AudioClip randomClip = voiceLines[index];
        audioSource.clip = randomClip;
        audioSource.Play();

        // Make sure index values correspond to Subtitle System
        //subSystem.ShowSubtitle(randomIndex); //using calculated time
        float duration = randomClip.length + 1;
        subSystem.ShowSubtitle(lineSubIndexes[index], duration);
    }
}
