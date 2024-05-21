using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using gamespace;
using TarodevController;

public class Enemies : MonoBehaviour
{
    public Animator monkeyAnimator;
    public GameObject main_player;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    BoxCollider2D overlapCollider;
    private bool collider_exists;
    string mode;
    int direction;
    public float speed;
    public int init_health = 3;
    private int _health;
    private Vector3 initial_pos;
    WorldType current_world;
    public float attack_power = 10.0f;
    private float _time;
    private float _timeSinceLastHit;

    void OnEnable()
    {
        WorldController.OnWorldChanged += UpdateWorld;
    }
    void OnDisable()
    {
        WorldController.OnWorldChanged -= UpdateWorld;
    }
    void UpdateWorld(WorldType type){
        current_world = type;
        switchWorld(type);
    }

    public void hitByBullet(){
        _health -= 1;
        if(_health <= 0) die();
    }
    void die(){
        Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        _health = init_health;
        current_world = WorldType.GoodWorld;
        initial_pos = transform.position;
        //monkeyAnimator.SetBool("goodWorld", false);
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        boxCollider = colliders[0];
        if (colliders.Length > 1)
        {
            overlapCollider = colliders[1];
            overlapCollider.isTrigger = true;
            collider_exists = true;
        }
        mode = "goodWorld";
        direction = 1;

    }

    // Update is called once per frame
    void FixedUpdate(){
        _time += Time.deltaTime;
        if(collider_exists && (_time - _timeSinceLastHit) > 0.5 && mode == "attack"){
            Collider2D[] hits = Physics2D.OverlapBoxAll(overlapCollider.bounds.center, overlapCollider.bounds.size, 0);
            foreach (Collider2D _hit in hits)
            {
                if (_hit.CompareTag("Player"))
                {
                    _timeSinceLastHit = _time;
                    PlayerController playerMovement = _hit.GetComponent<PlayerController>();
                    playerMovement.hitByEnemy(transform.position, attack_power);
                }
            }
        }

        int layerMask = ~(LayerMask.GetMask("EnemyLayer"));
        int enemyLayerMask = LayerMask.GetMask("EnemyLayer");

        RaycastHit2D hit_groundcheck = Physics2D.Raycast(boxCollider.bounds.center, new Vector2(direction,-1),Mathf.Sqrt(Mathf.Pow(boxCollider.bounds.extents.y , 2) + Mathf.Pow(boxCollider.bounds.extents.x, 2)) + 1.0f, layerMask);
        RaycastHit2D hit_friendlycheck = Physics2D.Raycast(boxCollider.bounds.center + new Vector3(direction * (boxCollider.bounds.extents.x + 0.3f),0.0f,0.0f), new Vector2(direction,0), 0.01f, enemyLayerMask);

        switch (mode)
        {
            case "goodWorld":
                monkeyAnimator.SetBool("goodWorld", true);
                monkeyAnimator.SetBool("monkeyMoves", true);
                break;
            
            case "badWorld":
                
                monkeyAnimator.SetBool("goodWorld", false);
                monkeyAnimator.SetBool("monkeyWalks", true);
                
                rb.velocity = new Vector2((float)(direction * speed), rb.velocity.y);

                if(hit_groundcheck.collider == null) // turn around when close to edge
                {
                    direction = direction * -1;
                    this.transform.Rotate(new Vector3(0, 180, 0));
                }
                if(hit_friendlycheck.collider != null && hit_friendlycheck.collider != boxCollider){ // turn around when walking in to other enemy
                    Debug.Log("finding col");
                    this.transform.Rotate(new Vector3(0, 180, 0));

                    direction = direction * -1;
                }
                    
                if(Mathf.Abs(main_player.transform.position.x - this.transform.position.x) < 5 && Mathf.Abs(main_player.transform.position.y - this.transform.position.y) < 2)
                {
                    mode = "attack";
                }
                break;

            case "attack":
                monkeyAnimator.SetBool("goodWorld", false);
                monkeyAnimator.SetBool("monkeyWalks", true);
                int attackSpeed = 3;
                //int layerMask = ~(LayerMask.GetMask("EnemyLayer"));
                //hit = Physics2D.Raycast(boxCollider.bounds.center, new Vector2(direction,-1),Mathf.Sqrt(Mathf.Pow(boxCollider.bounds.extents.y , 2) + Mathf.Pow(boxCollider.bounds.extents.x, 2)) + 1.0f, layerMask);
                if(hit_groundcheck.collider != null)
                {
                    Debug.Log("NONNULL");
                    rb.velocity = new Vector2((float)(direction * attackSpeed), rb.velocity.y);
                }else{
                    Debug.Log("null collider found");
                    rb.velocity = new Vector2(0.0f, 0.0f);
                }
                if (hit_friendlycheck.collider != null && hit_friendlycheck.collider != boxCollider)
                {
                    direction = direction * -1;
                    this.transform.Rotate(new Vector3(0, 180, 0));
                }
                if (Mathf.Abs(main_player.transform.position.x - this.transform.position.x) >= 5 || Mathf.Abs(main_player.transform.position.y - this.transform.position.y) >= 2)
                {
                    mode = "badWorld";
                }
                else if (Mathf.Abs(main_player.transform.position.x - this.transform.position.x) < 0.2)
                {
                    direction = 0;
                }
                else if (main_player.transform.position.x < this.transform.position.x)
                {
                    direction = -1;
                    //this.transform.Rotate(new Vector3(0, 180, 0));
                }
                else if(main_player.transform.position.x > this.transform.position.x)
                {
                    direction = 1;
                }
                break;

            case "getsShot":
                monkeyAnimator.SetBool("monkeyGetsShot", true);
                break;
        }
            
    }

    public void switchWorld(WorldType world)
    {
        if (world == WorldType.GoodWorld)
        {
            this.transform.position += new Vector3(0, 20, 0);
            mode = "goodWorld";
        }
        else if (world == WorldType.BadWorld)
        {
            this.transform.position += new Vector3(0, -20, 0);
            mode = "badWorld";
        }
    }
}