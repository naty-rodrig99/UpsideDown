using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    public GameObject bullet_expl_particle;
    public float wobbleAmplitude = 0.5f;
    public float wobbleFrequency = 10f;
    private float wobbleTimer = 0f; // Internal timer for the sine function
    public int direction;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }
    public void set_direction(int dir)
    {
        direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);

        // Wobble effect
        wobbleTimer += Time.deltaTime;
        float wobbleEffect = Mathf.Sin(wobbleTimer * wobbleFrequency) * wobbleAmplitude;

        // Apply the wobble effect to the bullet's position
        transform.position += new Vector3(0, wobbleEffect * Time.deltaTime, 0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("SOME COLLISION");
        if (other.gameObject.tag == "platform")
        {
            hit();
        }
        if (other.gameObject.tag == "Monster")
        {
            Enemies monster = other.gameObject.GetComponent<Enemies>();
            if (monster != null)
            {
                monster.hitByBullet();
            }            
            hit();
        }

    }
    void hit()
    {
        GameObject particle_instance = Instantiate(bullet_expl_particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
