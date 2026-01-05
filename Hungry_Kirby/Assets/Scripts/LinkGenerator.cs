using UnityEngine;

// Generates a chain of hinge joints connecting a hook to a weight.
public class LinkGenerator : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D hook;
    public GameObject linkPrefab;
    public GameObject weight;
    
    [Header("Chain Config")]
    public int numberOfLinks = 5;

    private void Start()
    {
        GenerateChain();
    }

    private void GenerateChain()
    {
        Rigidbody2D prevBody = hook;

        for (int i = 0; i < numberOfLinks; i++)
        {
            GameObject newLink = Instantiate(linkPrefab, transform);
            HingeJoint2D joint = newLink.GetComponent<HingeJoint2D>();
            
            if (joint != null)
            {
                joint.connectedBody = prevBody;
            }

            prevBody = newLink.GetComponent<Rigidbody2D>();
        }

        // Connect the final weight (e.g., Star) to the end of the chain
        if (weight != null)
        {
            HingeJoint2D weightJoint = weight.AddComponent<HingeJoint2D>();
            weightJoint.autoConfigureConnectedAnchor = false;
            weightJoint.connectedBody = prevBody;
            weightJoint.connectedAnchor = new Vector2(0, -0.5f);
        }
    }
}