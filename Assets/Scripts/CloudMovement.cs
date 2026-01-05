using UnityEngine;

// Moves background cloud objects horizontally.
public class CloudMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;

    private void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }
}
