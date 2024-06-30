using UnityEngine;
using System.Collections;

public class CameraFade : MonoBehaviour
{
    public float fadeDuration = 5f;
    public Color fadeColor = Color.black;
    public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1);
    private float alpha = 0f;
    private Texture2D texture;
    private bool fading = false;

    private void Start()
    {
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
    }

    private void Update()
    {
        
    }

    public IEnumerator FadeTotal()
{
    if (!fading) 
    {
        fading = true;
        yield return StartCoroutine(DoFade()); 
    }
}

    private IEnumerator DoFade()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            alpha = 1 - Curve.Evaluate(elapsedTime / fadeDuration); 
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            yield return null;
        }

        // Fade Out
        elapsedTime = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            alpha = Curve.Evaluate(elapsedTime / fadeDuration);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            yield return null;
        }
        alpha = 1f;
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();

        yield return new WaitForSeconds(2f);

        // Fade In
        elapsedTime = 3f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            alpha = 1 - Curve.Evaluate(elapsedTime / fadeDuration); 
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            yield return null;
        }
        alpha = 0f;
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
        yield return new WaitForSeconds(2f);

        fading = false;
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
    }
}
