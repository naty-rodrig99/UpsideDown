using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Burst.CompilerServices;
using System;
using TMPro;

using gamespace;


public class player_script : MonoBehaviour
{
    //public Animator animator;

    private int _health;
    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    public GameObject Camera;
    PlayerCamera camera_controller;

    public GameObject bullet;

    BoxScript box_controller;
    public GameObject Box;

    private int looking_direction;

    public TextMeshProUGUI bulletText; 
    private int currentBullets = 0; 

    public WorldType start_world;

    private Vector3 initial_pos;
    BoxCollider2D boxCollider2d;

    public SoundManager soundManager;
    //public HealthManager healthManager;
    private object console;

    // Start is called before the first frame update
    void Start()
    {

        boxCollider2d = GetComponent<BoxCollider2D>();

        initial_pos = transform.position;

        camera_controller = Camera.GetComponent<PlayerCamera>();
        box_controller = Box.GetComponent<BoxScript>();
        init_world();

    }

    void OnEnable()
    {
        WorldController.OnWorldChanged += UpdateWorld;
    }
    void OnDisable()
    {
        WorldController.OnWorldChanged -= UpdateWorld;
    }
    
    void UpdateWorld(WorldType type){
        change_world(type);
    }

    void init_world(){
        Debug.Log("test");

        WorldController.UpdateCurrentWorld(start_world);

        // if (start_world == WorldType.GoodWorld)
        // {
            
        //     //animator.SetBool("good_world", true);
        // }
        // else
        // {
        //     WorldController.UpdateCurrentWorld(1);
        //     //animator.SetBool("good_world", false);
        // }
    }

    void FixedUpdate()
    {
        check_bounds();
    }
    void Update(){
        check_inputs();
    }
    void check_bounds(){
        if ((WorldController.GetCurrentWorld() == WorldType.GoodWorld && transform.position.y < -14) || (WorldController.GetCurrentWorld() == WorldType.BadWorld && transform.position.y < -33))
        {
            transform.position = initial_pos;
            if (WorldController.GetCurrentWorld() == WorldType.BadWorld)
            {
                WorldController.change_world();
            }
            //Manager.ModifyHealth(-1);
            return;
        }
    }

    void check_inputs(){
        if (Input.GetButtonDown("Fire2")) // change from good to bad
        {
            Debug.Log("trying to change world");
            try_change_world();
        }
        if (Input.GetButtonDown("Fire1") && WorldController.IsWorldBad())
        {
            fire_bullet();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        if(WorldController.GetEnergyBar().isReady()){
            WorldController.GetEnergyBar().useEnergy();
            WorldController.change_world();
        }
    }

    void change_world(WorldType world){       
        switch (world){
            case WorldType.BadWorld:
                transform.position += new Vector3(0, -20, 0);
                break;
            case WorldType.GoodWorld:
                transform.position += new Vector3(0, 20, 0);
                break;
        }
    }

    private bool isGrounded()
    {
        int layerMask = ~(LayerMask.GetMask("PlayerLayer"));
        RaycastHit2D hit = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.down, boxCollider2d.bounds.extents.y + 0.1f, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "platform" || hit.collider.tag == "Tile")
            {
                //animator.SetBool("isJumping", false);
                return true;
            }
            
        }
        return false;
    }

    public void AddBullets(int amount)
    {
        currentBullets += amount;
    }

    public void UpdateBulletUI()
    {
        bulletText.text = currentBullets.ToString();
    }
}

