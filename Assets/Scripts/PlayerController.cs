using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private float currentSpeed;
    private Rigidbody rb;

    Vector3 movement;

    public float x;
    public float z;

    void Start()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("FullDayTrack");
        TimeHandler.OnDayChanged += ReturnToSpawn;
        x = transform.position.x;
        z = transform.position.z;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        x = transform.position.x;
        z = transform.position.z;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        movement.y = 0;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void ReturnToSpawn(TimeHandler.DateTime dateTime) {
        transform.position = new(0, 0.91f, 0); // Default coordinates of spawn chunk.
    }
}
