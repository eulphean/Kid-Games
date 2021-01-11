using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    [SerializeField]
    public float radius;
    public List<TargetPoint> neighbors; 

    void OnDrawGizmos() {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(gameObject.transform.localPosition, radius);
    }
}
