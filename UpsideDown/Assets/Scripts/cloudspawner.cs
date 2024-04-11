using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cloudspawner : MonoBehaviour
{

    public GameObject[] clouds;
    public Vector2 center;
    public Vector2 size;
    public int amount;

    private string current_world;

    public GameObject[] Background;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCloud(amount);
    }

    void SpawnCloud(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // Generate a local position within the bounds
            Vector2 localPosition = new Vector2(
                Random.Range(-size.x / 2, size.x / 2),
                Random.Range(-size.y / 2, size.y / 2)
            );

            // Convert local position to world position
            Vector3 worldPosition = transform.TransformPoint(new Vector3(localPosition.x + center.x, localPosition.y + center.y, 0));

            // Instantiate the cloud prefab at the world position
            GameObject cloudPrefab = clouds[Random.Range(0, clouds.Length)];
            GameObject cloud = Instantiate(cloudPrefab, worldPosition, Quaternion.identity, transform);

            // Since the cloud is already a child, adjust its local position to the initially calculated local position
            cloud.transform.localPosition = new Vector3(localPosition.x, localPosition.y, 0);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 1.0f);
        Gizmos.DrawWireCube(center, size);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
