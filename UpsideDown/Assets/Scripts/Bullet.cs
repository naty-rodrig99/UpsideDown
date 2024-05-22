using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject bullet_expl_particle;
    public float wobbleAmplitude = 0.5f;
    public float wobbleFrequency = 10f;
    private float wobbleTimer = 0f; // Internal timer for the sine function
    public int direction;

    public float duration = 0.5f;
    private float _time;
    public float speedMultiplier = 1.03f;

    private Renderer bulletRenderer;
    private Collider2D bulletCollider;
    private TrailRenderer trailRenderer;

    private float _offset;

    // Start is called before the first frame update
    void Start()
    {
        bulletRenderer = GetComponent<Renderer>();
        bulletCollider = GetComponent<Collider2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        _offset = 0.0f ;// Random.Range(-1.0f, 1.0f) * Mathf.PI;
    }

    public void set_direction(int dir)
    {
        direction = dir;
    }

    void FixedUpdate(){
        _time += Time.deltaTime;
        if(_time > duration) StartCoroutine(DisableAndDestroy(duration + 0.5f));
        speed *= speedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);

        // Wobble effect
        wobbleTimer += Time.deltaTime;
        float wobbleEffect = Mathf.Sin((wobbleTimer + _offset) * wobbleFrequency) * wobbleAmplitude;

        // Apply the wobble effect to the bullet's position
        transform.position += new Vector3(0, wobbleEffect * Time.deltaTime, 0);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
        Instantiate(bullet_expl_particle, transform.position, Quaternion.identity);
        float trailTime = trailRenderer.time;

        StartCoroutine(DisableAndDestroy(trailTime + 0.5f));
    }

    IEnumerator DisableAndDestroy(float delay)
    {
        bulletCollider.enabled = false;
        bulletRenderer.enabled = false;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        //trailRenderer.enabled = false;
        speed = 0;

        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
