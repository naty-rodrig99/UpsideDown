using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charge_pickup_script : MonoBehaviour
{

    private SwitchBarUIBar sb_ui;

    public float addCharge = 30;

    private charge_spawner _parentScript;

    // Start is called before the first frame update
    void Start()
    {
        sb_ui = GameObject.Find("SwitchChargeHUD").GetComponent<SwitchBarUIBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParentScript(charge_spawner script)
    {
        _parentScript = script;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("PICK UP");
        if (other.gameObject.tag == "Player")
        {
            if(_parentScript != null) _parentScript.pickedUp();
            sb_ui.update_switch_ui_bar(addCharge);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
    }
}
