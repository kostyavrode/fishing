using System.Collections.Generic;
using UnityEngine;

// This script is used to control the player's game object in the third-person perspective.
public class ThirdPController : MonoBehaviour
{
    public CharacterController controller;
    // the Main Camera in the scene
    public Transform camera;
    public float speed = 6.0f;
    public float runSpeed = 12.0f;

    private float currentSpeed = 0.0f;

    // parameters for smooth turning
    public float turnSmoothingTime = 0.1f;
    float turnSmoothingVelocity;

    // parameters for jumping
    bool isGrounded;
    public float jumpHeight = 1.0f;
    public float jumpAdjustment = -2.0f;
    public float gravity = -9.81f;
    // to fix floaty jumps
    public float gravityFactor = 2.0f;

    public Vector3 playerVelocity;

    public bool bhopEnabled = false;

    // animator
    public Animator animator;
    private CharacterInputController cinput;
    float _inputForward = 0f;
    float _inputTurn = 0f;
    public float speedChangeRate = 0.1f;
    private float currentVelY = 0f;
    private float targetVelY = 0f;
    public bool alreadyCast = false;
    public bool isFishing = false;
    public bool isReeling = false;

    public VerletLine verletLine;

    [Header("Joysticks")]
    public Joystick moveJoy;
    public Joystick rotateJoy;

    // fishing objects
    public GameObject fishingRod;
    public GameObject[] objectToDeactivate;

    void Start()
    {
        controller = GetComponent<CharacterController>();
       // Cursor.lockState = CursorLockMode.Locked; // lock the cursor to the center of the screen.
        cinput = GetComponent<CharacterInputController>();
        animator.SetBool("startFishing", false);
        fishingRod.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == GameStates.PLAYING)
        {
            // Debug.Log(controller.isGrounded);
            if (controller.isGrounded && playerVelocity.y < 0)
            {
                animator.SetBool("isGrounded", true);
                // todo: add jumping and landing animation
                // animator.SetBool("isJumping", false);
                // animator.SetBool("isFalling", false);
                playerVelocity.y = 0f;
                playerVelocity.x = 0f;
                playerVelocity.z = 0f;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Start fishing
                if (!isFishing)
                {
                    animator.SetBool("startFishing", true);
                    isFishing = true;
                    fishingRod.SetActive(true);
                    foreach (GameObject obj in objectToDeactivate)
                    {
                        obj.SetActive(true);
                    }
                }
                // Stop fishing
                else if (isFishing && !alreadyCast && !isReeling)
                {
                    animator.SetBool("startFishing", false);
                    isFishing = false;
                    fishingRod.SetActive(false);
                    foreach (GameObject obj in objectToDeactivate)
                    {
                        obj.SetActive(false);
                    }
                }
            }

            // Cast the Fishing Rod
            //else if (Input.GetMouseButtonDown(0) && isFishing && !alreadyCast && !isReeling)
            //{
            //    animator.SetTrigger("cast");
            //    alreadyCast = true;
            //}

            // Reel in the Fishing Rod
            if (Input.GetKeyDown(KeyCode.Q) && isFishing && alreadyCast && !isReeling)
            {
                animator.SetTrigger("reel");
                isReeling = true;
            }

            // Check if the Reel In animation is done playing
            if (isReeling)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (!stateInfo.IsName("Reel In"))
                {
                    alreadyCast = false;
                    isReeling = false;
                }
            }


