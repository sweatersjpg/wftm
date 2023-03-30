using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class DayNightSystem : MonoBehaviour
{
    [SerializeField] TMP_Text dayCounter; // canvas ui text to display
    int currentDay = 1; // current day

    public float dayLengthInSeconds = 60; //seconds the day should last for
    public float nightLengthInSeconds = 30; //seconds the night should last for
    private float timeOfDayInSeconds = 0f; //current time

    public Light2D globalLight; // intensity to 1 when day and intensity to 0 when night, should lerp or be smooth when changing

    void Update()
    {
        // Update the time of day based on the game's clock.
        timeOfDayInSeconds += Time.deltaTime;

        // If it's daytime, increase the light's intensity.
        if (timeOfDayInSeconds <= dayLengthInSeconds)
        {
            globalLight.intensity = Mathf.Lerp(0, 1, timeOfDayInSeconds / dayLengthInSeconds);
        }
        // If it's nighttime, decrease the light's intensity.
        else if (timeOfDayInSeconds <= dayLengthInSeconds + nightLengthInSeconds)
        {
            globalLight.intensity = Mathf.Lerp(1, 0, (timeOfDayInSeconds - dayLengthInSeconds) / nightLengthInSeconds);
        }
        // If the day has ended, start a new day.
        else
        {
            currentDay++;
            dayCounter.text = "Day " + currentDay.ToString();
            timeOfDayInSeconds = 0f;
        }
    }
}
