using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller3D))]
public class Player : MonoBehaviour {

    Vector3 velocity;
    float gravity = -10;

    Controller3D controller;
	void Start () {
        controller = GetComponent<Controller3D>();
	}

    void Update ()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
