using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreakableSFX : MonoBehaviour
{
    public bool isAnimal;

    public AudioClip[] sfx;
    public AudioClip breakSFX;

    [SerializeField] KripSubs subSystem;
    public int[] sfxSubIndexes;

    public bool interrupt = true;

    TextMeshProUGUI subtitleText;

    AudioSource audioSource;

    int lastIndex = 0;

    bool die = false;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("MasterSFX").GetComponent<AudioSource>();
        subtitleText = GameObject.FindGameObjectWithTag("SubText").GetComponent<TextMeshProUGUI>();

        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (die && !audioSource.isPlaying) Destroy(gameObject);
        if(die)
        {
            sr.color = new Color(1, 1, 1, 1 - audioSource.time / 0.5f);
        }
    }

    void PlaySFX()
    {
        if (!interrupt && audioSource.isPlaying || (subtitleText.enabled && subSystem != null))
            return;

        int index;
        do
        {
            index = Random.Range(0, sfx.Length);
        } while (sfx.Length > 1 && index == lastIndex);
        lastIndex = index;

        AudioClip randomClip = sfx[index];
        audioSource.clip = randomClip;
        audioSource.Play();

        if (subSystem == null) return;
        // Make sure index values correspond to Subtitle System
        //subSystem.ShowSubtitle(randomIndex); //using calculated time
        float duration = randomClip.length + 1;
        subSystem.ShowSubtitle(sfxSubIndexes[index], duration);
    }

    void FinalSFX()
    {
        if (die) return;

        if (isAnimal)
        {
            audioSource.clip = breakSFX;
            audioSource.Play();
            Destroy(gameObject);
        }
        else
        {
            GetComponent<CircleCollider2D>().enabled = false;

            die = true;
            audioSource.clip = breakSFX;
            audioSource.Play();
        }
    }
}
