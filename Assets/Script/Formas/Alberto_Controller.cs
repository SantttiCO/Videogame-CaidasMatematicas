using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Alberto_Controller : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private float speed;
    [SerializeField] private float runningMultiplier = 1.5f;  // Factor de multiplicación de la velocidad al correr
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float jumpForce;
    [SerializeField] private BoxCollider ground;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip walk;
    [SerializeField] private AudioClip spin;
    [SerializeField] private AudioClip run;
    [SerializeField] private AudioClip dance1;
    [SerializeField] private AudioClip dance2;
    [SerializeField] private AudioClip dance3;


    private Vector2 movement = Vector2.zero;
    private Rigidbody rb;
    private bool grounded = false;
    private bool isWalking = false;
    private Animator animator;
    private AudioSource audioSource;
    private string currentAnim = "";
    private int currentIlde = 0;
    public Camera playerCamera;


    public void changeAnimation(string animation, float crossfade = .2f, float time = 0f)
    {
        if (time > 0) StartCoroutine(wait());
        else
            validate();
        
        IEnumerator wait()
        {
            yield return new WaitForSeconds(time-crossfade);
            validate();
        }

        void validate()
        {

            if (currentAnim != animation)
            {
                currentAnim = animation;
                animator.CrossFade(animation, crossfade);
            }
        }
    }

    private void checkIdle()
    {
        
        switch(currentIlde) 
        {
            case 0:
                changeAnimation("Idle");
                break;
            case 1:
                changeAnimation("Dwarf Idle");
                break;

        }

    }

    private void checkAnimation(bool isRunning)
    {
        if (currentAnim == "Jumping Up" || currentAnim == "marometa")
            return;

        if (movement.y == 1)
            changeAnimation(isRunning ? "Run" : "Walking");
        else if (movement.x == 1)
            changeAnimation(isRunning ? "Right Strafe" : "Right Strafe Walking");
        else if (movement.x == -1)
            changeAnimation(isRunning ? "Left Strafe" : "Left Strafe Walking");
        else
            checkIdle();

        // Manejar los sonidos de caminar y correr
        if (isRunning)
        {
            if (currentAnim == "Run" || currentAnim == "Right Strafe" || currentAnim == "Left Strafe")
            {
                if (audioSource.clip != run || !audioSource.isPlaying)
                {
                    audioSource.clip = run;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
        else if (!isRunning && (currentAnim == "Walking" || currentAnim == "Right Strafe Walking" || currentAnim == "Left Strafe Walking"))
        {
            if (audioSource.clip != walk || !audioSource.isPlaying)
            {
                audioSource.clip = walk;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            // Detener el sonido de caminar o correr si no está caminando ni corriendo
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }


    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        StartCoroutine(ChangeIdle());
        audioSource = GetComponent<AudioSource>();

        IEnumerator ChangeIdle()
        {
            while(true)
            {
                yield return new WaitForSeconds(5);
                ++currentIlde;
                if(currentIlde >= 2)
                    currentIlde = 0;
            }
        }
    }
    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && movement != Vector2.zero; // Correr con Shift

        // Actualizar velocidad basado en correr o caminar
        speed = isRunning ? movementSpeed * runningMultiplier : movementSpeed;

        // Get Mouse Movement
        Vector2 mouse = mouseSensitivity * Time.deltaTime * new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate Character and Camera
        transform.Rotate(mouse.x * Vector3.up);
        playerCamera.transform.Rotate(Vector3.left * mouseY);
        float currentXRotation = playerCamera.transform.localEulerAngles.x;
        if (currentXRotation > 180) currentXRotation -= 360;
        playerCamera.transform.localEulerAngles = new Vector3(Mathf.Clamp(currentXRotation, -90, 90), 0, 0);

        // Check for Jump
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            changeAnimation("Jumping Up");
            audioSource.PlayOneShot(jump, 1.0f);
            grounded = false;
        }

        if (grounded && Input.GetKeyDown(KeyCode.M))
        {
            changeAnimation("marometa");
            grounded = false;
            return;
        }

        if (!grounded && !(currentAnim == "Jumping Up" || currentAnim == "marometa"))
        {
            
            changeAnimation("Falling");

        }
        else
        {
            checkAnimation(isRunning);
        }

        

    }
    private void FixedUpdate()
    {

        //Get Velocity Direction
        Vector3 velocity = speed * (movement.x * transform.right + movement.y * transform.forward);
        //Apply Velocity ‘
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        //Update Grounded
        grounded = Physics.CheckBox(ground.transform.position + ground.center, 0.5f * ground.size, ground.transform.rotation, groundMask);
    }
}
