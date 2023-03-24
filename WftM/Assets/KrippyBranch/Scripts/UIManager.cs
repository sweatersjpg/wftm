using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager instance;

    [SerializeField] GameObject optionsPanel;
    [SerializeField] Slider sliderMusic, sliderSFX;
    [SerializeField] AudioMixer musicMixer, sfxMixer;

    public bool canBePaused = false;
    static bool isGamePaused = false;
    bool doOnce;

    string volumeParameter = "Volume";

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            canBePaused = false;
        else
            canBePaused = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canBePaused)
            isGamePaused = !isGamePaused;

        if (!doOnce)
            LoadVolume();

        CheckPausedState();
    }

    void LoadVolume()
    {
        float Mvolume = PlayerPrefs.GetFloat("MVolume", 0);
        float SFXvolume = PlayerPrefs.GetFloat("SFXVolume", 0);

        musicMixer.SetFloat(volumeParameter, Mvolume == 0 ? -80 : Mathf.Log10(Mvolume) * 20);
        sliderMusic.value = Mvolume;
        sliderMusic.onValueChanged.AddListener(delegate { musicMixer.SetFloat(volumeParameter, Mathf.Log10(sliderMusic.value) * 20); PlayerPrefs.SetFloat("MVolume", sliderMusic.value); });

        sfxMixer.SetFloat(volumeParameter, SFXvolume == 0 ? -80 : Mathf.Log10(SFXvolume) * 20);
        sliderSFX.value = SFXvolume;
        sliderSFX.onValueChanged.AddListener(delegate { sfxMixer.SetFloat(volumeParameter, Mathf.Log10(sliderSFX.value) * 20); PlayerPrefs.SetFloat("SFXVolume", sliderSFX.value); });

        doOnce = true;
    }

    void CheckPausedState()
    {
        if (isGamePaused)
        {
            optionsPanel.SetActive(true);
            AudioListener.pause = true;
            Time.timeScale = 0f;
        }
        else
        {
            AudioListener.pause = false;
            optionsPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Resume()
    {
        isGamePaused = false;
        CheckPausedState();
    }

    public void musicSlider(float value)
    {
        musicMixer.SetFloat(volumeParameter, value == 0 ? -80 : Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MVolume", value);
    }

    public void sfxSlider(float value)
    {
        sfxMixer.SetFloat(volumeParameter, value == 0 ? -80 : Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void StartGame(int levelIndex)
    {
        isGamePaused = false;
        SceneManager.LoadScene(levelIndex);
    }

    public void QuitGame() => Application.Quit();
}
