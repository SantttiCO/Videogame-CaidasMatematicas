using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
public class nextlevelController : MonoBehaviour
{
    public VideoPlayer videoPlayer2;
    public Canvas pregunta;


    private void Start()
    {

        videoPlayer2.GetComponent<Renderer>().sortingOrder = 1;
        videoPlayer2.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            showVideoEnd();

        }

    }

    public void showVideoEnd()
    {

        videoPlayer2.gameObject.SetActive(!videoPlayer2.gameObject.activeSelf);
    }
}
