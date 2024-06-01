using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DoorBehaivour : NetworkBehaviour
{

    public Timer timer;
    public GameObject pressureplate1;
    public GameObject pressureplate2;
    public GameObject doorRight;
    public GameObject doorLeft;
    public Vector3 doorLeftInitPos;
    public Vector3 doorRightInitPos;
    private float distanceToTravel;
    private float tarjetDoorRight;
    private float tarjetDoorLeft;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if(!IsOwner){
            enabled = false;
            //pressureplate1.gameObject.SetActive(false);
            //pressureplate2.gameObject.SetActive(false);
            return;
        }

        doorLeftInitPos = doorLeft.transform.position;
        doorRightInitPos = doorRight.transform.position;

        distanceToTravel = Mathf.Abs(doorRight.transform.position.x * doorRight.transform.localScale.x);
        tarjetDoorRight = doorRight.transform.position.x + distanceToTravel;
        tarjetDoorLeft = doorLeft.transform.position.x - distanceToTravel;

        // pressureplate1.GetComponent<PressurePlate>();
        // pressureplate2.GetComponent<PressurePlate>();
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(pressureplate1.GetComponent<PressurePlate>().isPressed && pressureplate2.GetComponent<PressurePlate>().isPressed){
            if(doorLeft.transform.position.x >= tarjetDoorLeft){
                Vector3 newPosLeft = new Vector3(doorLeft.transform.position.x - Time.deltaTime, doorLeft.transform.position.y, doorLeft.transform.position.z);
                Vector3 newPosRight = new Vector3(doorRight.transform.position.x + Time.deltaTime, doorRight.transform.position.y, doorRight.transform.position.z);
                doorLeft.transform.position = newPosLeft;
                doorRight.transform.position= newPosRight;
            }
        }
        // else{
        //     if(doorLeft.transform.position.x <= doorLeftInitPos.x){
        //         Vector3 newPosLeft = new Vector3(doorLeft.transform.position.x + Time.deltaTime, doorLeft.transform.position.y, doorLeft.transform.position.z);
        //         Vector3 newPosRight = new Vector3(doorRight.transform.position.x -+ Time.deltaTime, doorRight.transform.position.y, doorRight.transform.position.z);
        //         doorLeft.transform.position = newPosLeft;
        //         doorRight.transform.position= newPosRight;
        //     }
        // }
    }
}
