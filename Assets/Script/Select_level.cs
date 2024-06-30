using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class Select_level : MonoBehaviour
{

    public void Nivel_Cannon()
    {

        SceneManager.LoadScene("Cannon");


    }


    public void Menu()
    {

        SceneManager.LoadScene("Inicio");




    }

    public void Formas()
    {

        SceneManager.LoadScene("Formas");


    }


    public void toboganes()
    {

        SceneManager.LoadScene("Nivel_toboganes");




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
