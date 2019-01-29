using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetAndFade : MonoBehaviour
{
    public Transform target;
    public bool fadeable;
    public bool shrinkable;
    public float timeToTarget = 3f;
    public float closenessThreshold = .1f;

    private bool hasMoveStarted;
    private Transform startingPosition;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasMoveStarted == false)
            return;

        t += Time.deltaTime / timeToTarget;
        transform.position = Vector3.Lerp(startingPosition.position, target.position, t);

       if (Vector3.Distance(transform.position, target.position) < closenessThreshold)
           Destroy(gameObject);
    }

    public void StartMoving()
    {
        startingPosition = gameObject.transform;
        hasMoveStarted = true;
    }
}
