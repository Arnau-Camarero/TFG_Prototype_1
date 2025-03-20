using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerLives : NetworkBehaviour
{
    //Vida del jugador
    public int lives;
    public NetworkVariable<bool> isInjured = new NetworkVariable<bool>(false);

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {   
        lives = 3;

        isInjured.OnValueChanged += OnIsInjuredChanged;

        if (isInjured.Value)
        {
            SetInjuredState();
        }
        else
        {
            SetAliveState();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lives == 0 && !isInjured.Value)
        {
            SetInjuredState();
            isInjured.Value = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Injured" && gameObject.tag != "Injured")
        {
            // Reanimar
            collision.gameObject.GetComponent<Renderer>().material = GetComponent<PlayerMovement>().aliveMat;
            collision.gameObject.GetComponent<PlayerMovement>().speed = 5.0f;
            collision.gameObject.GetComponent<PlayerLives>().isInjured.Value = false;
            collision.gameObject.GetComponent<PlayerLives>().lives = 3;
            collision.gameObject.tag = "Player";
        }
    }

    private void OnIsInjuredChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            SetInjuredState();
        }
        else
        {
            SetAliveState();
        }
    }

    private void SetInjuredState()
    {
        GetComponent<Renderer>().material = GetComponent<PlayerMovement>().injuredMat;
        GetComponent<PlayerMovement>().speed = 1.5f;
        gameObject.tag = "Injured";
    }

    private void SetAliveState()
    {
        GetComponent<Renderer>().material = GetComponent<PlayerMovement>().aliveMat;
        GetComponent<PlayerMovement>().speed = 5.0f;
        gameObject.tag = "Player";
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        // Unsubscribe from the OnValueChanged event
        isInjured.OnValueChanged -= OnIsInjuredChanged;
    }
}
