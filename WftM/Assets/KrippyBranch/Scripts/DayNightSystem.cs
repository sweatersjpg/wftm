using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNightSystem : MonoBehaviour
{
    private float dayLengthInSeconds = 120f;
    private float timeOfDayInSeconds = 0f;

    public Light2D globalLight;

    private void Update()
    {
       

        timeOfDayInSeconds += Time.deltaTime / dayLengthInSeconds * 86400f; // this is just how many seconds in a real day
        timeOfDayInSeconds = Mathf.Repeat(timeOfDayInSeconds, 86400f);

        float timeOfDay = timeOfDayInSeconds / 86400f;
        float angle = timeOfDay * Mathf.PI * 2f;
        float sine = (Mathf.Sin(angle) + 1f) / 2f;

        globalLight.color = Color.Lerp(Color.black, Color.white, sine);
        globalLight.intensity = Mathf.Lerp(0f, 1f, sine);
    }
}