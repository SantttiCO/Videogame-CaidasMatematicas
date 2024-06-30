using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Alberto_dos : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private BoxCollider ground;

    private bool grounded = false;

    [SerializeField] private AudioClip salsa;
    [SerializeField] private AudioClip hiphop;
    [SerializeField] private AudioClip gangma;
    [SerializeField] private AudioClip cuatro;

    private Animator animator;
    private string currentAnim = "";
    private int currentIlde = 0;
    private AudioSource audioSource;


    public void changeAnimation(string animation, float crossfade = .2f, float time = 0f)
    {
        if (time > 0) StartCoroutine(wait());
        else
            validate();

        IEnumerator wait()
        {
            yield return new WaitForSeconds(time - crossfade);
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
        switch (currentIlde)
        {
            case 0:
                changeAnimation("Idle");
                break;
            case 1:
                changeAnimation("Dwarf Idle");
                break;

        }

    }

  

    void Start()
    {
        
        animator = GetComponent<Animator>();
        StartCoroutine(ChangeIdle());

        audioSource = GetComponent<AudioSource>();

        IEnumerator ChangeIdle()
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                ++currentIlde;
                if (currentIlde >= 2)
                    currentIlde = 0;
            }
        }
    }
    void Update()
    {


        // Retorna si no está en el suelo para no procesar animaciones de baile

        if (grounded && Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("uno");
            changeAnimation("Salsa Dancing");
            audioSource.PlayOneShot(salsa);
            grounded = false;
        }
        else if (grounded && Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeAnimation("Snake Hip Hop Dance");
            grounded = false;
        }
        else if (grounded && Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeAnimation("Gangnam Style");
            grounded = false;
        }
        else if (grounded && Input.GetKeyDown(KeyCode.Alpha4))
        {
            changeAnimation("Arms Hip Hop Dance");
            grounded = false;
        }

        checkAnimation();


    }

    private void checkAnimation()
    {
        if (currentAnim == "Salsa Dancing" || currentAnim == "Snake Hip Hop Dance" || currentAnim == "Gangnam Style" || currentAnim == "Arms Hip Hop Dance")
            return;

        checkIdle();

        
    }


    private void FixedUpdate()
    {

        
        grounded = Physics.CheckBox(ground.transform.position + ground.center, 0.5f * ground.size, ground.transform.rotation, groundMask);
    }

}
