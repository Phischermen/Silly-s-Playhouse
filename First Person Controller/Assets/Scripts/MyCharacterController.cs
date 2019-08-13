using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    [HideInInspector]
    public int health = 100;
    private float desired_speed = 10.0f;
    [HideInInspector]
    public float walk_speed = 5f;
    [HideInInspector]
    public float sprint_speed = 7.5f;
    [HideInInspector]
    public float jump_force = 5f;
    public GameObject deadCharacter;
    public Text healthText;
    public Image healthBar;


    Rigidbody my_rigidbody;
    CapsuleCollider my_collider;
    public Camera my_camera;
    public Pickup my_pickup;
    private bool was_jump_pressed = false;
    private bool is_feather_falling;
    private Vector3 move_vector;
    private Vector3 prev_move_vector;
    private float highestY;
    private int number_of_adjacent_colliders;
    private bool isCrouched = false;
    private CrouchState crouch_state = CrouchState.NotCrouched;
    private int layer_mask_solid;
    enum CrouchState
    {
        Crouched,
        NotCrouched,
        Falling,
        Rising
    }
    static private Scene myLatestScene;
    static public bool firstSpawnInScene;
    void Awake()
    {
        //Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Get Components
        my_rigidbody = GetComponent<Rigidbody>();
        my_collider = GetComponent<CapsuleCollider>();
        
        //Get layer mask
        layer_mask_solid = LayerMask.GetMask("Default", "InteractSolid");
    }
    private void Start()
    {
        //Update 'firstSpawnInScene'
        //'firstSpawnInScene' is used by other scripts for initialization.
        if (firstSpawnInScene)
            myLatestScene = SceneManager.GetActiveScene();
        firstSpawnInScene = myLatestScene != SceneManager.GetActiveScene();
    }
    private void OnCollisionEnter(Collision collision)
    {
        number_of_adjacent_colliders += 1;
        if (number_of_adjacent_colliders > 1)
        {
            my_rigidbody.useGravity = false;
            //Debug.Log("Fake gravity engaged");
        }
        float point = collision.GetContact(0).point.y;
        //Fall damage
        if (point < transform.position.y-0.9f)
        {
            float impact = (highestY - transform.position.y);
            float[] threshold = new float[5]
            {
                0.5f,
                1.5f,
                6.5f,
                9.5f,
                18f,
            };
            if (impact > threshold[0]){
                Debug.Log(impact);
            }
            
            if (threshold[0] <= impact && impact < threshold[1])
            {
                Debug.Log("Soft Landing");
            }
            else if (threshold[1] <= impact && impact < threshold[2])
            {
                Debug.Log("Medium Landing");
            }
            else if (threshold[2] <= impact && impact < threshold[3])
            {
                //Hard land sound
                Debug.Log("Hard Landing");
                int damage = (int)((impact - threshold[2]) / (threshold[3] - threshold[2]) * 10);
                Debug.Log("Damage Dealt: " + damage);
                dealDamage(damage);
            }
            else if ( threshold[3] <= impact && impact < threshold[4])
            {
                Debug.Log("Really Hard Landing");
                int damage = (int)((impact - threshold[2]) / (threshold[3] - threshold[2]) * 10);
                Debug.Log("Damage Dealt: " + damage);
                dealDamage(damage);
            }
            else if (threshold[4] <= impact)
            {
                Debug.Log("Fatal Landing");
                killCharacter();
            }
            highestY = transform.position.y;
        }else if(point > (transform.position.y + 0.8f))
        {
            MatchThreeObject obj = collision.gameObject.GetComponent<MatchThreeObject>();
            if (obj != null)
            {
                //Possibly Crushed
                int weight = obj.GetStackWeight();
                Debug.Log(weight);
                int[] threshold = new int[3]
                {
                    1,
                    2,
                    3,
                };
                if (threshold[0] <= weight && weight < threshold[1])
                {
                    Debug.Log("Small Crush");
                    dealDamage(10);
                }
                else if (threshold[1] <= weight && weight < threshold[2])
                {
                    Debug.Log("Medium Crush");
                    dealDamage(15);
                }
                else if (threshold[2] <= weight)
                {
                    Debug.Log("Crushed");
                    killCharacter();
                }
            }
            
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        number_of_adjacent_colliders -= 1;
        if (number_of_adjacent_colliders <= 1)
        {
            my_rigidbody.useGravity = true;
            //Debug.Log("Fake gravity disengaged");
        }
        if(transform.position.y > highestY)
        {
            highestY = transform.position.y;
            Debug.DrawRay(transform.position, Vector3.up, Color.blue, 10f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "m3Grid")
        {
            my_pickup.placement_grid = other.GetComponent<Grid>();
            RoomResetterInteraction.canReset = true;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        bool grounded = checkGrounded();
        //Get jump input
        if (grounded)
        {
             if (Input.GetButtonDown("Jump"))
                {
                was_jump_pressed = true;
            }
        }
        else
        {
            if(highestY < transform.position.y)
            {
                highestY = transform.position.y;
                Debug.DrawRay(transform.position, Vector3.up, Color.blue, 10f);
            }
        }

        //Get Sprint
        desired_speed = (Input.GetButton("Sprint")) ? (sprint_speed) : (walk_speed);
        /*
        //Get Falling
        is_feather_falling = (my_rigidbody.velocity.y < 0 && Input.GetButton("Jump"));
        */
        //Get Move Input
        move_vector.z = Input.GetAxis("Vertical") * desired_speed * Time.fixedDeltaTime;
        move_vector.x = Input.GetAxis("Horizontal") * desired_speed * Time.fixedDeltaTime;
        move_vector = transform.localRotation * move_vector;
        move_vector = Vector3.Lerp(prev_move_vector, move_vector, 0.2f);
        move_vector = Vector3.ClampMagnitude(move_vector, desired_speed * Time.fixedDeltaTime);
        //Get Crouch Input
        if (Input.GetButtonDown("Crouch"))
        {
            if (isCrouched)
            {
                if(!Physics.CheckSphere(transform.position + new Vector3(0f, 0.5f, 0f), my_collider.radius-0.1f, layer_mask_solid))
                {
                    SetCrouchState(false);
                }
            }
            else
            {
                SetCrouchState(true);
            }
            
        }
        //Lerp the camera based on crouch
        if(crouch_state != CrouchState.Crouched && crouch_state != CrouchState.NotCrouched)
        {
            Vector3 target = Vector3.zero;
            switch (crouch_state)
            {
                case CrouchState.Rising:
                    target = new Vector3(0f, 0.5f, 0f);
                    break;
                case CrouchState.Falling:
                    target = new Vector3(0f, -0.1f, 0f);
                    break;
            }
            my_camera.transform.localPosition = Vector3.Lerp(my_camera.transform.localPosition, target, 0.1f);
            if (my_camera.transform.localPosition == target)
            {
                switch (crouch_state)
                {
                    case CrouchState.Rising:
                        crouch_state = CrouchState.NotCrouched;
                        break;
                    case CrouchState.Falling:
                        crouch_state = CrouchState.Crouched;
                        break;
                }
            }
        }
        //Locking the cursor
        if (Input.GetKeyDown("escape"))
        {
            //Toggle cursor lock
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ?
                (CursorLockMode.None) :
                (CursorLockMode.Locked);
            Cursor.visible = (Cursor.lockState == CursorLockMode.None);
        }
        
        //Restarting scene
        if (Input.GetKeyDown(KeyCode.F1))
        {
            //Force character to spwan like they do in the editor
            firstSpawnInScene = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            dealDamage(25);
        }
        //Update health bar
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health/100f, 0.1f);
        healthText.text = health.ToString();
    }

    private void FixedUpdate()
    {
        //Do Jump Input
        if (was_jump_pressed)
        {
            was_jump_pressed = false;
            Vector3 velocity = my_rigidbody.velocity;
            velocity.y = 0f;
            my_rigidbody.velocity = velocity;
            my_rigidbody.AddForce(new Vector3(0f, jump_force, 0f), ForceMode.VelocityChange);
            my_rigidbody.drag = 0;
        }
        //Set drag
        my_rigidbody.drag = (is_feather_falling) ? (5.0f) : (0.0f);
        //Check for approaching collision
        RaycastHit hit_info;
        while (Physics.CapsuleCast(transform.position + my_collider.center + (Vector3.up * my_collider.height * 0.15f),
                transform.position + my_collider.center + (Vector3.up * my_collider.height * -0.15f), my_collider.radius,
                move_vector.normalized, out hit_info, move_vector.magnitude,
                layer_mask_solid))
        {
            //If normal has a significant y component, break the while loop
            if(hit_info.normal.normalized.y > 0.5f)
            {
                //Debug.Log("Y: " + hit_info.normal.normalized.y);
                //Debug.DrawRay(transform.position, hit_info.normal.normalized, Color.green, 5);
                break;
            }
            Vector3 hit_normal_perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * hit_info.normal;
            hit_normal_perpendicular *= (Vector3.Angle(hit_normal_perpendicular, move_vector.normalized) > 90f) ? (-1) : (1);
            float clamp = Vector3.Dot(move_vector.normalized, hit_info.normal);
            clamp = (1f - Mathf.Abs(clamp)) * Mathf.Sign(clamp);
            move_vector = (hit_normal_perpendicular * Mathf.Sign(clamp)) * (move_vector.magnitude * clamp);
            move_vector.y = 0;

            //Debug.Log(move_vector);
        }
        //Manual Gravity
        if (my_rigidbody.useGravity == false)
        {
            my_rigidbody.AddForce(Vector3.down * 9.81f, ForceMode.Impulse);
        }
        //Translate the player
        my_rigidbody.MovePosition(my_rigidbody.position + move_vector);
        prev_move_vector = move_vector;
    }
    private bool checkGrounded()
    {
        return Physics.CheckSphere(transform.position + new Vector3(0, -1.06f, 0), 0.475f, layer_mask_solid);
    }
    public void dealDamage(int amount)
    {
        health = Mathf.Max(0, health - amount);
        if (health == 0)
        {
            killCharacter();
            return;
        }
    }
    public void killCharacter()
    {
        Destroy(gameObject);
        Instantiate(deadCharacter, transform.position, new Quaternion(my_camera.transform.rotation.x, transform.rotation.y, transform.rotation.z,transform.rotation.w));
    }
    public void SetCrouchState(bool crouched,bool instant = false)
    {
        isCrouched = crouched;
        if (isCrouched == false)
        {
            //Check if uncrouching is possible
            
            if (isCrouched == false)
            {
                //Set height and center of collider
                my_collider.height = 2;
                my_collider.center = new Vector3
                {
                    x = 0f,
                    y = 0f,
                    z = 0f
                };
                crouch_state = CrouchState.Rising;
            }
        }
        else
        {
            //Set height of collider
            my_collider.height = 1;
            my_collider.center = new Vector3
            {
                x = 0f,
                y = -0.5f,
                z = 0f
            };
            crouch_state = CrouchState.Falling;
        }
        if (instant)
        {
            Vector3 target = Vector3.zero;
            switch (crouch_state)
            {
                case CrouchState.Rising:
                    target = new Vector3(0f, 0.5f, 0f);
                    crouch_state = CrouchState.NotCrouched;
                    break;
                case CrouchState.Falling:
                    target = new Vector3(0f, -0.1f, 0f);
                    crouch_state = CrouchState.Crouched;
                    break;
            }
            my_camera.transform.localPosition = target;
        }
    }
    public bool GetCrouched()
    {
        return isCrouched;
    }
}
