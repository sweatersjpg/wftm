using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject pausePanel, mainPanel, optionsPanel;

    public static bool isGamePaused;
    bool doneCheck;

    //Audio
    public AudioMixer musicMixer, sfxMixer;
    string volumeParameter = "Volume";

    public Slider sliderMusic, sliderSFX;

    bool doOnce;

    // Update is called once per frame
    void Update()
    {
        checkVolume();
        CheckPausedState();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
        }
    }

    void checkVolume()
    {
        if (!doOnce)
        {
            // Music
            float Mvolume = PlayerPrefs.GetFloat("MVolume");

            if (Mvolume == 0)
            {
                musicMixer.SetFloat(volumeParameter, -80);
            }
            else
            {
                musicMixer.SetFloat(volumeParameter, Mathf.Log10(Mvolume) * 20);
            }

            sliderMusic.value = Mvolume;
            sliderMusic.onValueChanged.AddListener(delegate { musicSlider(sliderMusic.value); });

            // SFX
            float SFXvolume = PlayerPrefs.GetFloat("SFXVolume");

            if (SFXvolume == 0)
            {
                sfxMixer.SetFloat(volumeParameter, -80);
            }
            else
            {
                sfxMixer.SetFloat(volumeParameter, Mathf.Log10(SFXvolume) * 20);
            }

            sliderSFX.value = SFXvolume;
            sliderSFX.onValueChanged.AddListener(delegate { sfxSlider(sliderSFX.value); });
            doOnce = true;
        }
    }
    void CheckPausedState()
    {
        switch (isGamePaused)
        {
            case true:
                if (doneCheck)
                    return;
                AudioListener.pause = true;
                mainPanel.SetActive(true);
                pausePanel.SetActive(true);
                Time.timeScale = 0f;
                doneCheck = true;
                break;
            case false:
                AudioListener.pause = false;
                optionsPanel.SetActive(false);
                pausePanel.SetActive(false);
                Time.timeScale = 1f;
                doneCheck = false;
                break;
        }
    }

    public void Resume()
    {
        isGamePaused = false;
    }

    public void GoToMenu(int levelIndex)
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelIndex);
    }

    public void musicSlider(float value)
    {
        if (value == 0)
        {
            musicMixer.SetFloat(volumeParameter, -80);
            PlayerPrefs.SetFloat("MVolume", 0);
        }
        else
        {
            musicMixer.SetFloat(volumeParameter, Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat("MVolume", value);
        }
    }

    public void sfxSlider(float value)
    {
        if (value == 0)
        {
            sfxMixer.SetFloat(volumeParameter, -80);
            PlayerPrefs.SetFloat("SFXVolume", 0);
        }
        else
        {
            sfxMixer.SetFloat(volumeParameter, Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("MVolume", sliderMusic.value);
        PlayerPrefs.SetFloat("SFXVolume", sliderSFX.value);
    }
}
