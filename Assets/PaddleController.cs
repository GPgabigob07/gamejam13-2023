using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public Transform orb;
    public float radius;
    public Camera camera;
    public GameObject foll;

    private Transform pivot;

    void Start()
    {
        pivot = orb.transform;
        transform.parent = pivot;
        transform.position += Vector3.up * radius;
    }

    void Update()
    {
        Vector3 orbVector = camera.WorldToScreenPoint(orb.position);
        orbVector = Input.mousePosition - orbVector;
        float angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;

        foll.transform.position = transform.position;
        transform.position = orb.position;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
