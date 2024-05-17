using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignScript : MonoBehaviour
{
    TextMesh message;
    GameObject bgSquare;

    private SwitchBarUIBar sb_ui;

    public float addCharge = 0;

    // Start is called before the first frame update
    void Start()
    {
        Transform m2Transform = gameObject.transform.Find("m2");
        message = m2Transform.gameObject.GetComponent<TextMesh>();
            
        Transform bgSquareTransform = gameObject.transform.Find("bg_square");
        bgSquare = bgSquareTransform.gameObject;

        message.GetComponent<Renderer>().enabled = false;
        bgSquare.GetComponent<Renderer>().enabled = false;

        sb_ui = GameObject.Find("SwitchChargeHUD").GetComponent<SwitchBarUIBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            message.GetComponent<Renderer>().enabled = true;
            bgSquare.GetComponent<Renderer>().enabled = true;
            sb_ui.update_switch_ui_bar(addCharge);
            addCharge = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            message.GetComponent<Renderer>().enabled = false;
            bgSquare.GetComponent<Renderer>().enabled = false;
            
        }
    }
}
