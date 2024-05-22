using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_particle_when_finished : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem == null)
        {
            Debug.LogError("No ParticleSystem found on the GameObject.");
            Destroy(this);
        }
    }

    void Update()
    {
        if (particleSystem != null && !particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}