using UnityEngine;

// Handles user input for the line-cutting mechanic.
public class LineManager : MonoBehaviour
{
    public GameObject linePrefab;
    
    private GameObject _currentLine;
    private LineBehaviour _activeLineScript;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewLine();
        }

        if (_activeLineScript != null)
        {
            UpdateActiveLine();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DestroyActiveLine();
        }
    }

    private void CreateNewLine()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currentLine = Instantiate(linePrefab, mousePos, Quaternion.identity);
        _activeLineScript = _currentLine.GetComponent<LineBehaviour>();
    }

    private void UpdateActiveLine()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _activeLineScript.UpdateLine(mousePos);
    }

    private void DestroyActiveLine()
    {
        _activeLineScript = null;
        if (_currentLine != null)
        {
            Destroy(_currentLine);
        }
    }
}