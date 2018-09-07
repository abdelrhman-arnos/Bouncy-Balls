using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller3D))]
public class Player : MonoBehaviour {

    public float moveSpeed = 6, rotateSpeed = 60;

    Vector3 velocity;
    float gravity = -20;
    Controller3D controller;

	void Start () {
        controller = GetComponent<Controller3D>();
	}

    void Update ()
    {
        UpdateVelocity();
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0f, -rotateSpeed * Time.deltaTime, 0f);
        }
        UpdateVelocity();
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        }
    }
    void UpdateVelocity()
    {
        //Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        velocity.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        velocity.z = Input.GetAxisRaw("Vertical") * moveSpeed;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
