﻿using UnityEngine;
using UnityEngine.Networking;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;

public class PlayerController : NetworkBehaviour
{

    // Create public variables for player speed
    public float speed;


    // Create private references to the rigidbody component on the player
    private Rigidbody rb;


    // At the start of the game..
    void Start()
    {
        // Assign the Rigidbody component to our private rb variable
        rb = GetComponent<Rigidbody>();

    }

    // Each physics step
    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // Set some local float variables equal to the value of our Horizontal and Vertical Inputs
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed * Time.deltaTime, ForceMode.VelocityChange);

    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter(Collider other)
    {
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("Pick Up"))
        {
            // Make the other game object (the pick up) inactive, to make it disappear
            other.gameObject.SetActive(false);
        }
    }

}