using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class class_init : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("init WC");
        WorldController.init();
    }
}
