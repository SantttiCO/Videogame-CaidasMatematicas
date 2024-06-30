using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{

    public static int score;
    public static string name_level = "Nivel de Figuras";
    private float global_timer = 0f;
    string time_string;

    private bool isTimerActive = false;

    private string Pregunta2;
    private string Pregunta3;

    public Cuadrito[] gridCubos; // Array lineal de cuadritos
    public int gridWidth = 8; // Ancho de la cuadrícula
    public int gridHeight = 8; // Altura de la cuadrícula

    private Cuadrito[,] gridMatrix; // Matriz 2D de cuadritos
    public bool[,] RespuestaCuadrados; // Matriz de la figura deseada




    public Cuadrito[] gridTriangulos; // Array lineal de cuadritos
    private bool[] RespuestaTriangulos = new bool[120];

    public CuadritoVol[] gridTrianVol;
    private bool[] RespuestaTriangulosVol = new bool[120];


    private string userNumLista;
    public TMP_Text timerText; // Referencia al TextMeshPro para el temporizador
    public TMP_Text timerText2; // Referencia al TextMeshPro para el temporizador
    public TMP_Text PreguntasText;
    public TMP_Text scoreText;
    private float countdownTimer = 100.0f;
    private float timeElapsed = 0.0f;
    private int nivel = 0;
    private bool isTransitioning = false;
    private Vector3 pantallita;

    [SerializeField] public Canvas logrado;
    [SerializeField] public Canvas acabado;

    [SerializeField] private GameObject triangulos;
    [SerializeField] private GameObject cuadrados;
    [SerializeField] private GameObject triangulos3D;
    [SerializeField] private GameObject cuadrados3D;
    [SerializeField] private GameObject pantalla;



    void Start()
    {
        userNumLista = Login.num_list_variable;
        Debug.Log("Num lista: " + userNumLista);
        logrado.transform.GetChild(0).gameObject.SetActive(false);
        logrado.transform.GetChild(1).gameObject.SetActive(false);
        logrado.transform.GetChild(2).gameObject.SetActive(false);
        logrado.transform.GetChild(3).gameObject.SetActive(false);
        logrado.transform.GetChild(4).gameObject.SetActive(false);
        logrado.transform.GetChild(5).gameObject.SetActive(false);
        acabado.transform.GetChild(0).gameObject.SetActive(true);
        acabado.transform.GetChild(1).gameObject.SetActive(false);
        pantallita = pantalla.transform.position;
        gridMatrix = new Cuadrito[gridWidth, gridHeight];
        for (int i = 0; i < gridCubos.Length; i++)
        {
            int row = i / gridWidth;
            int col = i % gridWidth;
            gridMatrix[row, col] = gridCubos[i];
        }

        StartCoroutine(GetQuestionFromServer());


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

    IEnumerator GetQuestionFromServer()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://104.154.142.235:8080/Formas_question"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener la pregunta: " + www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Respuesta recibida correctamente: " + responseText); // Agrega este mensaje de depuración

                // Procesar la respuesta JSON
                QuestionList questionsResponse = JsonUtility.FromJson<QuestionList>(responseText);

                List<Question> filteredQuestions1 = questionsResponse.preguntas.FindAll(q => q.tipo == 1);

                if (filteredQuestions1.Count > 0)
                {
                    // Seleccionar una pregunta al azar de las filtradas
                    int randomIndex = Random.Range(0, filteredQuestions1.Count);
                    Question selectedQuestion = filteredQuestions1[randomIndex];

                    // Convertir la matriz de la pregunta en la matriz de respuesta
                    RespuestaTriangulos = ConvertStringToBoolArray(selectedQuestion.matriz);
                    PreguntasText.text = selectedQuestion.pregunta;

                }
                else
                {
                    Debug.LogError("No se encontraron preguntas del tipo 1.");
                    // Opcional: manejar el caso de que no hay preguntas del tipo 2
                }

                List<Question> filteredQuestions2 = questionsResponse.preguntas.FindAll(q => q.tipo == 2);

                if (filteredQuestions2.Count > 0)
                {
                    // Seleccionar una pregunta al azar de las filtradas
                    int randomIndex = Random.Range(0, filteredQuestions2.Count);
                    Question selectedQuestion2 = filteredQuestions2[randomIndex];

                    // Convertir la matriz de la pregunta en la matriz de respuesta
                    RespuestaCuadrados = ConvertStringToMatrix(selectedQuestion2.matriz, gridWidth, gridHeight);
                    Pregunta2 = selectedQuestion2.pregunta;


                }
                else
                {
                    Debug.LogError("No se encontraron preguntas del tipo 2.");
                    // Opcional: manejar el caso de que no hay preguntas del tipo 2
                }

                List<Question> filteredQuestions3 = questionsResponse.preguntas.FindAll(q => q.tipo == 3);

                if (filteredQuestions3.Count > 0)
                {
                    // Seleccionar una pregunta al azar de las filtradas
                    int randomIndex = Random.Range(0, filteredQuestions3.Count);
                    Question selectedQuestion3 = filteredQuestions3[randomIndex];

                    // Convertir la matriz de la pregunta en la matriz de respuesta
                    RespuestaTriangulosVol = ConvertStringToBoolArray(selectedQuestion3.matriz);
                    Pregunta3 = selectedQuestion3.pregunta;


                }
                else
                {
                    Debug.LogError("No se encontraron preguntas del tipo 3.");
                    // Opcional: manejar el caso de que no hay preguntas del tipo 2
                }


            }
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


    void Update()
    {
        movePantalla();

        if (Input.GetKeyDown(KeyCode.P)) // Verificar al presionar la barra espaciadora
        {
            CheckIfFiguraCompleta();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            CheckIfFiguraCompletaTriangulo();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            acabado.transform.GetChild(0).gameObject.SetActive(false);
            logrado.transform.GetChild(5).gameObject.SetActive(true);

        }
    }

    void FixedUpdate()
    {
        // Actualizar el tiempo transcurrido desde la última actualización
        timeElapsed += Time.fixedDeltaTime;

        // Verificar si ha pasado un segundo completo
        if (timeElapsed >= 1.0f)
        {
            // Restar un segundo del temporizador
            countdownTimer -= 1.0f;

            // Reiniciar el tiempo transcurrido
            timeElapsed = 0.0f;

            // Actualizar el texto del temporizador
            UpdateTimerText();
        }

        // Verificar si el temporizador ha llegado a cero
        if (countdownTimer <= 0)
        {
            // Realizar alguna acción cuando el temporizador llegue a cero
            Debug.Log("Temporizador terminado.");
            StartCoroutine(DesactivarYActivarDespuesDeTiempo(logrado.transform.GetChild(3).gameObject, 1.5f));
            switch (nivel)
            {
                case 0:
                    triangulos.SetActive(false);
                    break;
                case 1:
                    cuadrados.SetActive(false);
                    break;
                case 2:
                    LevelOver();
                    break;
                default:
                    Debug.Log("Opción no válida");
                    break;
            }


            ChangePlatform();
        }
    }

    public void LevelOver()
    {
        StartCoroutine(Finish(Login.num_list_variable, GameController.name_level));
        logrado.transform.GetChild(5).gameObject.SetActive(false);
        acabado.transform.GetChild(1).gameObject.SetActive(true);

        
    }

    void UpdateTimerText()
    {


        // Convertir el tiempo restante a un formato legible
        int seconds = Mathf.FloorToInt(countdownTimer);
        string timerString = string.Format("{0:00}", seconds);

        // Actualizar el texto del temporizador
        timerText.text = timerString;
        timerText2.text = timerString;


        if (countdownTimer <= 5)
        {
            timerText.color = Color.red;
            timerText2.color = Color.red;
        }
        else
        {
            // Si el temporizador no está en los últimos 5 segundos, restaurar el color original
            timerText.color = Color.white;
            timerText2.color = Color.white;
        }
    }

    void ChangePlatform()
    {
        countdownTimer = 100f;

        


        UpdateTimerText();
        nivel++;

        if (nivel == 1) // Si el nivel es impar, mostrar Pregunta2
        {
            PreguntasText.text = Pregunta2;
        }
        else if (nivel == 2) // Si el nivel es par, mostrar Pregunta3
        {
            PreguntasText.text = Pregunta3;
        }

        pantallita += new Vector3(0, -70f, 0);

        isTransitioning = true;
    }

    void movePantalla()
    {

        if (isTransitioning)
        {
            pantalla.transform.position = Vector3.MoveTowards(pantalla.transform.position, pantallita, Time.deltaTime * 40f);




            // Ajustar el tamaño del collider para que coincida con la altura actual
        }


    }

    void CheckIfFiguraCompleta()
    {
        PrintFiguraActualState();
        PrintGridMatrixState();

        bool figuraIncorrecta = false;
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                bool shouldHighlight = RespuestaCuadrados[i, j];
                bool isHighlighted = gridMatrix[i, j].IsHighlighted();

                // Verifica si los que deben estar resaltados lo están, y que los que no, no lo estén.
                if (shouldHighlight != isHighlighted)
                {
                    figuraIncorrecta = true;
                    break;
                }
            }
            if (figuraIncorrecta)
            {
                break;
            }
        }

        if (figuraIncorrecta)
        {
            Debug.Log("Figura incompleta");
            StartCoroutine(DesactivarYActivarDespuesDeTiempo(logrado.transform.GetChild(4).gameObject, 1.5f));

        }
        else
        {
            Debug.Log("Figura completada correctamente!");
            StartCoroutine(DesactivarYActivarDespuesDeTiempo(logrado.transform.GetChild(2).gameObject, 2f));

            cuadrados.SetActive(false);
            score = score + 100;
            scoreText.text = score.ToString();
            StartCoroutine(SendScore(Login.num_list_variable, score, name_level));

            ChangePlatform();



        }
    }

    void CheckIfFiguraCompletaTriangulo()
    {
        bool trianguloIncorrecto = false;
        for (int i = 0; i < gridTriangulos.Length; i++)
        {
            if (gridTriangulos[i].IsHighlighted() != RespuestaTriangulos[i])
            {
                trianguloIncorrecto = true; // Si alguna posición no coincide, no es la figura
                break;
            }
        }
        if (trianguloIncorrecto)
        {
            Debug.Log("Figura incompleta");
            StartCoroutine(DesactivarYActivarDespuesDeTiempo(logrado.transform.GetChild(4).gameObject, 1.5f));

        }
        else
        {
            Debug.Log("Figura triangulo completada correctamente!");
            StartCoroutine(DesactivarYActivarDespuesDeTiempo(logrado.transform.GetChild(1).gameObject, 2f));


            triangulos.SetActive(false);

            score = score + 100;
            scoreText.text = score.ToString();
            StartCoroutine(SendScore(Login.num_list_variable, score, name_level));


            ChangePlatform();


        }
    }





    void PrintFiguraActualState()
    {
        string figuraState = "Figura Actual State:\n";
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                figuraState += RespuestaCuadrados[i, j] ? "1 " : "0 ";
            }
            figuraState += "\n";
        }
        Debug.Log(figuraState);
    }


    void PrintGridMatrixState()
    {
        string matrixState = "Grid Matrix State:\n";
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                matrixState += gridMatrix[i, j].IsHighlighted() ? "1 " : "0 ";
            }
            matrixState += "\n";
        }
        Debug.Log(matrixState);
    }


    private bool[,] ConvertStringToMatrix(string matrixString, int rows, int cols)
    {
        bool[,] matrix = new bool[rows, cols];
        matrixString = matrixString.Trim(new char[] { '[', ']' }).Replace(" ", string.Empty);
        string[] rowsData = matrixString.Split(new string[] { "],[" }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < rows; i++)
        {
            string[] colData = rowsData[i].Split(',');
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = colData[j].Equals("true", System.StringComparison.OrdinalIgnoreCase);
            }
        }
        return matrix;
    }

    private bool[] ConvertStringToBoolArray(string matrixString)
    {
        // Eliminar los corchetes y dividir por comas
        matrixString = matrixString.Trim(new char[] { '[', ']' }).Replace(" ", string.Empty);
        string[] values = matrixString.Split(',');

        bool[] boolArray = new bool[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            boolArray[i] = bool.Parse(values[i]);
        }
        return boolArray;
    }


    [System.Serializable]
    public class Question
    {
        public string matriz;
        public string pregunta;
        public int tipo;
    }

    // Clase para deserializar la respuesta completa
    [System.Serializable]
    public class QuestionList
    {
        public List<Question> preguntas;
    }

    

}