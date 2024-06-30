using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{

    public static Login Instance;

    public static string num_list_variable;

    public TMP_InputField num_lista;
    public TMP_InputField nombre;
    public TMP_InputField user_name;
    public TMP_InputField edad;
    public TMP_InputField sexo;



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator SendData(User user)
    {
        WWWForm form = new WWWForm();
        form.AddField("numero_de_lista", user.id);
        form.AddField("username", user.user_name);
        



        using (UnityWebRequest www = UnityWebRequest.Post("http://104.154.142.235:8080/loginGame", form))
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
                SceneManager.LoadScene("Inicio");
            }
        }
    }




    public void BotonLogin()
    {
        User user_pro = new User
        {
            id = int.Parse(num_lista.text),
            user_name = user_name.text,
            

        };

        Debug.Log($"Enviando datos: {user_pro.id}, {user_pro.user_name}");
        StartCoroutine(SendData(user_pro));

        num_list_variable = num_lista.text;
    }



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
