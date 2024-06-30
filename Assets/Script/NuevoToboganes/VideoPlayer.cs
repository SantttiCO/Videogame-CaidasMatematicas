using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer.GetComponent<Renderer>().sortingOrder = 1;
        videoPlayer.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            videoPlayer.gameObject.SetActive(!videoPlayer.gameObject.activeSelf);
        }
    }
}
