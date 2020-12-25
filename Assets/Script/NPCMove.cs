using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class NPCMove : MonoBehaviour
{
    [SerializeField]
    Transform _destination;

    [SerializeField]
    float _radius; 
    NavMeshAgent _navMeshAgent; 

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null) {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name); 
        } else {
            SetDestination(); 
        }
    }

    private void SetDestination()
    {
        if (_destination != null) {
            Vector3 target = _destination.transform.position; 
            _navMeshAgent.SetDestination(target);
        }
    }

    void Update() {
        // Has the agent reached?
        // if (_navMeshAgent.remainingDistance == 2) {
        //     calculateNewPosition();
        // }
         Vector3 target = _destination.transform.position;
        _navMeshAgent.SetDestination(target);
    }

    private void calculateNewPosition()
    {
        var pos = UnityEngine.Random.insideUnitCircle * _radius;
        Vector3 target = new Vector3(pos.x, 0, pos.y); 
        _destination.transform.position = target; 
        _navMeshAgent.SetDestination(target);
    }
}
