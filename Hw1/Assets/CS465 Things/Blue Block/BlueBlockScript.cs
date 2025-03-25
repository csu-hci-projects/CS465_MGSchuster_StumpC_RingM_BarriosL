using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Renderer blockRenderer;
    private Color originalColor = Color.blue;
    private Color highlightColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        blockRenderer = GetComponent<Renderer>();
        blockRenderer.material.color = originalColor;
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") || other.CompareTag("Controller"))
        {
            blockRenderer.material.color = highlightColor;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand") || other.CompareTag("Controller"))
        {
            blockRenderer.material.color = originalColor;
        }
    }
}
