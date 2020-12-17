using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float searchRadius;
    public float speed;
    public GameObject targetObject;

    private Vector3 m_targetPos = new Vector3(0, 0, 0); 

    void Start()
    {
        calcTarget();
    }

    // Update is called once per frame
    void Update()
    {
        var currentPosition = transform.localPosition;
        var targetDirection = (m_targetPos - currentPosition).normalized;
        transform.localPosition = transform.localPosition + speed * targetDirection * Time.deltaTime;

        // Has reached?
        if (hasReached())
        {
            Debug.Log("Calculating a new position");
            calcTarget();
        }
    }

    void calcTarget()
    {
        var r = Random.insideUnitCircle * searchRadius;

        // Calculate random position. 
        m_targetPos.x = m_targetPos.x + r.x;
        m_targetPos.z = m_targetPos.z + r.y;

        // Set the game object to target position.
        targetObject.transform.localPosition = m_targetPos;

        rotateBody(); 

        Debug.Log("New Position" + targetObject.transform.localPosition); 
    }

    bool hasReached()
    {
        var curPos = transform.localPosition;
        var d = Vector3.Distance(m_targetPos, curPos); 
        Debug.Log(d); 
        return d < 0.05; // Threshold distance.
    }

    void rotateBody() {
        // Calculate new direction. 
        var currentPosition = transform.localPosition;
        var targetDirection = m_targetPos - currentPosition;
        var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 999, 999);
        transform.localRotation = Quaternion.LookRotation(newDirection);
    }
}