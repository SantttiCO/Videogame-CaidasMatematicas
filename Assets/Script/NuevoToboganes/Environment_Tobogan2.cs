using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Video;
public class Enviroment_Tobogan2 : MonoBehaviour
{
    public VideoPlayer videoPlayer2;
    private string userNumLista;
    [SerializeField] private GameObject UIPause;
    [SerializeField] private bool Pause;
    public nextlevelController videoPlayerController;
    public int variableToboganesInterna = 0;
    public Canvas UICanvas;

    public Canvas Uipreguntas;
    public TextMeshProUGUI preguntaText;
    public TextMeshProUGUI Score_text;
    public TextMeshPro R1;
    public TextMeshPro R2;
    public TextMeshPro R3;
    private string pregunta;
    public int correctAnswerIndex;
    User user = new User();
    public int currentQuestion = 1;
    public static int score;
    public static string name_level = "Nivel de Tobogan";
    private float global_timer = 0f;
    string time_string;
    public int scoreToboganes = score;
    public void NextButton()
    {
        SceneManager.LoadScene("Cannon");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Inicio");
    }

    Canvas Answers()
    {
        GameObject canvasGameObject = new GameObject("Canvas");
        Canvas canvas = canvasGameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject textGameObject = new GameObject("TextMeshPro Text");
        preguntaText = textGameObject.AddComponent<TextMeshProUGUI>();
        preguntaText.fontSize = 60;
        preguntaText.text = pregunta;  //PREGUNTA EN EL CANVAS_______ 
        textGameObject.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform = textGameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 450);
        preguntaText.alignment = TextAlignmentOptions.Center;
        preguntaText.autoSizeTextContainer = true;
        preguntaText.fontWeight = TMPro.FontWeight.Bold;
        preguntaText.enableWordWrapping = false;
//____________________________________________________________________________________________
        GameObject textGameObject_4 = new GameObject("TextMeshPro Text Score");
        Score_text = textGameObject_4.AddComponent<TextMeshProUGUI>();
        //Score_text.text = scoreToboganes.ToString();
        
        textGameObject_4.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform_4 = textGameObject_4.GetComponent<RectTransform>();
        rectTransform_4.anchoredPosition = new Vector2(-700, 450);
        rectTransform_4.sizeDelta = new Vector2(300, 100);
        return canvas;
    }


    
    public string GetScoreToboganesAsString()
    {
        return scoreToboganes.ToString();
    }
    public IEnumerator SendScore(string list_user, int score, string levelName)
    {
        SaveGameTime();
        Debug.Log(time_string);
        Debug.Log("Tiempo de juego prueba savegame:" + time_string);
        Debug.Log("Número de lista " + list_user);
        Debug.Log("Score actualizado a " + score);
        Debug.Log("Nivel :" + levelName);
        WWWForm form = new WWWForm();
        form.AddField("numero_de_lista", list_user);
        form.AddField("puntaje", score);
        form.AddField("name_level", levelName);
        form.AddField("global_timer", time_string);


        using (UnityWebRequest www = UnityWebRequest.Post("http://104.154.142.235:8080/update_score", form))
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

    public IEnumerator GetQuestionFromServer()
    {
        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Get("http://104.154.142.235:8080/Tobogan_question"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener la pregunta: " + www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                QuestionData question = JsonUtility.FromJson<QuestionData>(www.downloadHandler.text);

                pregunta = question.pregunta;
                Debug.Log(pregunta);
                preguntaText.text = pregunta;
                R1.text = question.respuesta_a;
                R2.text = question.respuesta_b;
                R3.text = question.respuesta_c;
                correctAnswerIndex = question.correct_answer;
                CanvasRespuestas canvasRespuestas = GameObject.Find("CanvasRespuestas").GetComponent<CanvasRespuestas>();
                if (canvasRespuestas != null)
                {
                    canvasRespuestas.UpdateAnswers(R1.text, R2.text, R3.text);
                }
            }
        }
    }

    void Start()
    {
        //videoPlayer2.GetComponent<Renderer>().sortingOrder = 1;
        videoPlayer2.gameObject.SetActive(false);
        scoreToboganes = 0;
        Uipreguntas = Answers();
        StartCoroutine(GetQuestionFromServer());
        videoPlayerController = FindObjectOfType<nextlevelController>();
        userNumLista = Login.num_list_variable;
        Debug.Log("Num lista: " + userNumLista);
        Level_info tobogan = new Level_info();
        tobogan.name_level = name_level;
        Debug.Log("Estas en el " + name_level);

        UICanvas.gameObject.SetActive(false);

        Character_Tobogan2 character = GameObject.Find("Character").GetComponent<Character_Tobogan2>();
        character.SetEnvironmentReference(this);
        Uipreguntas.enabled = false;
    }

    public void ShowUI()
    {
        if (Uipreguntas != null)
        {
            Uipreguntas.gameObject.SetActive(true);
        }
    }

    public void HideUI()
    {
        if (Uipreguntas != null)
        {
            Uipreguntas.gameObject.SetActive(false);
        }
    }



    public string FormatTime(float timeInSeconds)
    {
        int seconds = (int)timeInSeconds;  // Convierte el tiempo de float a segundos enteros

        // Devuelve el tiempo formateado solo en segundos
        return seconds.ToString();
    }


    string SaveGameTime()
    {
        time_string = FormatTime(global_timer);
        Debug.Log("Guardando tiempo de juego: " + time_string);
        return time_string;
    }

    public void showVideoEnd()
    {
        videoPlayer2.gameObject.SetActive(true);
        Debug.Log("show video");
    }
    void Update()
    {
        if (Time.timeScale == 1)
        {
            global_timer += Time.deltaTime;
        }

        HandlePause();
        if (Input.GetKeyDown(KeyCode.S))
        {
            Uipreguntas.enabled = true;
        }
        if (variableToboganesInterna > 2)
        {
            showVideoEnd();
            HideUI();
            SceneManager.LoadScene("Cannon");
        }

    }

   

    void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause = !Pause;
        }
        if (Pause)
        {
            HideUI();
            Time.timeScale = 0;
            UIPause.SetActive(true);
        }
        else
        {
            ShowUI();
            Time.timeScale = 1;
            UIPause.SetActive(false);
        }
    }

    [System.Serializable]
    public class QuestionData
    {
        public string pregunta;
        public string respuesta_a;
        public string respuesta_b;
        public string respuesta_c;
        public int correct_answer;
    }

}