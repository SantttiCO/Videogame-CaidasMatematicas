using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Tobogan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body.transform.parent = gameObject.transform; //=ROBOT.transform
        body.transform.localScale = new Vector3(50, 50, 50);
        body.name = "body";

        gameObject.transform.position = new Vector3(-279, 331, 1121);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(-5F, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(5F, 0, 0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0, -5F);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0, 5F);
        }
    }
}
