using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public CharacterController controller;
    [SerializeField]
    public PlayerInput playerInput;
    public Vector3 movement;

    [SerializeField]
    Animator animator;
    public float jumpSpeed = 0.08f;
    public Vector2 inputVector;
    public bool left, right = true;
    public bool jump;

    public int knittedItem = 0;


    GrappleScript move;
    yarnCount thing;

    [SerializeField]
    healthScript healthBar;
    GameObject platform;
    [SerializeField]
    Transform destination;
    bool throwing;

    bool pickUp = false;
    int canCreateItems;
    public int throwItems;
    private bool timerStart;
    float timer = 0f;
    bool platformIsMade;
    bool canPlaceDown;
    public int health = 3;
    public bool facingLeft, facingRight;
    bool moving;
    public bool playAnimation;
    int currentHealth;
    [SerializeField]
    AudioSource hitAudio;
    [SerializeField]
    AudioSource platformAudios;
    [SerializeField]
    AudioSource ballAudio;

    GameObject madePlatform;
    bool pickUpAgain;


    [SerializeField]
    Transform[] reSpawnLocations;

    [SerializeField]
    ParticleSystem walkEffect;

    [SerializeField]
    Text yarnText;
    public GameObject MadePlatform
    {
        get { return madePlatform; }
        set { madePlatform = value; }
    }
    public bool PickUpAgain
    {
        get
        {
            return pickUpAgain;
        }
        set
        {
            pickUpAgain = value;
        }
    }

    public int KnittedItem
    {
        get { return knittedItem; }
        set { knittedItem = value; }
    }
    public bool Moving
    {
        get { return moving; }
    }
    public bool Jumping
    {
        get { return jump; }
    }
    public Transform Destination
    {
        get { return destination; }
    }

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
        }
    }
    public bool PlatformIsMade
    {
        get { return platformIsMade; }
        set
        {
            platformIsMade = value;
        }
    }
    public bool CanPlaceDown
    {
        get { return canPlaceDown; }
        set
        {
            canPlaceDown = value;
        }
    }

    public bool Pickup
    {
        get { return pickUp; }
        set
        {
            pickUp = value;
        }
    }

    private void Awake()
    {
        // if we dont wanna add the inputaction to the player we do it like this:
        // PlayerInputActions playerInputActions = new PlayerInputActions();, playerInputActions.Player.Enable();, playerInputActions.Jump.Performed += Jump;
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        // animator = GetComponent<Animator>();
    }
    private void Start()
    {
        move = gameObject.GetComponent<GrappleScript>();
        healthBar.setMaxHealth(health);
        currentHealth = health;
        thing = GetComponent<yarnCount>();
    }


    public void Throw(InputAction.CallbackContext value)
    {
        if (platformIsMade)
        {
            platformIsMade = false;
            canPlaceDown = true;
            platformAudios.Play();
            if (right)
            {
                transform.localScale = new Vector3(transform.localScale.x - 0.003f, transform.localScale.y - 0.003f, transform.localScale.z - 0.003f);
            } else if (left)
            {
                transform.localScale = new Vector3(transform.localScale.x - 0.003f, transform.localScale.y - 0.003f, transform.localScale.z - 0.003f);

            }
        }
    }
    public void CreatePlatform(InputAction.CallbackContext value)
    {
        if (value.performed && knittedItem >= 1 && !platformIsMade)
        {
            platform = Resources.Load("KnittedPlatform") as GameObject;
            madePlatform = Instantiate(platform, new Vector3(destination.transform.position.x, destination.transform.position.y, 0), destination.rotation);
            Debug.Log(madePlatform);
            platformIsMade = true;
            knittedItem--;
            playAnimation = true;
            animator.SetBool("isKnitting", true);
            StartCoroutine(StopKnitting());
        }
    }

    IEnumerator StopKnitting()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isKnitting", false);

    }
    public void Movement(InputAction.CallbackContext value)
    {
        inputVector = value.ReadValue<Vector2>();
        moving = true;
    }

    public void Jump(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            movement = new Vector3(0, movement.y, 0);
            jump = true;
            playAnimation = false;
        }
    }

    private void FixedUpdate()
    {
        if(Mathf.Abs(inputVector.x) > 0.01f)
        {
            walkEffect.Play();
        }
        if (controller.isGrounded && movement.y < 0)
        {
            movement.y = 0f;
        }
        if (move.state == GrappleScript.State.Normal)
        {
            controller.Move(new Vector3(inputVector.x * 0.6f * Time.fixedDeltaTime, movement.y * 0.85f, 0));
            animator.SetFloat("playerSpeed", Mathf.Abs(inputVector.x));
            if (jump && controller.isGrounded)
            {
                walkEffect.Stop();
                //animator.SetBool("isJumping", true);
                movement.y = jumpSpeed;
                animator.SetBool("isJumping", true);
                jump = false;
            }
            if (movement.y < 0.004f)
            {
                animator.SetBool("isJumping", false);
            }
            // animator.SetBool("isJumping", false);
            //Debug.Log(inputVector.x);
            movement.y -= 0.4f * Time.fixedDeltaTime;
        }

    }
    void Update()
    {
        yarnText.text = "Yarn: " + knittedItem.ToString();
 
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        if (inputVector.x == 0)
        {
            moving = false;
            StartCoroutine(StopAnimation());
        }
        if (inputVector.x > 0 && left)
        {
            facingRight = true;
            facingLeft = false;
            TurnRight();
            playAnimation = false;
        }
        else if (inputVector.x < 0 && right)
        {
            facingRight = false;
            facingLeft = true;
            TurnLeft();
            // playAnimation = false;
        }
        // Debug.Log("TIMER: " + timer);
        if (timerStart)
        {
            timer += Time.deltaTime;
            if (timer >= 5f)
            {
                timer = 0f;
                timerStart = false;
            }
        }
    }
    void TurnLeft()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        left = true;
        right = false;
    }
    void TurnRight()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        left = false;
        right = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Knitted"))
        {
            Destroy(other.gameObject);
            knittedItem++;
            platformAudios.Play();
            transform.localScale = new Vector3(transform.localScale.x + 0.003f, transform.localScale.y + 0.003f, transform.localScale.z + 0.003f);            
        }
        if (other.gameObject.CompareTag("PickUp") && Input.GetKey(KeyCode.F))
        {
            Destroy(other.gameObject);
            knittedItem++;
            if (left)
            {
                transform.localScale = new Vector3(transform.localScale.x + 0.003f, transform.localScale.y + 0.003f, transform.localScale.z + 0.003f);
            }
            else if (right)
            {
                transform.localScale = new Vector3(transform.localScale.x + 0.003f, transform.localScale.y + 0.003f, transform.localScale.z + 0.003f);
            }
        }
    }
    IEnumerator StopAnimation()
    {
        yield return new WaitForSeconds(1f);
        playAnimation = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Win"))
        {
            SceneManager.LoadScene("Victory");
        }

        if (other.gameObject.CompareTag("PickUp"))
        {
            pickUpAgain = true;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            currentHealth -= 1;
            healthBar.SetHealth(currentHealth);
            Debug.Log("Health: " + health);
            hitAudio.Play();
        }
        if(other.gameObject.CompareTag("HurtDestination"))
        {
            RespawnLocations(0);
        }
        if (other.gameObject.CompareTag("HurtDestination2"))
        {
            RespawnLocations(1);
        }
        if (other.gameObject.CompareTag("HurtDestination3"))
        {
            RespawnLocations(2);
        }
        if (other.gameObject.CompareTag("HurtDestination4"))
        {
            RespawnLocations(3);
        }
        if (other.gameObject.CompareTag("HurtDestination5"))
        {
            RespawnLocations(4);
        }
        if (other.gameObject.CompareTag("HurtDestination6"))
        {
            RespawnLocations(5);
        }
        if (other.gameObject.CompareTag("HurtDestination7"))
        {
            RespawnLocations(6);
        }
        if (other.gameObject.CompareTag("HurtDestination8"))
        {
            RespawnLocations(7);
        }
        if (other.gameObject.CompareTag("HurtDestination9"))
        {
            RespawnLocations(8);
        }
        if (other.gameObject.CompareTag("HurtDestination10"))
        {
            RespawnLocations(9);
        }
        if (other.gameObject.CompareTag("HurtDestination11"))
        {
            RespawnLocations(10);
        }
        if (other.gameObject.CompareTag("HurtDestination12"))
        {
            RespawnLocations(11);
        }
        if (other.gameObject.CompareTag("HurtDestination13"))
        {
            RespawnLocations(12);
        }
    }

    void RespawnLocations(int index) {

        this.gameObject.SetActive(false);
        transform.position = reSpawnLocations[index].transform.position;
        this.gameObject.SetActive(true);
        currentHealth -= 1;
        healthBar.SetHealth(currentHealth);
        hitAudio.Play();
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("PickUp"))
        {
            pickUpAgain = false;
        }
    }
}

