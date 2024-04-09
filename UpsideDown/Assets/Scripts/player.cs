using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc;
    Vector2 movement = new Vector2(0, 0);
    public float speed = 1;
    public float speedJump = 1;
    bool onGround = false;

    bool goodWorld = true;
    // Start is called before the first frame update
    void Start()
    {
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

        if (Input.GetButtonDown("Fire1") && goodWorld == true) // change from good to bad
        {
            Debug.Log("hello frends");
            goodWorld = false;
            transform.position = (new Vector3(0, -15, 0.5f));
        }
        if (Input.GetButtonDown("Fire1") && goodWorld == false) // change from bad to good
        {
            goodWorld = true;
            transform.position = (new Vector3(0, 15, 0.5f));
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
