using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class button_level : MonoBehaviour
{
    private string userNumLista;
    public TextMeshProUGUI puntaje_1, nombre_nivel_1, puntaje_2, nombre_nivel_2, puntaje_3, nombre_nivel_3;

    public TextMeshProUGUI scoreTotal;



    public void LevelScene()
    {

        SceneManager.LoadScene("Level_Selector");


    }



    IEnumerator GetToboganScore(string numeroDeLista)
    {
        WWWForm form = new WWWForm();
        form.AddField("numero_de_lista", numeroDeLista);

        using (UnityWebRequest www = UnityWebRequest.Post("http://104.154.142.235:8080/get_tobogan_level_score", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener el puntaje: " + www.error);
            }
            else
            {
                var scoreResponse = JsonUtility.FromJson<ScoreResponse>(www.downloadHandler.text);
                puntaje_1.text = "Puntaje: " + scoreResponse.puntaje;
                nombre_nivel_1.text = "Nivel: " + scoreResponse.nombre_nivel;
            }
        }
    }

    IEnumerator GetCannonScore(string numeroDeLista)
    {
        WWWForm form = new WWWForm();
        form.AddField("numero_de_lista", numeroDeLista);

        using (UnityWebRequest www = UnityWebRequest.Post("http://104.154.142.235:8080/get_cannon_level_score", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener el puntaje: " + www.error);
            }
            else
            {
                var scoreResponse = JsonUtility.FromJson<ScoreResponse>(www.downloadHandler.text);
                puntaje_2.text = "Puntaje: " + scoreResponse.puntaje;
                nombre_nivel_2.text = "Nivel: " + scoreResponse.nombre_nivel;
            }
        }
    }

    IEnumerator GetFigurasScore(string numeroDeLista)
    {
        WWWForm form = new WWWForm();
        form.AddField("numero_de_lista", numeroDeLista);

        using (UnityWebRequest www = UnityWebRequest.Post("http://104.154.142.235:8080/get_figuras_level_score", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener el puntaje: " + www.error);
            }
            else
            {
                var scoreResponse = JsonUtility.FromJson<ScoreResponse>(www.downloadHandler.text);
                puntaje_3.text = "Puntaje: " + scoreResponse.puntaje;
                nombre_nivel_3.text = "Nivel: " + scoreResponse.nombre_nivel;
            }
        }
    }



    IEnumerator GetPlayerScore(string idJugador)
    {
        WWWForm form = new WWWForm();
        form.AddField("id_jugador", idJugador);

        using (UnityWebRequest www = UnityWebRequest.Post("http://104.154.142.235:8080/get_player_score", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener el puntaje total: " + www.error);
            }
            else
            {
                // Assuming the response JSON key for the score is "puntaje"
                PlayerScoreResponse scoreResponse = JsonUtility.FromJson<PlayerScoreResponse>(www.downloadHandler.text);
                scoreTotal.text = "Puntaje Total: " + scoreResponse.puntaje; // Update the UI with the fetched score
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        userNumLista = Login.num_list_variable;

        StartCoroutine(GetToboganScore(userNumLista));
        StartCoroutine(GetCannonScore(userNumLista));
        StartCoroutine(GetFigurasScore(userNumLista));
        StartCoroutine(GetPlayerScore(userNumLista));

    }

    // Update is called once per frame
    void Update()
    {

    }


    [System.Serializable]
    public class ScoreResponse
    {
        public int puntaje;
        public string nombre_nivel;
    }

    [System.Serializable]
    public class PlayerScoreResponse
    {
        public int puntaje; // Ensure this matches the JSON response key from the server
    }

}
