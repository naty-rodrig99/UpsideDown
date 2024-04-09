using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement = new Vector2(0, 0);
    public float speed;
    public float speedJump;
    bool onGround;
    bool goodWorld;
    // Start is called before the first frame update
    void Start()
    {
        goodWorld = true;
        onGround = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        rb.AddForce(movement, ForceMode2D.Force);

        if (Input.GetButtonDown("Jump") && onGround)
        {
            rb.AddForce(new Vector2(0, speedJump), ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Fire1")) // change from good to bad
        {
            Debug.Log("FIRE");
            Debug.Log(goodWorld);
            switch (goodWorld)
            {
                case true:
                    Debug.Log("FIRE 1");
                    goodWorld = false;
                    transform.position += new Vector3(0, -15, 0);
                    break;
                case false:
                    Debug.Log("FIRE 1");
                    goodWorld = true;
                    transform.position += new Vector3(0, 15, 0);
                    break;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "platform")
        {
            onGround = true;
        }

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "platform")
        {
            onGround = false;
        }

    }
}
