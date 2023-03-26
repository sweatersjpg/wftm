using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomLineSFX : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sfxLines;
    [SerializeField] KripSubs subSystem;
    [SerializeField] TextMeshProUGUI subtitleText;

    float duration;

    private void Start()
    {
        subtitleText = GameObject.FindGameObjectWithTag("SubText").GetComponent<TextMeshProUGUI>();
    }

    public void RandomSubtitle()
    {
        if (audioSource.isPlaying || subtitleText.enabled)
            return;

        int randomIndex = Random.Range(0, sfxLines.Length);
        AudioClip randomClip = sfxLines[randomIndex];
        audioSource.clip = randomClip;
        audioSource.Play();

        // Make sure index values correspond to Subtitle System
        //subSystem.ShowSubtitle(randomIndex); //using calculated time
        duration = randomClip.length + 1;
        subSystem.ShowSubtitle(randomIndex, duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RandomSubtitle();
        }
    }
}
