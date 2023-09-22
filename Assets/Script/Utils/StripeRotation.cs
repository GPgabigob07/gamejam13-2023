using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripeRotation : MonoBehaviour {

    public float scalar;
    private float increment = 0.001f;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward * scalar * increment * Time.unscaledDeltaTime * 10);
		increment += increment >= 1 ? 0.001f : -0.001f;
	}
}
