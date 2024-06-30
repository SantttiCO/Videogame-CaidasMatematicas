using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CuadritoVol : MonoBehaviour
{
    public Material defaultMaterial;
    public Material highlightedMaterial;
    private Renderer renderer;
    private bool isDefault = true;
    public float normalHeight = 110f;     // Altura normal
    public float highlightedHeight = 2000f;

    public float transitionSpeed = 0.5f; // Velocidad de transición
    private float ElapsedTime ;

    private Vector3 targetScale; // Escala de destino
    private bool isTransitioning = false; // 

    void Start()
    {

        renderer = GetComponent<Renderer>();
        renderer.material = defaultMaterial;

        Vector3 scale = transform.localScale;
        targetScale = transform.localScale;
        scale.y = normalHeight;
        transform.localScale = scale;

    }
    


    void OnTriggerEnter(Collider other)
    {
        Debug.Log(" ENTRO Trigger event with: " + other.name);
        other.gameObject.transform.SetParent(transform);
        ToggleMaterial();
        ToggleHeight();
        //Debug.Log("IsHighlighted: " + IsHighlighted());
    }


    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.SetParent(null);
        other.transform.localScale = new Vector3(5, 5, 5);
        Debug.Log("SALIO");
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
            // Debug.Log("Material predeterminado reasignado T.");
        }
        isDefault = !isDefault;  // Cambia el estado
    }


    void ToggleHeight()
    {
        // Cambiar la escala de destino y activar la transición
        targetScale.y = isDefault ? normalHeight : highlightedHeight;
        isTransitioning = true;
    }

    void Update()
    {
        // Realizar la transición suave si es necesario
        if (isTransitioning)
        {
            ElapsedTime += Time.deltaTime;

            // Calcular el factor de interpolación, asegurando que no exceda de 1
            float t = Mathf.Clamp01(ElapsedTime / transitionSpeed);

            // Aplicar la interpolación suave
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);

            GameObject.FindWithTag("Player").transform.localScale = new Vector3(110 / transform.localScale.x, 110 / transform.localScale.y, 110 / transform.localScale.z);



            // Si estamos cerca del tamaño objetivo, detener la transición
            if (t >= 1.0f)
            {
                isTransitioning = false;  // Marcar la transición como completa
                ElapsedTime = 0;          // Resetear ElapsedTime si la transición es para un solo uso
            }



            // Ajustar el tamaño del collider para que coincida con la altura actual
        }
    }

    public bool IsHighlighted()
    {
        return !isDefault;
    }

}

