using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageManager : MonoBehaviour
{

    ARTrackedImageManager m_TrackedImageManager;
    public GameObject placedPrefab; 

    void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }


    void UpdateInfo(ARTrackedImage trackedImage)
    {
        Vector3 position = trackedImage.transform.position;
        Quaternion rotation = trackedImage.transform.rotation; 

        GameObject g = Instantiate(placedPrefab, position, rotation);
        g.name = placedPrefab.name;
        g.transform.SetParent(trackedImage.transform);
        g.SetActive(true);
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateInfo(trackedImage);
        }
    }
}
