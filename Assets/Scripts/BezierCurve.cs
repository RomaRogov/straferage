using UnityEngine;
using System;

public class BezierCurve : MonoBehaviour {
    
	public Vector3[] points;

	public Vector3 GetPoint (float t) {
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}

	public Vector3 GetVelocity (float t) {
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
	}

	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

    public float GetPointValue(Vector3 point)
    {
        float kSum = 0f;
        for (int i = 0; i < 3; i++ )
        {
            Vector3 norm = transform.TransformVector(points[i + 1] - points[i]);
            Vector3 proj = Vector3.Project(point - transform.TransformPoint(points[i]), norm);
            float k = Mathf.Clamp(proj.magnitude, 0, norm.magnitude) / norm.magnitude;
            if (Vector3.Dot(proj, norm) < 0) { k = 0; }
            
            kSum += k;
        }
        return kSum / 3f;
    }

	public void Reset () {
		points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
	}
}