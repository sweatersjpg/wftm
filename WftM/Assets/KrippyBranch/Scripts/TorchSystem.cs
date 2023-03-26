using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchSystem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private GameObject torchObject;
    [SerializeField] private Light2D torchLight;

    public float baseIntensity = 1f;
    public float flickerRange = 0.1f;
    public float flickerSpeed = 10f;
    public float fadeSpeed = 2f;

    private float currentIntensity;
    private float flickerAmount;
    private bool fading;

    private Coroutine fadeCoroutine;

    private void Start()
    {
        currentIntensity = baseIntensity;
    }

    void Update()
    {
        if (playerSprite.sprite.name != "chop_0" && playerSprite.sprite.name != "chop_1")
        {
            if (fadeCoroutine == null)
            {
                fadeCoroutine = StartCoroutine(FadeLight());
            }
            torchObject.SetActive(true);
        }
        else
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }
            torchObject.SetActive(false);
            torchLight.intensity = baseIntensity;
            fading = false;
        }
        if (!fading)
        {
            flickerAmount = Mathf.Sin(Time.time * flickerSpeed) * flickerRange;
            currentIntensity = baseIntensity + flickerAmount;
            torchLight.intensity = currentIntensity;
        }
    }

    IEnumerator FadeLight()
    {
        fading = true;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            torchLight.intensity = Mathf.Lerp(0, baseIntensity, t);
            yield return null;
        }

        torchLight.intensity = baseIntensity;
        fading = false;
    }
}
