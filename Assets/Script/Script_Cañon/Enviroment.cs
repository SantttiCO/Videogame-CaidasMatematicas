//NIVEL CAÑON

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;


public class Enviroment : MonoBehaviour
{
    public bool hasCollided = false;
    public int questioncount = 0;
    private TextMeshProUGUI preguntaText, respuestaAText, respuestaBText, respuestaCText, UserScore, timeText;
    public int amountToAdd = 10;
    private string userNumLista;
    public Canvas UIscore, Uipreguntas;

    public AudioSource audioSource;

    public Character_Cannon characterCannon;

    private float timer = 10.0f, global_timer = 0f;
    private bool timerActive = true;

    //esta variable es la que te da el cronometro de cuanto tiempo tiene para iniciar

    //idea para el futuro, puedes poner un if en el Fire() para que despues de una racha sea más rápida


    private int correctAnswerIndex;

    public GameObject CannonPrefab;

    public GameObject cannon_ballPrefab;

    public GameObject sueloPrefab;


    public static int score;

    public static string name_level = "Nivel de Cañon";


    List<GameObject> ubiCannon = new List<GameObject>();


    public Canvas UICanvas;


    string time_string;

    [SerializeField] private GameObject UIPause;

    public bool Stop = false;

    private bool Pause = false;



    public void NextButton()
    {

        SceneManager.LoadScene("Formas");

    }


    public void PrevButton()
    {

        SceneManager.LoadScene("Toboganes");


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

        preguntaText.text = "PREGUNTA";

        preguntaText.alignment = TextAlignmentOptions.Center;
        preguntaText.fontSize = 40;
        TMP_FontAsset myFont = Resources.Load<TMP_FontAsset>("Assets/TextMesh Pro/Examples & Extras/Resources/Fonts & Materials/Electronic Highway Sign SDF.asset"); // Asegúrate de tener la ruta correcta
        preguntaText.font = myFont;

        textGameObject.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform = textGameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 450);
        rectTransform.sizeDelta = new Vector2(500, 100);



        GameObject textGameObject_1 = new GameObject("TextMeshPro Text");
        respuestaAText = textGameObject_1.AddComponent<TextMeshProUGUI>();

        respuestaAText.text = "Respuesta 1";
        respuestaAText.alignment = TextAlignmentOptions.Center;

        textGameObject_1.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform_1 = textGameObject_1.GetComponent<RectTransform>();
        rectTransform_1.anchoredPosition = new Vector2(-429, 210);
        rectTransform_1.sizeDelta = new Vector2(400, 100);




        GameObject textGameObject_2 = new GameObject("TextMeshPro Text");
        respuestaBText = textGameObject_2.AddComponent<TextMeshProUGUI>();

        respuestaBText.text = "Respuesta 2";

        respuestaBText.alignment = TextAlignmentOptions.Center;

        textGameObject_2.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform_2 = textGameObject_2.GetComponent<RectTransform>();
        rectTransform_2.anchoredPosition = new Vector2(40, 210);
        rectTransform_2.sizeDelta = new Vector2(400, 100);



        GameObject textGameObject_3 = new GameObject("TextMeshPro Text");
        respuestaCText = textGameObject_3.AddComponent<TextMeshProUGUI>();

        respuestaCText.text = "Respuesta 3";
        respuestaCText.alignment = TextAlignmentOptions.Center;

        textGameObject_3.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform_3 = textGameObject_3.GetComponent<RectTransform>();
        rectTransform_3.anchoredPosition = new Vector2(542, 210);
        rectTransform_3.sizeDelta = new Vector2(400, 100);



        //Score
        GameObject textScoreGameObject = new GameObject("ScoreTextMeshPro Text");
        UserScore = textScoreGameObject.AddComponent<TextMeshProUGUI>();
        UserScore.text = "" + score;
        UserScore.alignment = TextAlignmentOptions.Center;
        UserScore.fontStyle = FontStyles.Bold;
        UserScore.fontSize = 43;


        textScoreGameObject.transform.SetParent(canvas.transform, false);
        RectTransform rectTransformScore = textScoreGameObject.GetComponent<RectTransform>();
        rectTransformScore.anchoredPosition = new Vector2(-700, 400); // Configurar la posición del score adecuadamente


        //temporizador
        GameObject timerTextObject = new GameObject("TimerText");
        timeText = timerTextObject.AddComponent<TextMeshProUGUI>();

        timeText.fontSize = 48;
        timeText.color = Color.red; // Color del texto
        timerTextObject.transform.SetParent(canvas.transform, false);
        RectTransform timerRect = timerTextObject.GetComponent<RectTransform>();
        timerRect.anchoredPosition = new Vector2(20, 300); // Posición en la pantalla
        timerRect.sizeDelta = new Vector2(200, 100); // Tamaño del texto

        return canvas;


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




