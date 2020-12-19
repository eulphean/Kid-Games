using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float speed;
    public GameObject targetObject;

    private Vector3 m_targetPos = new Vector3(0, 0, 0); 
    private Animator m_animator;
    private Callback m_calcTarget; 

    void Start()
    {
        // Access the animation from the child Agent component.
        m_animator = transform.GetChild(0).GetComponent<Animator>();
        m_animator.Play("Running01");
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
            Vector2 target = m_calcTarget();
            this.setTarget(target); 

        }
    }

    public void setTarget(Vector2 targetPos)
    {
        // Set a new target. 
        m_targetPos.x = targetPos.x;
        m_targetPos.z = targetPos.y;

        rotateBody();
    }

    public void setCallback(Callback calcTarget)
    {
        m_calcTarget = calcTarget; 
    }

    bool hasReached()
    {
        var curPos = transform.localPosition;
        var d = Vector3.Distance(m_targetPos, curPos); 
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

//var r = Random.insideUnitCircle * searchRadius;

//// Calculate random position. 
//m_targetPos.x = m_targetPos.x + r.x;
//m_targetPos.z = m_targetPos.z + r.y;

//// Set the game object to target position.
//targetObject.transform.localPosition = m_targetPos;

//if (Input.GetKeyDown("1"))
//{
//    m_animator.Play("Idle01");
//}

//if (Input.GetKeyDown("2"))
//{
//    m_animator.Play("Walking01");
//}