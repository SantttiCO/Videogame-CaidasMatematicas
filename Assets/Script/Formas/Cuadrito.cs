using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Cuadrito : MonoBehaviour
{
    public Material defaultMaterial;
    public Material highlightedMaterial;
    private Renderer renderer;
    private bool isDefault = true;

    void Start()
    {
        
        renderer = GetComponent<Renderer>();
        renderer.material = defaultMaterial;
        
    }

    


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger event with: " + other.name);
        ToggleMaterial();
        Debug.Log("IsHighlighted: " + IsHighlighted());
    }

    void ToggleMaterial()
    {
        if (isDefault)
        {
            renderer.material = highlightedMaterial;
        }
        else
        {
            renderer.material = defaultMaterial;
        }
        isDefault = !isDefault;  // Cambia el estado
    }

    public bool IsHighlighted()
    {
        return !isDefault;
    }

}

