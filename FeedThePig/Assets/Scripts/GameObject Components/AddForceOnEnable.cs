using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AddForceOnEnable : MonoBehaviour
{
    [SerializeField]
    private float forceYMin;
    [SerializeField]
    private float forceYMax;

    [SerializeField]
    private float rotationMin;
    [SerializeField]
    private float rotationMax;

    private void OnEnable()
    {
        var rb = GetComponent<Rigidbody2D>();

        if(Random.Range(1, 100) > 50)
            transform.Rotate(new Vector3(0, 0, Random.Range(rotationMin, rotationMax)));
        else
            transform.Rotate(new Vector3(0, 0, -Random.Range(rotationMin, rotationMax)));

        rb.AddRelativeForce(transform.up * Random.Range(forceYMin, forceYMax));
    }
}
