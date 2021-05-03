using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Vector3 mousePos;
    public bool isLeft = false;

    Rigidbody rbody;
    Camera viewCamera;

    Vector3 velocity;
    public float moveSpeed = 6F;

    void Start() {
        rbody = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
    }

    void Update() {
        // mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        mousePos.z = 0;  // ScreenToWorldPoint is giving it camera transform (Z)
        // Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        // Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.z));
        // Debug.Log("Mouse: " + mousePos);

        // if (Input.mousePosition.x >= 0) {
        //     isLeft = false;
        // } else if (Input.mousePosition.x < 0) {
        //     isLeft = true;
        // }
        
        // Quaternion rot = Quaternion.Euler(0, -90, 0);
        // Vector3 temp =  rot * (mousePos + Vector3.up * transform.position.y);
        Vector3 temp = mousePos + Vector3.up * transform.position.y; // Check against tutorial
        transform.LookAt(temp);
        transform.Rotate(0, -90, 0);

        // transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        // transform.LookAt(mousePos);

        // transform.LookAt(mousePos + Vector3.forward * transform.position.y);
        // transform.Rotate(0, -90, 0);

        // Debug.Log(Vector3.up * transform.position.y);

        // transform.LookAt(mousePos);

        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * moveSpeed;
    }

    void FixedUpdate() {
        rbody.MovePosition(rbody.position + velocity * Time.fixedDeltaTime);

        // if (Mathf.Sign(velocity.x) == -1)  {
        //     isLeft = true;
        // } else if (Mathf.Sign(velocity.x) == 1 && velocity.x != 0) {
        //     isLeft = false;
        // }
    }
}
