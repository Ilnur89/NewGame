using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (vievfield))]
public class ViewEditor : Editor
{
    void OnSceneGUI()
    {
        vievfield fow = (vievfield)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360,fow.viewRadios);
        Vector3 viewAngleA = fow.FromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.FromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadios);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadios);
        Handles.color = Color.red;
        foreach(Transform visTarget in fow.visibleTarget)
        {
            Handles.DrawLine(fow.transform.position, visTarget.position);
        }
    }
    
}
