using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
[RequireComponent(typeof(ARSessionOrigin))]
public class ImageManager : MonoBehaviour
{

    ARTrackedImageManager m_TrackedImageManager;


    public GameObject placedPrefab;
    private GameObject _instance = null;
    private Pose _pose;
    private ARSessionOrigin _sessionOrigin; 

    void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        _sessionOrigin = GetComponent<ARSessionOrigin>(); 
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
        if (_instance == null) {
            Vector3 position = trackedImage.transform.localPosition;
            Quaternion rotation = trackedImage.transform.localRotation;

            _pose = new Pose(position, rotation);

            _instance = Instantiate(placedPrefab, trackedImage.transform);
            _instance.name = placedPrefab.name;

            if (_instance.GetComponent<ARAnchor>() == null)
            {
                _instance.AddComponent<ARAnchor>();
            }

            _sessionOrigin.MakeContentAppearAt(_instance.transform, _pose.position, _pose.rotation);

        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateInfo(trackedImage);
        }
    }
}
