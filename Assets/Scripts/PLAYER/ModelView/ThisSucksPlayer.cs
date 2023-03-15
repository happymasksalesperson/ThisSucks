using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ThisSucksPlayer : MonoBehaviour
{
    [SerializeField] private GameObject ciggy;
    [SerializeField]
    private bool smoking=true;
    
    GameObject playerObject;
    Rigidbody rb;
    Renderer rend;
    
    int uselessPoints;

    public int ReturnHP()
    {
        return HP;
    }
    
    public int UselessPoints()
    {
        return uselessPoints;
    }
    
    
    int HP;
    int maxHP;
    bool isAlive=true;

    bool intangible=false;
    bool takingDamage = false;

    //tracks if the Player is currently performing a trick
    bool tricking=false;

    enum TricksList { Kickflip, PopShoveIt};
    TricksList currentTrick;

    //
    private enum PlayerState
    {
        Spawning,
        TakingDamage,
        JumpCharging,
        Jumping,
        Tricking
    };
    private PlayerState currentState;
    
 //enum declares Rails. the Player can occupy one of three Rails
    //Player moves between Rails to dodge enemies and obstacles
    private int[] railInts;
    public enum Rails { RailDown, RailMid, RailUp};
    Rails currentRail;
    int railInt;
       
    //determines how fast the Player moves between Rails
    private float railSpeed = 10f;

    private int fixedUpdateCount=0;

    //
    bool accelerating=false;
    bool deaccelerating=false;
    private float moveSpeed = 300f;
    public float maxSpeed = 500f;
    private float minSpeed = 200f;
    private bool canSpeed = true;

    private bool grounded;

    private bool jumpCharging = false;
    [SerializeField] private float jumpChargeRate = .15f;
    private float currentJumpPower;
    [SerializeField] private float minJumpPower;
    [SerializeField] private float maxJumpPower;

    private bool canMove=true;
    private bool isMoving=false;
    private int moveDir=0;

    private int moveCharge = 1;

    //float determines the max distance the Player can Move
    //clamps movement between rails so it's the same every time
    private float totalDistance = 0;
    float maxDistance;
    private Vector3 previousPos;

    private Vector3 Up = new Vector3(0, 0, 50f);
    private Vector3 Down = new Vector3(0, 0, -50f);

    //declares skateboard gameObject to rotate along with player movement
    public GameObject skateboard;


    //the beginnings of the Player State Machine
    private enum PlayerStance { Idle, Moving, ChargingJump, Jumping, Tricking, Grinding, Dead };
    PlayerStance currentStance;

    void Start()
    {
         //   OnRailsControls onRailsControls = new OnRailsControls();
           // onRailsControls.OnRailsMap.Enable();

            //nRailsControls.OnRailsMap.Jump.performed += JumpCharge;
           // normalControls.InGame.Jump.performed += JumpOnperformed;

            //onRailsControls.OnRailsMap.Jump.WasReleasedThisFrame() += Jump();


            playerObject = this.gameObject;
        rb = this.GetComponent<Rigidbody>();
        rend = this.GetComponent<Renderer>();
        rend.enabled = true;

        currentRail = Rails.RailMid;
        railInt = 1;

        rb.useGravity = false;

        Physics.IgnoreLayerCollision(0, 0);
        Physics.gravity = new Vector3(0, -1000f, 0);


        maxHP = 3;
        HP = maxHP;
        
        ciggy.SetActive(true);

    }

    // Update is called once per frame
    void Update()
   
    { 
        Debug.Log(HP);
        if (isAlive) {
            if (canMove)
            {


                if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.A)))
                {
                    if (canMove)
                    {
                        if ((railInt == 0) || (railInt == 1))
                        {
                            moveDir = 1;
                            StartCoroutine(Move());
                        }
                    }
                }
                    

                if ((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.D)))
                {
                    if (canMove)
                    {
                        if ((railInt == 2) || (railInt == 1))
                        {
                            moveDir = 2;
                            StartCoroutine(Move());
                        }
                    }
                }

            }
            if ((canMove) && (grounded))
            {

                if (Input.GetKey(KeyCode.Space))
                    jumpCharging = true;

            if (Input.GetKeyUp(KeyCode.Space))
                StartCoroutine(Jump());
        }

            if (!accelerating)
        {
            if ((Input.GetKeyUp(KeyCode.E)) && (grounded) && (canSpeed))
                StartCoroutine(SpeedUp());
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
            Shoot();

        if ((Input.GetKeyDown(KeyCode.R)) && (!grounded))
        {
            tricking = true;
            currentTrick = TricksList.Kickflip;
        }

        if ((Input.GetKeyDown(KeyCode.Q)) && (!grounded))
        {
            tricking = true;
            currentTrick = TricksList.PopShoveIt;
        }
    }
        
    }

    private void FixedUpdate()
    {

        Vector3 down = transform.TransformDirection(Vector3.down);

        if (Physics.Raycast(transform.position, down, 15))
        {
            grounded = true;
        }

        else
            grounded = false;

        if (isMoving)
        {
            fixedUpdateCount++;

            if (totalDistance == 100f)
                isMoving = false;

            else if (moveDir == 1)
                rb.MovePosition(transform.position+Up * Time.deltaTime * railSpeed);

            else if (moveDir == 2)
                rb.MovePosition(transform.position+Down * Time.deltaTime * railSpeed);

        }

        if (jumpCharging)
        {
            if (currentJumpPower < maxJumpPower)
                currentJumpPower += jumpChargeRate * Time.deltaTime;

            if (currentJumpPower >= maxJumpPower)
                currentJumpPower = maxJumpPower;
        }


        //controls max and min movespeed
        if(isAlive)
        rb.AddForce(Vector3.right * moveSpeed);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        if (rb.velocity.magnitude < minSpeed)
        {
            rb.velocity = rb.velocity.normalized * minSpeed;
        }

        if(accelerating)
        {
            //put acceleration animation here
        }

        //in the event the Player is no longer alive, reduce all speed until speed is 0
        if (deaccelerating)
        {
            moveSpeed -= 25;
            minSpeed-=25;
            maxSpeed-=25;
            if (minSpeed <= 25)
            {
                moveSpeed = 0;
                maxSpeed = 0;
                minSpeed = 0;
            }
        }

        if (grounded)
        {
            tricking = false;
            moveCharge = 1;
        }

        if (tricking)
        {
            moveCharge = 0;
            ChangePoints(1);
            switch (currentTrick)
            {
                case TricksList.Kickflip:
                    if(moveDir==1)
                    skateboard.transform.Rotate(15, 0, 0, Space.Self);
                    else
                        skateboard.transform.Rotate(-15, 0, 0, Space.Self);
                    break;

                case TricksList.PopShoveIt:
                    if (moveDir == 1)
                        skateboard.transform.Rotate(0, -15, 0, Space.Self);
                    else
                        skateboard.transform.Rotate(0, 15, 0, Space.Self);
                    break;
            }

        }

        if ((!tricking) && (!isMoving))
        {
            playerObject.transform.rotation = Quaternion.identity;
            skateboard.transform.rotation = Quaternion.identity;
        }

        if ((takingDamage) && (isAlive))
            rend.enabled = !rend.enabled;
        else
                rend.enabled = true;

    }

    private IEnumerator SpeedUp()
    {
        accelerating = true;
        canSpeed = false;
        maxSpeed += 750f;

        yield return new WaitForSeconds(1.25f);
        accelerating = false;
        canSpeed = true;
        maxSpeed -= 500f;

    }

    private IEnumerator Move()
    {

        if (moveCharge == 1)
        {
            moveCharge -= 1;
            if (!tricking)
            {
                if (moveDir == 1)
                {
                    railInt++;
                    skateboard.transform.Rotate(0, -45f, 0.0f, Space.Self);
                }
                else if (moveDir == 2)
                {
                    railInt--;
                    skateboard.transform.Rotate(0, 45f, 0.0f, Space.Self);
                }
            }

            isMoving = true;
            canMove = false;
            intangible = true;

            yield return new WaitUntil(() => fixedUpdateCount >= 5);


            isMoving = false;
            intangible = false;
            canMove = true;

            fixedUpdateCount = 0;

            skateboard.transform.rotation = Quaternion.identity;
        }
    }

    private IEnumerator Jump()
    {
        moveCharge = 0;
        
        if (currentJumpPower < minJumpPower)
            currentJumpPower = minJumpPower;

        jumpCharging = false;
        rb.AddForce(transform.up * currentJumpPower);
        rb.useGravity = true;
        Debug.Log("Jumping with " + currentJumpPower);

        currentJumpPower = 0;


        yield return new WaitUntil(() => grounded);

    }

    private void Shoot()
    {
        Debug.Log("Pew pew!");
    }

    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.down) * 35;
        Gizmos.DrawRay(transform.position, direction);


        Vector3 fordirection = transform.TransformDirection(Vector3.right) * 15;
        Gizmos.DrawRay(transform.position, fordirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        //change this to just NPC later
        if (other.gameObject.tag == "NPC")
        {
            if (!intangible)
                StartCoroutine(TakeDamage());
        }
        if (other.gameObject.tag == "Goal")
            Debug.Log("You win!");

       // if (other.gameObject.tag == "HealthUp")
            if(other.gameObject.GetComponent<PowerupScript>() != null)
            {
                ChangeHealth(1);
            Debug.Log("Health up!");
        }
    }

    private IEnumerator TakeDamage()
    {
        takingDamage = true;
        if(HP <= 0)
        GameOver();

        intangible = true; 
        ChangeHealth(-1);
        ChangePoints(-50);
        yield return new WaitForSeconds(1.25f);
        intangible = false;
        takingDamage = false;
    }

    public void ChangeHealth(int amount)
    {
        HP += amount;
        if (HP >= maxHP)
            HP = maxHP;

    }

    public void ChangePoints (int amount)
    {
        uselessPoints += amount;
        if(uselessPoints <=0)
            uselessPoints = 0;
    }

    private void GameOver()
    {
            isAlive = false;
            canMove = false;
            deaccelerating = true;
    }


}
