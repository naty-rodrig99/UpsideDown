using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;


public class player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement = new Vector2(0, 0);
    public float speed;
    public float speedJump;
    public Animator animator;
    string current_world;

    public GameObject Camera;
    PlayerCamera camera_controller;
    BoxScript box_controller;

    public GameObject bullet;
    public GameObject Box;

    private int looking_direction;

    public GameObject switchEnergy;
    private SwitchBarUIBar EnergyBar;

    private bool[] wallClimbReady;
    private bool[] wallClimbActive;
    private float wallClimbTimout;
    private float wallClimbGraceTimer;
    private bool latest_activation; // false left, true right

    public float wallGlideSpeed;

    private float horizontalMovement;

    private Vector2 horizontalVelocity;
    private float[] accelerationCurve;
    private int[] horizontalIndex;


    public enum WorldType
    {
        GoodWorld,
        BadWorld
    }
    public WorldType start_world;
    private Vector3 initial_pos;
    BoxCollider2D boxCollider2d;

    public SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {

        GenerateAccelerationCurve(50, speed*2, speed/5);
        horizontalVelocity = new Vector2(0.0f,0.0f);
        horizontalIndex = new int[]{0,0};

        wallClimbReady = new bool[2]{false,false}; // [0] left [1] right
        wallClimbActive = new bool[2]{false,false}; // [0] left [1] right
        wallClimbTimout = 0;
        wallClimbGraceTimer = 0;
        latest_activation = false;

        EnergyBar = switchEnergy.GetComponent<SwitchBarUIBar>();

        boxCollider2d = GetComponent<BoxCollider2D>();

        initial_pos = transform.position;

        looking_direction = 1;
        rb = GetComponent<Rigidbody2D>();
        camera_controller = Camera.GetComponent<PlayerCamera>();
        box_controller = Box.GetComponent<BoxScript>();
        rb.freezeRotation = true;


        if (start_world == WorldType.GoodWorld)
        {
            current_world = "good";
            animator.SetBool("good_world", true);
        }
        else
        {
            current_world = "bad";
            animator.SetBool("good_world", false);
        }
        camera_controller.update_current_world(current_world);
        box_controller.update_current_world(current_world);
    }

    void Update()
    {
        if ((current_world == "good" && transform.position.y < -14) || (current_world == "bad" && transform.position.y < -33))
        {

            transform.position = initial_pos;
            rb.velocity = new Vector3(0, 0, 0);
            if (current_world == "bad")
            {
                change_world();
            }
            return;
        }


        if (horizontalMovement > 0)
        {
            looking_direction = 1; // Right
            animator.SetBool("good_Left", false);
            animator.SetBool("good_Right", true);
            animator.SetBool("bad_Left", false);
            animator.SetBool("bad_Right", true);
        }
        else if (horizontalMovement < 0)
        {
            looking_direction = -1; // Left
            animator.SetBool("good_Right", false);
            animator.SetBool("good_Left", true);
            animator.SetBool("bad_Right", false);
            animator.SetBool("bad_Left", true);
        }
        else if(horizontalMovement == 0)
        {
            animator.SetBool("good_Left", false);
            animator.SetBool("good_Right", false);
            animator.SetBool("bad_Left", false);
            animator.SetBool("bad_Right", false);
        }


        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("isJumping", true);
            jump();
        }

        if (Input.GetButtonDown("Fire2")) // change from good to bad
        {
            try_change_world();
        }
        if (Input.GetButtonDown("Fire1") && current_world == "bad")
        {
            fire_bullet();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Wall gliding
        if (Input.GetAxisRaw("Horizontal") < 0 && wallClimbReady[0])
        {
            wallClimbGraceTimer = 15.0f;
            wallClimbActive[0] = true; 
            latest_activation = false;
        }else{
            wallClimbActive[0] = false;
        }
        if (Input.GetAxisRaw("Horizontal") > 0 && wallClimbReady[1])
        {
            wallClimbGraceTimer = 15.0f;
            wallClimbActive[1] = true;
            latest_activation = true;
            
        }else{
            wallClimbActive[1] = false;
        }
        if((wallClimbActive[0] || wallClimbActive[1]) && rb.drag != wallGlideSpeed){
            rb.drag = wallGlideSpeed;
        }
        if(rb.drag == wallGlideSpeed && !wallClimbActive[0] && !wallClimbActive[1]){
            rb.drag = 0.05f;
        }
    }
    void FixedUpdate(){

        horizontalMovement = Input.GetAxis("Horizontal");

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            horizontalMovement *= accelerationCurve[Math.Min(accelerationCurve.Length -1, horizontalIndex[0])];
            horizontalIndex[0] += 1;
            Debug.Log(horizontalMovement);
        }else{
            horizontalIndex[0] = 0;
        }
        if (Input.GetAxisRaw("Horizontal") > 0 )
        {
            horizontalMovement *= accelerationCurve[Math.Min(accelerationCurve.Length -1, horizontalIndex[1])];
            horizontalIndex[1] += 1;
        }else{
            horizontalIndex[1] = 0;
        }

        //if(!isGrounded()){
        //    horizontalMovement *= 0.7f;
        //}

        //rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);
        rb.AddForce(new Vector2(horizontalMovement, 0), ForceMode2D.Force);

        float maxHorizontalSpeed = 10f; 
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed), rb.velocity.y);


        if (wallClimbTimout > 0)
        {
            //("timeout: " + wallClimbTimout);
            wallClimbTimout -= 1;
        }
        if (wallClimbGraceTimer > 0)
        {
            //Debug.Log("grace: " + wallClimbGraceTimer);
            wallClimbGraceTimer -= 1;
        }
    }

    public void GenerateAccelerationCurve(int n, float init, float y_converge)
    {
        accelerationCurve = new float[n]; 
        for (int i = 0; i < n; i++)
        {
            float x = i; 
            accelerationCurve[i] = (init / (float)Math.Exp(x)) + y_converge; 
        }
    }
    
    void jump()
    {
        if (isGrounded() )
        {
            Debug.Log("regular jump");
            rb.AddForce(new Vector2(0.0f, speedJump), ForceMode2D.Impulse);
            soundManager.PlayJumpSound();
        }
        if(wallClimbGraceTimer > 0.0f && latest_activation == false){ // left wall jump
            Debug.Log("Jump left wall");
            rb.AddForce(new Vector2(speedJump*0.8f, speedJump*0.8f), ForceMode2D.Impulse);
            wallClimbTimout = 10.0f;
            wallClimbReady[0] = false; 
        }
        if(wallClimbGraceTimer > 0.0f && latest_activation == true){ // right wall jump
            Debug.Log("Jump right wall");
            rb.AddForce(new Vector2(-speedJump*0.8f, speedJump*0.8f), ForceMode2D.Impulse);
            wallClimbTimout = 10.0f;
            wallClimbReady[1] = false; 
        }
    }
    void fire_bullet()
    {
        GameObject bullet_instance = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet bullet_script = bullet_instance.GetComponent<Bullet>();
        bullet_script.set_direction(looking_direction); // sets direction of bullet, 1 right -1 left
        soundManager.PlayShootSound();
    }
    void try_change_world(){
        Debug.Log("try change world");
        if(EnergyBar.getCharge() >= EnergyBar.SwitchCost){
            Debug.Log("Success");
            EnergyBar.update_switch_ui_bar(-EnergyBar.SwitchCost);
            change_world();
        }
    }

    void change_world()
    {
        switch (current_world)
        {
            case "good":
                current_world = "bad";
                transform.position += new Vector3(0, -20, 0);
                animator.SetBool("good_world", false);
                break;
            case "bad":
                current_world = "good";
                transform.position += new Vector3(0, 20, 0);
                animator.SetBool("good_world", true);
                break;
        }
        camera_controller.update_current_world(current_world);
        box_controller.update_current_world(current_world);
    }
    private bool isGrounded()
    {
        int layerMask = ~(LayerMask.GetMask("PlayerLayer"));
        RaycastHit2D hit = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.down, boxCollider2d.bounds.extents.y + 0.1f, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "platform" || hit.collider.tag == "Tile")
            {
                animator.SetBool("isJumping", false);
                return true;
            }
            
        }
        //Debug.DrawRay(boxCollider2d.bounds.center, Vector2.down * (boxCollider2d.bounds.extents.y + 0.1f));

        return false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "platform" || other.gameObject.tag == "box" || other.gameObject.tag == "Tile"){
        
            soundManager.PlayLandingSound();
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

            if(!isGrounded()){
                RaycastHit2D hit_left = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.left, boxCollider2d.bounds.extents.x + 0.2f, ~(LayerMask.GetMask("PlayerLayer")));
                RaycastHit2D hit_right = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.right, boxCollider2d.bounds.extents.x + 0.2f, ~(LayerMask.GetMask("PlayerLayer")));

                wallClimbReady[0] = hit_left.collider != null && (wallClimbTimout <= 0.5f || latest_activation == true);
                wallClimbReady[1] = hit_right.collider != null && (wallClimbTimout <= 0.5f || latest_activation == false);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "platform")
        {
        }
        if (other.gameObject.tag == "Tile")
        {
            wallClimbReady[0] = false;
            wallClimbReady[1] = false;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "point")
        {
            Destroy(other.gameObject);
            soundManager.PlayCollectPointSound();
        }
    }
}
