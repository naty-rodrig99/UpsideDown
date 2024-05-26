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

    public WorldType start_world;

    private Vector3 initial_pos;
    BoxCollider2D boxCollider2d;

    public ParticleSystem shoot_particles_right;
    public ParticleSystem shoot_particles_left;

    public Vector3 localBulletOffset = new Vector3(0.0f, 0.0f, 0.0f);

    public SoundManager soundManager;
    public HealthManager healthManager;
    public MenuManager menuManager;

    public TextMeshProUGUI bulletText;
    private int currentBullets = 0;
    private int currentHealth = 3;

    private object console;

    private bool dontUpdatePosOnStart = true;

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
        WorldController.OnPlayerDirectionChanged += UpdateDir;
    }
    void OnDisable()
    {
        WorldController.OnWorldChanged -= UpdateWorld;
        WorldController.OnPlayerDirectionChanged -= UpdateDir;
    }
    void UpdateWorld(WorldType type){
        change_world(type);
    }
    void UpdateDir(int dir){
        looking_direction = dir;
        SpriteRenderer _sr = GetComponent<SpriteRenderer>();
        if(dir == -1) _sr.flipX = true;
        else if(dir == 1) _sr.flipX = false;;

    }
    

    void init_world(){
        WorldController.UpdateCurrentWorld(start_world);

        if (start_world == WorldType.GoodWorld)
        {
            
            //animator.SetBool("good_world", true);
        }
        else
        {
            //WorldController.UpdateCurrentWorld(1);
            //animator.SetBool("good_world", false);
        }
    }

    void FixedUpdate()
    {
        check_bounds();
    }
    void Update(){
        check_inputs();
    }
    void check_bounds(){
        if ((WorldController.GetCurrentWorld() == WorldType.GoodWorld && transform.position.y < -14) || (WorldController.GetCurrentWorld() == WorldType.BadWorld && transform.position.y < - SETTINGS.worlds_offset - 33))
        {
            transform.position = initial_pos;
            if (WorldController.GetCurrentWorld() == WorldType.BadWorld)
            {
                WorldController.change_world();
            }
            ManageHealth(-1);
            return;
        }
    }

    void check_inputs(){
        if (Input.GetButtonDown("Fire2")) // change from good to bad
        {
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
        else if (Input.GetKeyDown(KeyCode.P))
        {
            menuManager.Pause();
        }
    }
    
    void fire_bullet()
    {
        //if there are no bullets, exit the method
        if (currentBullets == 0)
            return;

        Vector3 localBulletOffset_new = new Vector3(localBulletOffset.x * looking_direction, localBulletOffset.y, localBulletOffset.y);
        GameObject bullet_instance = Instantiate(bullet, transform.position + localBulletOffset_new, Quaternion.identity);
        Bullet bullet_script = bullet_instance.GetComponent<Bullet>();

        Debug.Log(looking_direction);

        if(looking_direction > 0.0f) shoot_particles_right.Play();
        if(looking_direction < 0.0f) shoot_particles_right.Play();
        
        bullet_script.set_direction(looking_direction); // sets direction of bullet, 1 right -1 left
        soundManager.PlayShootSound();
        ManageBullet(-1);
    }

    void try_change_world(){
        if(WorldController.GetEnergyBar().isReady()){
            WorldController.GetEnergyBar().useEnergy();
            WorldController.change_world();
        }
    }

    void change_world(WorldType world){      
        if(dontUpdatePosOnStart){
            dontUpdatePosOnStart = false;
            return;
        }
        switch (world){
            case WorldType.BadWorld:
                Debug.Log("bad");
                Debug.Log(SETTINGS.worlds_offset);
                transform.position += new Vector3(0, -SETTINGS.worlds_offset , 0);
                break;
            case WorldType.GoodWorld:
                Debug.Log("GOOD");
                Debug.Log(SETTINGS.worlds_offset);
                transform.position += new Vector3(0, SETTINGS.worlds_offset , 0);
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

    public void ManageBullet(int amount)
    {
        currentBullets += amount;
        bulletText.text = currentBullets.ToString();
    }

    public void ManageHealth(int amount)
    {
        currentHealth += amount;
        healthManager.ModifyHealth(currentHealth);
    }
}

