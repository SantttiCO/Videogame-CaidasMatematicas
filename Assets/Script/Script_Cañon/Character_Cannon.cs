using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Cannon : MonoBehaviour
{
    public GameObject characterPrefab;
    private GameObject body;
    private float[] allowedPositions = new float[] { -44, 38, 120 }; // Posiciones permitidas
    private int currentPositionIndex = 1; // Inicia en la posición central
    public bool canMove = true;

    void Start()
    {
        // Suponiendo que body es tu personaje
        if (body == null)
        {
            body = Instantiate(characterPrefab, new Vector3(-240, -25, 0), Quaternion.identity);
            body.transform.localScale = new Vector3(20, 20, 20);
            body.transform.parent = transform;
            body.name = "Body";
            body.tag = "Character";
        }

        // Añadir un Collider
        CapsuleCollider collider = body.AddComponent<CapsuleCollider>();
        collider.height = 2.0f; // Ajustar según la escala y forma de tu personaje
        collider.radius = 0.5f; // Ajustar según la escala y forma de tu personaje
        collider.center = new Vector3(0, 1f, 0);
        // Opcional: Añadir un Rigidbody si quieres que tu personaje responda a físicas
        Rigidbody rb = body.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false; // Permitir que la física influya en el objeto
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;


    }


    void Update()
    {
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(-1); // Mover a la izquierda
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(1); // Mover a la derecha
            }
        }
    }

    private void Move(int direction)
    {
        currentPositionIndex = Mathf.Clamp(currentPositionIndex + direction, 0, allowedPositions.Length - 1);
        transform.position = new Vector3(allowedPositions[currentPositionIndex], transform.position.y, transform.position.z);
    }



}