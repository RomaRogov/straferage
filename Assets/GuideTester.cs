using UnityEngine;
using System;
using System.Collections;

[ExecuteInEditMode]
public class GuideTester : MonoBehaviour
{
    private float guideRotation;

    public string GuideName;

    void Update ()
    {
        BezierCurve guide = null;
        Array.ForEach(Physics.OverlapSphere(transform.position, 1f), col => { if (col.isTrigger) { guide = col.GetComponent<BezierCurve>(); } });

        if (guide != null)
        {
            guideRotation = Mathf.LerpAngle(guideRotation, Quaternion.LookRotation(guide.GetDirection(guide.GetPointValue(transform.position))).eulerAngles.y, Time.fixedDeltaTime * 10f);
            GuideName = guide.name;
        }
        else
        {
            GuideName = "NULL";
        }
        transform.rotation = Quaternion.Euler(Vector3.up * (guideRotation));
    }
}
