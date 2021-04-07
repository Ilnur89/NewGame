using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    float movespeed = 6f;
    Rigidbody rigidbody;
    Camera viewcamera;
    Vector3 velosity;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        viewcamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = viewcamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewcamera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
        velosity = new Vector3(Input.GetAxisRaw("Vertical"), 0,Input.GetAxisRaw("Horizontal")).normalized * movespeed;

    }
    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velosity * Time.fixedDeltaTime);

    }
}