            // If the player isn't fishing, then they can move
            if (!isFishing)
            {
                if (cinput.enabled)
                {
                    _inputForward = cinput.Forward;
                    _inputTurn = cinput.Turn;
                }
                //float horizontal = Input.GetAxisRaw("Horizontal");
                //float vertical = Input.GetAxisRaw("Vertical");
                float vertical = moveJoy.Vertical;
                float horizontal = moveJoy.Horizontal;
                var animState = animator.GetCurrentAnimatorStateInfo(0);
                // animator.SetFloat("velX", _inputTurn);

                // absolute value of _inputForward and _inputTurn
                _inputForward = Mathf.Abs(_inputForward);
                _inputTurn = Mathf.Abs(_inputTurn);
                targetVelY = Mathf.Max(_inputForward, _inputTurn);
                Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
                if (direction.magnitude >= 0.1f) // if no input, stop applying movement
                {
                    // movement and also handles jumping while moving
                    // this section might need to be moved to a new function and changed if bhop or other movement mechanics are added.

                    // calc the angle between the player's input direction and the positive x-axis.
                    // the angle is then used to rotate the player's game object so that it faces the direction of movement.
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
                    // smoothly rotate the player's game object to face the direction of movement.
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVelocity, turnSmoothingTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                    // if Jumping while moving
                    if (controller.isGrounded)
                    {
                        if (bhopEnabled)
                        {
                            if (Input.GetButton("Jump"))
                            {
                                Jump();
                            }
                        }
                        else
                        {
                            if (Input.GetButtonDown("Jump"))
                            {
                                Jump();
                            }
                        }
                        // player cannot "run" while in the air
                        // if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                        // {
                        //     currentSpeed = runSpeed;
                        // }
                        // else
                        // {
                        //     currentSpeed = speed;
                        // }
                        currentSpeed = speed;
                    }
                    // current speed is preserved while in the air
                    controller.Move(moveDirection * currentSpeed * Time.deltaTime);
                }
                else if (controller.isGrounded)
                {
                    // jumping in place
                    if (bhopEnabled)
                    {
                        if (Input.GetButton("Jump"))
                        {
                            Jump();
                        }
                    }
                    else
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            Jump();
                        }
                    }
                }
                currentVelY = Mathf.Lerp(currentVelY, targetVelY, speedChangeRate);
                animator.SetFloat("velY", currentVelY);
                playerVelocity.y += gravityFactor * gravity * Time.deltaTime;
                controller.Move(playerVelocity * Time.deltaTime);
            }
        }
    }

    public void Throw()
    {
        if (isFishing && !alreadyCast && !isReeling)
        {
            animator.SetTrigger("cast");
            alreadyCast = true;
            verletLine.StartCorout();
            verletLine.poplavok.fish.SetActive(false);
            
            //verletLine.StartCoroutine(verletLine.IncreaseLengthAfterDelay(Delay));
        }
    }
    public void StartReel()
    {
        if (isFishing && alreadyCast && !isReeling)
        {
            animator.SetTrigger("reel");
            isReeling = true;
            verletLine.StartReeling();
        }
    }
    public void StartFisghing()
    {
        // Start fishing
        if (!isFishing)
        {
            verletLine.poplavok.transform.position = this.transform.position + Vector3.up+transform.forward/2;
            animator.SetBool("startFishing", true);
            isFishing = true;
            fishingRod.SetActive(true);
            foreach (GameObject obj in objectToDeactivate)
            {
                obj.SetActive(true);
            }
        }
        // Stop fishing
        else if (isFishing && !alreadyCast && !isReeling)
        {
            animator.SetBool("startFishing", false);
            isFishing = false;
            fishingRod.SetActive(false);
            foreach (GameObject obj in objectToDeactivate)
            {
                obj.SetActive(false);
            }
        }
    }
    public void DisableFishAnim()
    {
        animator.SetBool("startFishing", false);
        isFishing = false;
        alreadyCast = false;
        isReeling = false;
        verletLine.poplavok.rb.isKinematic = false;
        verletLine.DEactivateAnim();
        fishingRod.SetActive(false);
        foreach (GameObject obj in objectToDeactivate)
        {
            obj.SetActive(false);
        }
        verletLine.poplavok.transform.position = this.transform.position + Vector3.up + transform.forward / 2;
    }
    // Character Controller handles collision detection different. Make sure to read docs.
    // I don't believe it can detect triggers, so an alternative method would be creating an empty GameObject child
    // and then giving that child a collider (make sure it doesn't collide with Player) to detect triggers.

    // private void OnCollisionStay(Collision collision) {
    //     if (collision.gameObject.tag == "Ground") {
    //         isGrounded = true;
    //     }
    // }
    // private void OnCollisionExit(Collision collision) {
    //     if (collision.gameObject.tag == "Ground") {
    //         isGrounded = false;
    //     }
    // }
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        // Debug.Log("Controller collision detected");
    }
    void Jump() {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * jumpAdjustment * gravityFactor * gravity);
    }
}