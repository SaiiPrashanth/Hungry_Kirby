using UnityEngine;
using UnityEngine.SceneManagement;

// Handles level resets when objects fall out of bounds.
public class SafetyNetBehaviour : MonoBehaviour
{
    [Header("Restart Rules")]
    public float winGracePeriod = 3.0f;
    
    private float _lastWinTime = -10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore triggers if the level was recently completed
        if (Time.time - _lastWinTime < winGracePeriod)
        {
            return;
        }
        
        Debug.Log($"{other.gameObject.name} out of bounds. Restarting level.");
        
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RestartLevel();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Updates the win timestamp to enable the grace period
    public void NotifyStarCaught()
    {
        _lastWinTime = Time.time;
    }
}