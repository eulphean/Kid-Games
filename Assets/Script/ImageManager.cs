using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageManager : MonoBehaviour
{

    ARTrackedImageManager m_TrackedImageManager;


    public GameObject placedPrefab;
    private GameObject _instance = null;
    private Pose _pose;

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
        if (_instance == null) {
            Vector3 position = trackedImage.transform.localPosition;
            Quaternion rotation = trackedImage.transform.localRotation;
            _pose = new Pose(position, rotation);

            _instance = Instantiate(placedPrefab, _pose.position, _pose.rotation);
            _instance.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
            _instance.name = placedPrefab.name;

            _instance.isStatic = true;
            _instance.SetActive(true);

            _instance.transform.SetParent(trackedImage.transform);

            if (_instance.GetComponent<ARAnchor>() == null)
            {
                _instance.AddComponent<ARAnchor>();
            }
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
