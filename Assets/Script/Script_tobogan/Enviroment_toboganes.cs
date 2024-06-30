using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;

public class Enviroment_toboganes : MonoBehaviour
{

    [SerializeField] private GameObject UIPause;


    [SerializeField] private bool Pause;

    User user = new User();

    public GameObject sueloPrefab;

    public Canvas UICanvas;

    public GameObject toboganPrefab;


    public void NextButton()
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




    void Answers()
    {
        GameObject canvasGameObject = new GameObject("Canvas");

        Canvas canvas = canvasGameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;



        GameObject textGameObject = new GameObject("TextMeshPro Text");
        TextMeshProUGUI tmpText = textGameObject.AddComponent<TextMeshProUGUI>();

        tmpText.text = "PREGUNTA";

        textGameObject.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform = textGameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(6, 218);



        GameObject textGameObject_1 = new GameObject("TextMeshPro Text");
        TextMeshProUGUI tmpText_1 = textGameObject_1.AddComponent<TextMeshProUGUI>();

        tmpText_1.text = "Respuesta 1";

        textGameObject_1.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform_1 = textGameObject_1.GetComponent<RectTransform>();
        rectTransform_1.anchoredPosition = new Vector2(-329, 100);



        GameObject textGameObject_2 = new GameObject("TextMeshPro Text");
        TextMeshProUGUI tmpText_2 = textGameObject_2.AddComponent<TextMeshProUGUI>();

        tmpText_2.text = "Respuesta 2";

        textGameObject_2.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform_2 = textGameObject_2.GetComponent<RectTransform>();
        rectTransform_2.anchoredPosition = new Vector2(15, 100);


        GameObject textGameObject_3 = new GameObject("TextMeshPro Text");
        TextMeshProUGUI tmpText_3 = textGameObject_3.AddComponent<TextMeshProUGUI>();

        tmpText_3.text = "Respuesta 3";

        textGameObject_3.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform_3 = textGameObject_3.GetComponent<RectTransform>();
        rectTransform_3.anchoredPosition = new Vector2(383, 100);


    }


    


    IEnumerator SendLevel(Level_info level)
    {
        WWWForm form = new WWWForm();
        form.AddField("level", level.name_level);
        using (UnityWebRequest www = UnityWebRequest.Post("http://pushi.pythonanywhere.com/level", form))
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

    IEnumerator SendScore(User user, int amountToAdd)
    {
        
        user.score += amountToAdd;

        Debug.Log("Score actualizado a " + user.score);
        WWWForm form = new WWWForm();
        form.AddField("score", user.score);

        using (UnityWebRequest www = UnityWebRequest.Post("http://pushi.pythonanywhere.com/update", form))
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
        // Level_SendApi level = new Level_SendApi(); RECUERDA ELIMINAR EL ARCHIVO SENDAPI

        int score = 0;

        

        user.score = score;

        int amountToAdd = 5; // Cantidad a sumar

        
        StartCoroutine(SendScore(user, amountToAdd));


        string name_level = "Nivel de tobogan";

        Level_info tobogan = new Level_info();

        tobogan.name_level = name_level;

        Debug.Log("Estas en el " + name_level);

        StartCoroutine(SendLevel(tobogan));


        GameObject suelo = Instantiate(sueloPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));

        suelo.transform.localScale = new Vector3(.4f, .4f, .4f);


        GameObject tobogan_1 = Instantiate(toboganPrefab, new Vector3(-300, -320, 594), Quaternion.Euler(0, 270, 0));

        tobogan_1.transform.localScale = new Vector3(20, 20, 20);

        GameObject tobogan_2 = Instantiate(toboganPrefab, new Vector3(0, -320, 594), Quaternion.Euler(0, 270, 0));

        tobogan_2.transform.localScale = new Vector3(20, 20, 20);


        GameObject tobogan_3 = Instantiate(toboganPrefab, new Vector3(300, -320, 594), Quaternion.Euler(0, 270, 0));

        tobogan_3.transform.localScale = new Vector3(20, 20, 20);



        Answers();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) {


            Pause = !Pause;

        }



        if (Pause)
        {
            Time.timeScale = 0;

            ActivateMenu();
        }

        else {

            Time.timeScale = 1;
            DeactivateMenu();
        }

    }

    void ActivateMenu() {

        UIPause.SetActive(true);


    }


    void DeactivateMenu() {

        UIPause.SetActive(false);
        Pause = false;
    }

}



