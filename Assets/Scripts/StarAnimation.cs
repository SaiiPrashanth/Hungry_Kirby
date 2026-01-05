using System.Collections;
using UnityEngine;

// Controls the sparkle animation loop for the Star object.
public class StarAnimation : MonoBehaviour
{
    [Header("Timing")]
    public float minInterval = 5f;
    public float maxInterval = 7f;
    public float velocityThreshold = 0.1f;

    private Animator _animator;
    private Rigidbody2D _rb;
    private Vector3 _lastPos;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _lastPos = transform.position;

        StartCoroutine(SparkleLoop());
    }

    // Checks if the star is stationary enough to play effects
    private bool IsStationary()
    {
        if (_rb != null)
        {
            return _rb.linearVelocity.magnitude < velocityThreshold;
        }

        float dist = Vector3.Distance(transform.position, _lastPos);
        _lastPos = transform.position;
        return dist < (velocityThreshold * Time.deltaTime);
    }

    private IEnumerator SparkleLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            if (IsStationary() && _animator != null)
            {
                _animator.SetTrigger("Sparkle");
            }
        }
    }
}