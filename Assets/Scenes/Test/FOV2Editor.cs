using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FOV2))]
public class FOV2Editor : Editor
{
    void OnSceneGUI() {
        FOV2 view = (FOV2) target;

        Handles.color = Color.white;
        Handles.DrawWireArc(view.transform.position, Vector3.forward, Vector3.right, 360, view.viewRadius);

        Vector3 viewAngleA = view.DirFromAngle(-view.viewAngle / 2, false);
        Vector3 viewAngleB = view.DirFromAngle(view.viewAngle / 2, false);

        Handles.DrawLine(view.transform.position, view.transform.position + viewAngleA * view.viewRadius);
        Handles.DrawLine(view.transform.position, view.transform.position + viewAngleB * view.viewRadius);
    }
}
