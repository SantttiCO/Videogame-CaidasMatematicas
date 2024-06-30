using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Forma_ambient : MonoBehaviour
{
    [SerializeField] private GameObject UIPause;

    User user = new User();

    [SerializeField] private bool Pause;

    public GameObject BasePrefab;

    public void PrevButton()
    {

        SceneManager.LoadScene("Cannon");


    }

    public void Menu()
    {

        SceneManager.LoadScene("Inicio");

        StartCoroutine(ResetScore(user));


    }





    IEnumerator ResetScore(User user)
    {

        user.score = 0;

        Debug.Log("Score reseteado");
        WWWForm form = new WWWForm();
        form.AddField("score", user.score);

        using (UnityWebRequest www = UnityWebRequest.Post("http://pushi.pythonanywhere.com/delete", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Response: " + responseText);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {


            Pause = !Pause;

        }



        if (Pause)
        {
            Time.timeScale = 0;

            ActivateMenu();
            Cursor.visible = true;
            // Bloquea el cursor en el centro de la pantalla
            Cursor.lockState = CursorLockMode.None;
        }

        else
        {

            Time.timeScale = 1;
            DeactivateMenu();
        }

    }

    void ActivateMenu()
    {

        UIPause.SetActive(true);


    }


    void DeactivateMenu()
    {

        UIPause.SetActive(false);
        Pause = false;
    }

}





