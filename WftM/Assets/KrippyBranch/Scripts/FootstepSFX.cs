using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSFX : MonoBehaviour
{
    // Will later change to match pitch/speed on player's main movement script
    // Just did this to mess with the pause menu for now (SFX)
    AudioSource audioSource;

    float volumeReductionSpeed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        // Footsteps
        float movementMagnitude = movement.magnitude;
        float targetVolume = Mathf.Lerp(0, 1, movementMagnitude);

        if (movementMagnitude > 0)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime);
        }
        else
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime * volumeReductionSpeed);
        }
    }
}
