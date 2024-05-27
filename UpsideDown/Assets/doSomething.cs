using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doSomething : MonoBehaviour
{
    public GameObject ui;
    private SwitchBarUIBar _uiScript;
    // Start is called before the first frame update
    void Start()
    {
        _uiScript = ui.GetComponent<SwitchBarUIBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _uiScript.autoCharging = true;
            Destroy(gameObject);
        }
    }
}