/* 
 * 
 * if(ballIsMade)
        {
            if (value.performed && timer == 0f)
            {
                if (inputVector.x > 0)
                {
                    // Debug.Log("Were shooting right");
                    shootingRight = true;
                    shootingLeft = false;
                    ballCount = 1;                  
                }
                else if (inputVector.x < 0)
                {
                    // Debug.Log("wee shooting left");
                    shootingLeft = true;
                    shootingRight = false;
                    ballCount = 1;

                }
                throwing = true;
                ballCount = 1;
                throwItems = 1;
                timerStart = true;
                ballAudio.Play();
                playAnimation = false;
            }
        }
        else
 *  public void PickUp(InputAction.CallbackContext value)
    {
        if (value.performed && knittedItem >= 1 && canCreateItems >= 1 && !platformIsMade && !ballIsMade) 
        {
            ball = Resources.Load("yarnBallThrown") as GameObject;
            Instantiate(ball, new Vector3(destination.transform.position.x, destination.transform.position.y, 0), destination.rotation);
            canCreateItems--;
            ballIsMade = true;
            knittedItem--;
            playAnimation = true;
        }
    }
 [SerializeField]
    GameObject ball;
    [SerializeField]
    GameObject instantiatedBall;

 *  public bool BallIsMade
    {
        get { return ballIsMade; }
        set
        {
            ballIsMade = value;
        }
    }
 *  bool shootingRight, shootingLeft
 *  public bool Throwing
    {
        get { return throwing; }
        set
        {
            throwing = value;
        }
    }
    public bool ShootingLeft
    {
        get { return shootingLeft; }
    }
    public bool ShootingRight
{
    get { return shootingRight; }
}
 * public void Throw(InputAction.CallbackContext value)
    {
        if(ballIsMade)
        {
            if (value.performed && timer == 0f)
            {
                if (inputVector.x > 0)
                {
                    // Debug.Log("Were shooting right");
                    shootingRight = true;
                    shootingLeft = false;
                    ballCount = 1;                  
                }
                else if (inputVector.x < 0)
                {
                    // Debug.Log("wee shooting left");
                    shootingLeft = true;
                    shootingRight = false;
                    ballCount = 1;

                }
                throwing = true;
                ballCount = 1;
                throwItems = 1;
                timerStart = true;
                ballAudio.Play();
                playAnimation = false;
            }
        }
        else if (platformIsMade && value.performed)
        {
            platformIsMade = false;
            canPlaceDown = true;
            platformAudios.Play();
        }
    }
 * if (other.gameObject.CompareTag("Enemy"))
        {
            currentHealth -= 1;
            healthBar.SetHealth(currentHealth);
            Debug.Log("Health: " + health);
            hitAudio.Play();
        }
*/
