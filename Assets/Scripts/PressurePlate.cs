using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isPressed = false;

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            isPressed = true;
        }
    }

    void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Player"){
            isPressed = false;
        }
    }
}
