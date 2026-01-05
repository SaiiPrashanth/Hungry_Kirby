using System.Collections.Generic;
using UnityEngine;

// Manages the visual trail and collision logic for the cutting effect.
[RequireComponent(typeof(EdgeCollider2D))]
public class LineBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float followSpeed = 20f;
    public float minPointDistance = 0.1f;
    
    private List<Vector2> _points = new List<Vector2>();
    private EdgeCollider2D _edgeCollider;

    private void Start()
    {
        _edgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Updates the line's position and adds new points to the collider
    public void UpdateLine(Vector2 mousePos)
    {
        transform.position = Vector3.Lerp(transform.position, mousePos, followSpeed * Time.deltaTime);

        Vector2 localPos = mousePos - (Vector2)transform.position;

        if (_points.Count == 0)
        {
            _points.Add(localPos);
            return;
        }

        if (Vector2.Distance(_points[_points.Count - 1], localPos) > minPointDistance)
        {
            _points.Add(localPos);
            _edgeCollider.points = _points.ToArray();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Detects and destroys chain links upon contact
        if (other.CompareTag("Chain"))
        {
            Destroy(other.gameObject);
        }
    }
}