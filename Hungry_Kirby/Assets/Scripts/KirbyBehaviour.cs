using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages Kirby's interactions, animations, and the star collection sequence.
public class KirbyBehaviour : MonoBehaviour
{
    // Internal state tracking
    private bool _isStarCaught = false;
    private bool _isLevelEnding = false;

    [Header("Animation Settings")]
    public int clicksForCute = 3;
    public int clicksForAngry = 6;
    public float clickResetTime = 2f;

    private int _clickCount = 0;
    private float _lastClickTime = 0f;

    [Header("System References")]
    public bool debugMode = true;

    private Animator _animator;
    private Coroutine _resetClickRoutine;
    private Coroutine _idleAnimationRoutine;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        // Ensure a LevelManager instance exists in the scene
        if (LevelManager.Instance == null)
        {
            GameObject go = new GameObject("LevelManager");
            go.AddComponent<LevelManager>();
        }

        _idleAnimationRoutine = StartCoroutine(DoIdleAnimation());
        
        if (debugMode) Debug.Log("Kirby initialized and ready.");
    }

    private void Update()
    {
        UpdateFacingDirection();
    }

    // Flips the character sprite to face the mouse position
    private void UpdateFacingDirection()
    {
        if (Camera.main == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 scale = transform.localScale;
        
        if (mousePos.x < transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }
        
        transform.localScale = scale;
    }

    private void OnMouseDown()
    {
        if (_isStarCaught || _isLevelEnding) return;

        // Reset click counter after inactivity
        if (Time.time - _lastClickTime > clickResetTime)
        {
            _clickCount = 0;
        }

        _clickCount++;
        _lastClickTime = Time.time;

        if (_resetClickRoutine != null) StopCoroutine(_resetClickRoutine);

        // Trigger reactions based on interaction frequency
        if (_clickCount == clicksForCute)
        {
            _animator.SetTrigger("Pookie");
        }
        else if (_clickCount >= clicksForAngry)
        {
            _animator.SetTrigger("Angry");
            _clickCount = 0;
        }

        _resetClickRoutine = StartCoroutine(ResetClicks());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Star"))
        {
            if (_isStarCaught || _isLevelEnding) return;

            HandleStarCollection(other.gameObject);
        }
    }

    // Processes the star collection logic and visual transitions
    private void HandleStarCollection(GameObject star)
    {
        if (debugMode) Debug.Log("Star collected.");

        Rigidbody2D starRb = star.GetComponent<Rigidbody2D>();
        if (starRb != null)
        {
            starRb.linearVelocity = Vector2.zero;
            starRb.isKinematic = true;
        }

        _isStarCaught = true;

        // Notify safety net to prevent accidental restarts
        SafetyNetBehaviour safetyNet = FindObjectOfType<SafetyNetBehaviour>();
        if (safetyNet != null)
        {
            safetyNet.NotifyStarCaught();
        }

        if (_idleAnimationRoutine != null) StopCoroutine(_idleAnimationRoutine);

        _animator.SetTrigger("Eat");
        StartCoroutine(ExecuteEatSequence(star));
    }

    private IEnumerator ResetClicks()
    {
        yield return new WaitForSeconds(clickResetTime);
        _clickCount = 0;
    }

    private IEnumerator DoIdleAnimation()
    {
        while (!_isStarCaught && !_isLevelEnding)
        {
            yield return new WaitForSeconds(5f);
            if (!_isStarCaught && !_isLevelEnding)
            {
                _animator.SetTrigger("Doubt");
            }
        }
    }

    private IEnumerator ExecuteEatSequence(GameObject star)
    {
        float duration = 0.5f;
        float steps = 20;
        
        // Scale and move the star towards Kirby's position
        for (int i = 0; i < steps; i++)
        {
            if (star == null) break;

            float t = (float)i / steps;
            float scale = 1f - t;
            star.transform.localScale = new Vector3(scale, scale, scale);
            star.transform.position = Vector3.Lerp(star.transform.position, transform.position, 0.2f);
            
            yield return new WaitForSeconds(duration / steps);
        }

        if (star != null) Destroy(star);

        yield return new WaitForSeconds(0.8f);
        
        _isLevelEnding = true;
        
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
}