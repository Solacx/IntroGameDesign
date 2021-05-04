using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScriptBecauseBugs : MonoBehaviour
{
    public Player player;
    public LayerMask obstacleMask;
    private GameObject[] sceneObjects;

    void Start() {
        sceneObjects = GameObject.FindObjectsOfType<GameObject>();
    }

    void Update() {
        foreach (GameObject o in sceneObjects) {
            if (o.GetComponent<DisappearOnSight>()) {
                Vector3 currentPos = player.transform.position;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                
                Debug.DrawRay(currentPos, mousePos - currentPos);

                // Debug.DrawLine(player.transform.position, mousePos, Color.green);
                // Debug.DrawRay(player.transform.position, mousePos, Color.blue);

                RaycastHit2D collision = Physics2D.Raycast(currentPos, mousePos - currentPos, 6, obstacleMask);
                if (collision) {
                    if (collision.transform.gameObject.GetComponent<DisappearOnSight>()) {
                        if (player.isEyesObtained) {
                            collision.transform.gameObject.GetComponent<DisappearOnSight>().Disappear();
                        }
                    }
                }




            //     RaycastHit2D collision = Physics2D.Raycast(castOrigin, Vector2.right * moveDirection, castRange, collisionMask);

            // Debug.DrawRay(castOrigin, Vector2.right * moveDirection, Color.red);

            // if (collision) {
            //     float slopeAngle = Vector2.Angle(collision.normal, Vector2.up);
                
            //     if (collision.distance == 0) {

            //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //     RaycastHit hit;
            //     Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            //     if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            //         Debug.Log(hit.collider.name);
            //     }
            }
        }
    }
}
