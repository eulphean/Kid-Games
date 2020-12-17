using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation; 
using UnityEngine.XR.ARSubsystems; 
using UnityEngine; 


public class ObjectPlacement : MonoBehaviour
{  
    ARRaycastManager m_RaycastManager; 
    List<ARRaycastHit> s_Hits = new List<ARRaycastHit>(); 
    List<GameObject> runners = new List<GameObject>();

    bool hasInstantiatedObject = false; 

    // Assign this in the inspector. 
    public GameObject m_prefab;
    public RuntimeAnimatorController m_aniController;
    public Transform m_parent;

    

    void Start()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
       //if (!TryGetTouchPosition(out Vector2 touchPosition))
       //     return;

        if (Input.touchCount > 0)
        {
            // Get the current touch input. 
            var t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began && !hasInstantiatedObject)
            {
                raycast(t.position); 
            }

            if (t.phase == TouchPhase.Ended)
            {
                // Reset the flag. 
                hasInstantiatedObject = false; 
            }
        }
    }

    void raycast(Vector2 screenPos)
    {
        if (m_RaycastManager.Raycast(screenPos, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            // New object sorted under the parent. 
            var o = Instantiate(m_prefab, hitPose.position, hitPose.rotation, m_parent);
            var animator = o.GetComponent<Animator>();
            animator.runtimeAnimatorController = m_aniController;

            // Store in a collection. 
            runners.Add(o);

            // No more creation. 
            hasInstantiatedObject = true; 
        }

    }
}

//bool TryGetTouchPosition(out Vector2 touchPosition)
//{
//    if (Input.touchCount > 0)
//    {
//        touchPosition = Input.GetTouch(0).position;
//        return true;
//    }

//    touchPosition = default;
//    return false;
//}