    IEnumerator SendScore(string list_user, int score, string levelName)
    {
        SaveGameTime();

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


    IEnumerator GetQuestionFromServer()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://104.154.142.235:8080/Cannon_question"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener la pregunta: " + www.error);
            }
            else
            {
                QuestionData question = JsonUtility.FromJson<QuestionData>(www.downloadHandler.text);
                preguntaText.text = question.pregunta;
                respuestaAText.text = question.respuesta_a;
                respuestaBText.text = question.respuesta_b;
                respuestaCText.text = question.respuesta_c;
                correctAnswerIndex = question.correct_answer;
                Debug.Log("pregunta: " + question.pregunta);
                Debug.Log(" respuesta correcta" + correctAnswerIndex);
            }
        }
    }

    IEnumerator Unfinish(string list_user, string levelName)
    {
        WWWForm form = new WWWForm();
        form.AddField("numero_de_lista", list_user);
        form.AddField("name_level", levelName);

        using (UnityWebRequest www = UnityWebRequest.Post("http://104.154.142.235:8080/unfinish_level", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al actualizar el tiempo: " + www.error);
            }
            else
            {
                Debug.Log("Tiempo actualizado con éxito para " + levelName);
            }
        }
    }

    IEnumerator Finish(string list_user, string levelName)
    {
        WWWForm form = new WWWForm();
        form.AddField("numero_de_lista", list_user);
        form.AddField("name_level", levelName);

        using (UnityWebRequest www = UnityWebRequest.Post("http://104.154.142.235:8080/finish_level", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al actualizar el tiempo: " + www.error);
            }
            else
            {
                Debug.Log("Tiempo actualizado con éxito para " + levelName);
            }
        }
    }


    public void TriggerStopFromCollision()
    {
        Stop = true;
        HideUI();
        Time.timeScale = 0;
        UIPause.SetActive(false); // No muestra el UI de pausa cuando se detiene por colisión
    }



    // Start is called before the first frame update
    void Start()
    {
        UICanvas.transform.GetChild(1).gameObject.SetActive(false);
        UICanvas.transform.GetChild(2).gameObject.SetActive(false);

        Stop = false;

        audioSource = GetComponent<AudioSource>();


        User user = new User();


        Debug.Log("Estas en el " + name_level);


        userNumLista = Login.num_list_variable;
        Debug.Log("Usuario: " + userNumLista);
        //StartCoroutine(SendLevel(cannon));

        //int score = 0;





        GameObject cannon_1 = Instantiate(CannonPrefab, new Vector3(-327, -8, 60), Quaternion.Euler(0, 270, 15));
        cannon_1.transform.localScale = new Vector3(.9f, .9f, .9f);
        ubiCannon.Add(cannon_1);

        Rigidbody rb_1 = cannon_1.GetComponent<Rigidbody>();
        if (rb_1 == null)
        {
            rb_1 = cannon_1.AddComponent<Rigidbody>();
        }
        rb_1.useGravity = false;  // No afectado por la gravedad
        rb_1.isKinematic = true;  // No responde a las fuerzas físicas, permanece estático


        GameObject cannon_2 = Instantiate(CannonPrefab, new Vector3(-240, -8f, 60), Quaternion.Euler(0, 270, 15));
        cannon_2.transform.localScale = new Vector3(.9f, .9f, .9f);
        ubiCannon.Add(cannon_2);

        Rigidbody rb_2 = cannon_1.GetComponent<Rigidbody>();
        if (rb_2 == null)
        {
            rb_2 = cannon_1.AddComponent<Rigidbody>();
        }
        rb_2.useGravity = false;  // No afectado por la gravedad
        rb_2.isKinematic = true;  // No responde a las fuerzas físicas, permanece estático


        GameObject cannon_3 = Instantiate(CannonPrefab, new Vector3(-153, -8f, 60), Quaternion.Euler(0, 270, 15));
        cannon_3.transform.localScale = new Vector3(.9f, .9f, .9f);
        ubiCannon.Add(cannon_3);

        Rigidbody rb_3 = cannon_1.GetComponent<Rigidbody>();
        if (rb_3 == null)
        {
            rb_3 = cannon_1.AddComponent<Rigidbody>();
        }
        rb_3.useGravity = false;  // No afectado por la gravedad
        rb_3.isKinematic = true;  // No responde a las fuerzas físicas, permanece estático



        GameObject suelo = Instantiate(sueloPrefab, new Vector3(-214, -36, 0), Quaternion.Euler(0, 270, 0));
        suelo.transform.localScale = new Vector3(.3f, .3f, .3f);

        Rigidbody rb = cannon_1.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = cannon_1.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;  // No afectado por la gravedad
        rb.isKinematic = true;  // No responde a las fuerzas físicas, permanece estático

        Uipreguntas = Answers();

        StartCoroutine(GetQuestionFromServer());



    }
    IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (characterCannon != null)
        {
            characterCannon.canMove = true;
        }
    }

    IEnumerator CheckCollisionAndScore()
    {
        yield return new WaitForSeconds(.5f);  // Esperar 1 segundo

        if (!hasCollided)  // Verificar si no hubo colisión
        {
            score += amountToAdd;  // Sumar puntos
            Debug.Log("Score actualizado a " + score);
            UserScore.text = "Score: " + score;
            DesactivarYActivarDespuesDeTiempo(UICanvas.transform.GetChild(1).gameObject, 1.5f);
        }
        questioncount += 1;
        hasCollided = false;  // Restablecer la bandera de colisión para la próxima verificación
        characterCannon.canMove = true;  // Permitir que el personaje se mueva de nuevo
        Debug.Log(questioncount);
    }

    IEnumerator DesactivarYActivarDespuesDeTiempo(GameObject item, float tiempoE)
    {
        // Desactiva los cuadrados
        item.SetActive(true);

        // Espera el tiempo especificado
        yield return new WaitForSeconds(tiempoE);

        // Activa los cuadrados nuevamente
        item.SetActive(false);
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

    // Update is called once per frame
    void Update()
    {
        HandlePause();

        if (Time.timeScale == 1)
        {
            global_timer += Time.deltaTime;
        }
        if (questioncount < 10)
        {
            if (timerActive)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    characterCannon.canMove = false;
                    Fire();
                    StartCoroutine(EnableMovementAfterDelay(.5f)); // Iniciar la corutina con 0.5 segundos de retraso para permitir movimiento
                    timer = 10; // Reiniciar el timer

                    int num = (int)global_timer;
                    Debug.Log(num);

                    StartCoroutine(GetQuestionFromServer());

                    StartCoroutine(CheckCollisionAndScore());

                }

                int minutes = Mathf.FloorToInt(timer / 60);
                int seconds = Mathf.FloorToInt(timer % 60);
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }

        }
        else
        {
            StartCoroutine(SendScore(Login.num_list_variable, score, Enviroment.name_level));
            SceneManager.LoadScene("Winner_Cannon");
            StartCoroutine(Finish(Login.num_list_variable, Enviroment.name_level));

        }

    }


    void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause = !Pause;
        }

        if (Pause || Stop)
        {
            HideUI();
            Time.timeScale = 0;

            if (Pause)
            {
                UIPause.SetActive(true);
            }
        }
        else
        {
            ShowUI();
            if (!Stop)
            {
                Time.timeScale = 1;
                UIPause.SetActive(false);
            }
        }
    }

    void Fire()
    {
        for (int i = 0; i < ubiCannon.Count; i++)
        {
            if (i == correctAnswerIndex - 1) continue; // Ignora el cañón que no debe disparar

            GameObject cannonBall = GameObject.CreatePrimitive(PrimitiveType.Sphere); // Crea una esfera
            cannonBall.transform.position = ubiCannon[i].transform.position + new Vector3(0, 1, -40); // Posiciona la bala de cañón
            cannonBall.transform.localScale = new Vector3(11f, 11f, 11f); // Establece el tamaño de la bala de cañón

            SphereCollider collider = cannonBall.GetComponent<SphereCollider>();
            collider.radius = 0.5f; // Ajusta el radio del collider, si es necesario

            Rigidbody rb = cannonBall.AddComponent<Rigidbody>(); // Añade un Rigidbody
            rb.useGravity = true; // Activa la gravedad
            rb.mass = 1; // Establece la masa
            rb.isKinematic = false; // Asegura que responda a la física
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Para una mejor detección de colisiones

            cannonBall.tag = "CannonBall"; // Asigna el tag

            // Añade el script Cannonball que maneja las colisiones
            cannonBall.AddComponent<Cannonball>();

            Cannonball cannonballComponent = cannonBall.AddComponent<Cannonball>();

            cannonballComponent.environment = this;

            rb.AddForce(0, 0, -35000); // Añade fuerza para disparar la bala de cañón

            Destroy(cannonBall, 2); // Destruye la bala de cañón después de 2 segundos

            // Reproduce el sonido de disparo, si hay un AudioSource y un AudioClip disponibles
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogError("AudioSource o AudioClip no existente en el cañón " + i);
            }
        }
    }

    public class Cannonball : MonoBehaviour
    {
        public Enviroment environment;  // Referencia a Environment
        private Animator animatorAlberto;

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Character")
            {
                animatorAlberto = collision.gameObject.GetComponent<Animator>();
                animatorAlberto.CrossFade("Falling Back Death", 0);

                if (environment != null)
                {
                    environment.hasCollided = true;
                    environment.StartCoroutine(environment.SendScore(Login.num_list_variable, score, Enviroment.name_level));


                    Debug.Log("Cannonball colisionó con: " + collision.gameObject.name);
                    Debug.Log("Nivel:" + Enviroment.name_level);
                    Debug.Log("Numero de lista" + Login.num_list_variable);
                    environment.StartCoroutine(environment.Unfinish(Login.num_list_variable, Enviroment.name_level));
                    SceneManager.LoadScene("Formas");
                }
            }
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