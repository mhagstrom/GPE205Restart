using UnityEngine;

public class ScorePickup : MonoBehaviour
{
    public int points;

    public void OnTriggerEnter(Collider other)
    {
        var controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.AddScore(points);

            Destroy(gameObject);
        }
    }
}