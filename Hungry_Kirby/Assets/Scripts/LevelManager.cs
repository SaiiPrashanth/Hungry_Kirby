using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Centralized manager for level transitions and persistence.
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Transition Settings")]
    public bool autoNextLevel = true;
    public float transitionDelay = 1.0f;

    private bool _isLevelCompleted = false;
    private bool _isLoading = false;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initiates the level completion sequence
    public void LoadNextLevel()
    {
        if (_isLevelCompleted || _isLoading) return;

        _isLevelCompleted = true;
        StartCoroutine(LevelTransitionRoutine());
    }

    // Reloads the currently active scene
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _isLevelCompleted = false;
        _isLoading = false;
    }

    private IEnumerator LevelTransitionRoutine()
    {
        _isLoading = true;
        yield return new WaitForSeconds(transitionDelay);

        if (autoNextLevel)
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextIndex);
            }
            else
            {
                Debug.Log("End of levels reached. Returning to start.");
                SceneManager.LoadScene(0);
            }
        }
        
        _isLevelCompleted = false;
        _isLoading = false;
    }
}
