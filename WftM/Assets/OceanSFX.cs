using UnityEngine;

public class OceanSFX : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 10f;
    public float minDistance = 2f;
    public AudioSource audioSource;

    void Update()
    {
        float closestDistance = Mathf.Infinity;

        foreach (var colliderPoint in GetComponent<PolygonCollider2D>().points)
        {
            Vector3 pointWorldPosition = transform.TransformPoint(colliderPoint);
            float distance = Vector3.Distance(player.position, pointWorldPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }

        float volume = Mathf.Lerp(0f, 1f, 1f - Mathf.InverseLerp(minDistance, maxDistance, closestDistance));

        audioSource.volume = volume;
    }
}
