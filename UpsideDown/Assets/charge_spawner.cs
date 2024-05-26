using System.Collections;
using UnityEngine;

public class charge_spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Vector3 spawnPosition;
    private Vector3 _spawnPos;
    public float spawnInterval = 2.0f; 
    public bool hasCharge;

    void Start()
    {
        _spawnPos = new Vector3(transform.position.x + spawnPosition.x, transform.position.y + spawnPosition.y, transform.position.z);
        hasCharge = false;
        StartCoroutine(SpawnCharge());
    }

    public void pickedUp()
    {
        hasCharge = false;
        StartCoroutine(SpawnCharge());
    }

    IEnumerator SpawnCharge()
    {
        if (!hasCharge)
        {
            yield return new WaitForSeconds(spawnInterval); // Wait before spawning a new charge
            spawn_charge();
        }
    }

    void spawn_charge()
    {
        Debug.Log("Spawning new");
        hasCharge = true;
        GameObject charge = Instantiate(prefabToSpawn, _spawnPos, Quaternion.identity, transform);
        charge_pickup_script prefabScript = charge.GetComponent<charge_pickup_script>();
        if (prefabScript != null)
        {
            prefabScript.SetParentScript(this);
        }
    }
}
