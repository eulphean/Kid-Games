using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
   Idle,
   Run,
   Walk
}

public class Agent : MonoBehaviour
{
    // Properties from the inspector. 
    public float speed;
    public GameObject targetObject;

    // Internal variables. 
    private Callback m_calcTarget;
    private Vector3 m_targetPos; 
    private Animator m_animator;
    private State m_state;
    private double elapsedTime;
    private float m_curSpeed;

    void Start()
    {
        m_targetPos = new Vector3(0, 0, 0); 
        m_curSpeed = speed;

        // Calculate new state 
        float p = Random.Range(0.0f, 1.0f) ;
        m_state = p <= 0.5 ? State.Walk : State.Run;
        this.setTarget(m_calcTarget(gameObject.name));
        
        // Access the animation from the child Agent component.
        m_animator = transform.GetChild(0).GetComponent<Animator>();
        this.setAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        var currentPosition = transform.localPosition;
        var targetDirection = (m_targetPos - currentPosition).normalized;
        transform.localPosition = transform.localPosition + m_curSpeed * targetDirection * Time.deltaTime;

        // Has reached?
        if (hasReached())
        {
            if (m_state == State.Walk || m_state == State.Run)
            {
                // Spend time idling. 
                m_state = State.Idle;
                elapsedTime = 0;
                m_curSpeed = 0;
            } else if (elapsedTime > 5) {
                float p = Random.Range(0.0f, 1.0f);
                m_state = p <= 0.5 ? State.Run : State.Walk;

                // New target. 
                Vector2 target = m_calcTarget(gameObject.name);
                this.setTarget(target);

                m_curSpeed = speed;
            }

            this.setAnimation();
        }

        elapsedTime = (m_state == State.Idle) ? elapsedTime + Time.deltaTime : 0;
    }

    bool hasReached()
    {
        var curPos = transform.localPosition;
        var d = Vector3.Distance(m_targetPos, curPos); 
        return d < 0.05; // Threshold distance.
    }

    void rotateBody()
    {
        var currentPosition = transform.localPosition;
        var targetDirection = (m_targetPos - currentPosition).normalized;
        transform.localRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
    }

    public void setTarget(Vector2 targetPos)
    {
        // Set a new target. 
        m_targetPos.x = targetPos.x;
        m_targetPos.z = targetPos.y;

        this.rotateBody();
    }

    public void setCallback(Callback calcTarget)
    {
        m_calcTarget = calcTarget;
    }

    void setAnimation()
    {
        switch (m_state)
        {
            case State.Run:
            {
                m_animator.Play("Running01");
                break;
            }
            case State.Walk:
            {
                m_animator.Play("Walking01");
                break;
            }
            case State.Idle:
            {
                m_animator.Play("Idle01");
                break;
            }
            default:
            {
                break;
            }
        }
    }
}