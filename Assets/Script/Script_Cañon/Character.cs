using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public GameObject characterPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject body = Instantiate(characterPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        body.transform.parent = gameObject.transform; //=ROBOT.transform
        body.transform.localScale = new Vector3(50, 50, 50);
        body.name = "body";

        gameObject.transform.position = new Vector3(-250, 60, -100);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(-1F, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(1F, 0, 0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0, -1F);
        }
        if (Input.GetKey(KeyCode.W ) || Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0, 1F);
        }
    }
}
