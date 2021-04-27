using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float lookAtX;
    public float lookAtY;
    public float lookAtZ;

    Rigidbody rbody;
    Camera viewCamera;

    Vector3 velocity;
    public float moveSpeed = 6F;

    void Start() {
        rbody = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
    }

    void Update() {
        // Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        // Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        // Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.z));
        // transform.LookAt(mousePos + Vector3.right * transform.position.y);

        // Debug.Log(Vector3.up * transform.position.y);

        // transform.LookAt(new Vector3(lookAtX, lookAtY, lookAtZ));

        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * moveSpeed;
    }

    void FixedUpdate() {
        rbody.MovePosition(rbody.position + velocity * Time.fixedDeltaTime);

        if (Mathf.Sign(velocity.x) == -1)  {
            transform.eulerAngles = new Vector3(0, 270, 0);
        } else if (Mathf.Sign(velocity.x) == 1 && velocity.x != 0) {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }
}
