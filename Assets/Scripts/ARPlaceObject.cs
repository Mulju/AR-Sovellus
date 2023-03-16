using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.Touch;

// Tehty tutoriaalin perusteella
// https://www.youtube.com/watch?v=lYDfV-GaKQA

[RequireComponent(requiredComponent: typeof(ARRaycastManager), 
                  requiredComponent2: typeof(ARPlaneManager))]
public class ARPlaceObject : MonoBehaviour
{
    [SerializeField]
    private GameObject arPrefab;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }



    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }


    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    // EnhancedTouch turha parametrikutsussa?
    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if(finger.index != 0)
        {
            // Palataan funktiosta jos useampi kuin yksi sormi koskee n‰yttˆ‰
            // Emme t‰ll‰ kertaa halua tukea multitouchia
            return;
        }

        // TrackableType.PlaneWithinPolygon m‰‰ritt‰‰ raycastin osumaan vain AR:n luomiin planeihin
        if (raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            foreach(ARRaycastHit hit in hits)
            {
                Pose pose = hit.pose;
                GameObject obj = Instantiate(arPrefab, pose.position, pose.rotation);

            }
        }
    }

}
