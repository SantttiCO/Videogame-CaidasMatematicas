using UnityEngine;
using TMPro;

public class CanvasRespuestas : MonoBehaviour
{
    public TextMeshPro R1;
    public TextMeshPro R2;
    public TextMeshPro R3;

    public void UpdateAnswers(string respuesta_a, string respuesta_b, string respuesta_c)
    {
        R1.text = respuesta_a;
        R2.text = respuesta_b;
        R3.text = respuesta_c;
    }
}
