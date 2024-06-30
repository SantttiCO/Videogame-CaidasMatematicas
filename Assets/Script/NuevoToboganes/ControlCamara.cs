using UnityEngine;

public class ControlCamara : MonoBehaviour
{
    public Transform objetivoASeguir;
    private float distanciaObjetivoX = 0f;
    private float distanciaObjetivoY = -100f;
    private float distanciaObjetivoZ = 250f;
    private float velocidadCamara = 25f;
    public bool suavizadoActivado = false;
    private Vector3 nuevaPosicion;

    void Update()
    {
        nuevaPosicion = this.transform.position;
        nuevaPosicion.x = objetivoASeguir.transform.position.x - distanciaObjetivoX;
        nuevaPosicion.y = objetivoASeguir.transform.position.y - distanciaObjetivoY;
        nuevaPosicion.z = objetivoASeguir.transform.position.z - distanciaObjetivoZ;
        if (suavizadoActivado)
        {
            this.transform.position =
                Vector3.Lerp(this.transform.position, nuevaPosicion, velocidadCamara * Time.deltaTime);
        }
        else
        {
            this.transform.position = nuevaPosicion;
        }
    }
}