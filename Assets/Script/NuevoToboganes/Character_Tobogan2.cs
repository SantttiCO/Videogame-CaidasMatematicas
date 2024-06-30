using System.Collections;
using UnityEngine;

public class Character_Tobogan2 : MonoBehaviour
{
    private string userNumLista;
    CameraFade fade;
    public Transform t;
    private float duracionIgnorarColision = 4.5f;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip walk;
    [SerializeField] private AudioClip spin;
    private Animator animator;
    private string currentAnim = "";
    private int currentIlde = 0;
    private Enviroment_Tobogan2 environment;

    public void SetEnvironmentReference(Enviroment_Tobogan2 env)
    {
        environment = env;
    }

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

    void Start()
    {
        userNumLista = Login.num_list_variable;
        fade = FindObjectOfType<CameraFade>();
        GameObject character = GameObject.Find("Character");
        character.transform.position = new Vector3(2, 163, -985);
        animator = GetComponent<Animator>();
        StartCoroutine(ChangeIdle());
        t = transform;

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

    void LimitMovement()
    {
        float minX = -420f;
        float maxX = 420f;
        float minZ = -1078f;
        float maxZ = -530f;

        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);
        transform.position = newPosition;
    }

    

    void OnCollisionEnter(Collision col)
    {
        Collider character = GameObject.FindWithTag("Player")?.GetComponent<Collider>();
        Collider tobogan1Collider = GameObject.FindWithTag("Tobogan")?.GetComponent<Collider>();
        Collider tobogan2Collider = GameObject.FindWithTag("Tobogan2")?.GetComponent<Collider>();
        Collider tobogan3Collider = GameObject.FindWithTag("Tobogan3")?.GetComponent<Collider>();

        if (col.gameObject.tag == "Tobogan")
        {
            Physics.IgnoreCollision(character, tobogan2Collider, true);
            StartCoroutine(IgnoreCollisionForSeconds(col.collider));
            ToboganUno();
            Physics.IgnoreCollision(character, tobogan2Collider, false);
            if (environment.correctAnswerIndex == 1)
            {
                environment.scoreToboganes = environment.scoreToboganes +10;
                environment.Score_text.text=environment.scoreToboganes.ToString();
                StartCoroutine(environment.SendScore(userNumLista,environment.scoreToboganes,"Nivel de Tobogan"));
                StartCoroutine(environment.GetQuestionFromServer());
                environment.variableToboganesInterna =  environment.variableToboganesInterna + 1;
            }
            else
            {
                if (environment.scoreToboganes > 0)
                {
                    environment.scoreToboganes = environment.scoreToboganes -2;
                    environment.Score_text.text=environment.scoreToboganes.ToString();
                    StartCoroutine(environment.SendScore(userNumLista,environment.scoreToboganes,"Nivel de Tobogan"));
                }
            }
        }

        if (col.gameObject.tag == "Tobogan2")
        {
            ToboganDos();
            StartCoroutine(IgnoreCollisionForSeconds(col.collider));
            if (environment.correctAnswerIndex == 2)
            {
                environment.scoreToboganes = environment.scoreToboganes +10;
                environment.Score_text.text=environment.scoreToboganes.ToString();
                StartCoroutine(environment.SendScore(userNumLista,environment.scoreToboganes,"Nivel de Tobogan"));
                StartCoroutine(environment.GetQuestionFromServer());
                environment.variableToboganesInterna =  environment.variableToboganesInterna + 1;
            }
            else
            {
                if (environment.scoreToboganes > 0)
                {
                    environment.scoreToboganes = environment.scoreToboganes -2;
                    environment.Score_text.text=environment.scoreToboganes.ToString();
                    StartCoroutine(environment.SendScore(userNumLista,environment.scoreToboganes,"Nivel de Tobogan"));
                }
            }
        }
        if (col.gameObject.tag == "Tobogan3")
        {
            Physics.IgnoreCollision(character, tobogan2Collider, true);
            StartCoroutine(IgnoreCollisionForSeconds(col.collider));
            ToboganTres();
            Physics.IgnoreCollision(character, tobogan2Collider, false);
            if (environment.correctAnswerIndex == 3)
            {
                environment.scoreToboganes = environment.scoreToboganes +10;
                environment.Score_text.text=environment.scoreToboganes.ToString();
                StartCoroutine(environment.SendScore(userNumLista,environment.scoreToboganes,"Nivel de Tobogan"));
                StartCoroutine(environment.GetQuestionFromServer());
                environment.variableToboganesInterna = environment.variableToboganesInterna + 1;
            }
            else
            {
                if (environment.scoreToboganes > 0)
                {
                    environment.scoreToboganes = environment.scoreToboganes -2;
                    environment.Score_text.text=environment.scoreToboganes.ToString();
                    StartCoroutine(environment.SendScore(userNumLista,environment.scoreToboganes,"Nivel de Tobogan"));
                }
            }
        }
    }


    IEnumerator IgnoreCollisionForSeconds(Collider otherCollider)
    {
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), otherCollider);
        yield return new WaitForSeconds(duracionIgnorarColision);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), otherCollider, false);
    }

    void Update()
    {
        GeneralMovements();
        t.Translate(Time.deltaTime, 0f, 0f);
    }

    void ToboganUno()
    {
        StartCoroutine(ToboganMove(new Vector3(-323, 200, -682)));
    }

    void ToboganDos()
    {
        StartCoroutine(ToboganMove(new Vector3(15, 200, -682)));
    }

    void ToboganTres()
    {
        StartCoroutine(ToboganMove(new Vector3(380, 200, -682)));
    }

    IEnumerator ToboganMove(Vector3 targetPosition)
    {
        ToboganGeneral();
        transform.position = targetPosition;
        yield return fade.FadeTotal();
    }


    void ToboganGeneral()
    {
        changeAnimation("Jumping Up");
        StartCoroutine(MoveInSequence());
    }

    IEnumerator MoveInSequence()
    {
        Vector3[] sequence = new Vector3[]{
            new Vector3(0f, 40f, 140f),
            new Vector3(0f, -210f, 350f),
            new Vector3(0f, -100f, 250f),
            new Vector3(0f, -20f, 350f),
            new Vector3(0f, 0f, 250f)
        };

        float duration = 0.4f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition;

        foreach (Vector3 move in sequence)
        {
            endPosition = startPosition + move;

            float elapsedTime = 0.0001f;
            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = endPosition;
            startPosition = endPosition;
        }

        Vector3 finalPosition = new Vector3(2, 163, -985);
        float finalDuration = 1.0f;
        Vector3 initialPosition = transform.position;

        float finalElapsedTime = 0.0f;
        while (finalElapsedTime < finalDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, finalPosition, finalElapsedTime / finalDuration);
            finalElapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = finalPosition;
    }

    void GeneralMovements()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            changeAnimation("Left Strafe Walking");
            gameObject.transform.position = gameObject.transform.position + new Vector3(-1F, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            changeAnimation("Right Strafe Walking");
            gameObject.transform.position = gameObject.transform.position + new Vector3(1F, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            changeAnimation("Walking Backwards");
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0, -1F);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            changeAnimation("Walking");
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0, 1F);
        }

        LimitMovement();
    }

}