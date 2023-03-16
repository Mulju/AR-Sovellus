using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[RequireComponent(requiredComponent: typeof(ARRaycastManager),
                  requiredComponent2: typeof(ARPlaneManager))]
public class ARPlaceObjectWithTiming : MonoBehaviour
{
    [SerializeField]
    private GameObject duckPrefab, bombPrefab;

    private GameObject arPrefab;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private float touchTime = 0;
    private bool placeState = true, duckState = true;

    [SerializeField]
    private float minTouchTime;

    [SerializeField]
    private Button placementButton, duckButton;

    [SerializeField]
    private Quest quest;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        placementButton.onClick.AddListener(SetPlaceState);
        duckButton.onClick.AddListener(SetCastlePlacement);
        arPrefab = duckPrefab;
    }



    // Update is called once per frame
    void Update()
    {
        // Vaan eka kosketus
        Touch touch = Input.touches[0];

        if(Input.touches.Length == 1 && placeState)
        {
            // Jos ruutua koskettaa vain yksi sormi.
            // N‰in v‰ltet‰‰n objektien luonti kun yritet‰‰n zoomata

            if (touch.phase == TouchPhase.Began)
            {
                touchTime = Time.time;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (Time.time - touchTime <= minTouchTime)
                {
                    // Ei tehd‰ mit‰‰n jos n‰p‰ytet‰‰n n‰yttˆ‰
                }
                else
                {
                    // Pidempi painallus
                    if(raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                    {
                        foreach (ARRaycastHit hit in hits)
                        {
                            Pose pose = hit.pose;
                            GameObject obj = Instantiate(arPrefab, pose.position, pose.rotation);

                            if(duckState)
                            {
                                quest.placedDucks++;
                            }
                        }
                    }
                }
            
                touchTime = 0;
            }
        }

    }

    // Asettaa voiko n‰kym‰ss‰ asettaa uusia objekteja
    private void SetPlaceState()
    {
        placeState = !placeState;

        if(!placeState)
        {
            // Nappi himme‰mm‰ksi
            placementButton.image.color = new Color(placementButton.image.color.r, placementButton.image.color.g, placementButton.image.color.b, 0.5f);
            placementButton.GetComponentInChildren<TMP_Text>().text = "Not\nplacing\nobjects";
        }
        else
        {
            // Nappi n‰kyv‰ksi
            placementButton.image.color = new Color(placementButton.image.color.r, placementButton.image.color.g, placementButton.image.color.b, 1f);
            placementButton.GetComponentInChildren<TMP_Text>().text = "Placing\nobjects";
        }
    }
    
    // Asettaa asetatko n‰kym‰ss‰ linnaa vai pommia
    private void SetCastlePlacement()
    {
        duckState = !duckState;

        if(duckState)
        {
            arPrefab = duckPrefab;
            duckButton.GetComponentInChildren<TMP_Text>().text = "Duck";
        }
        else
        {
            arPrefab = bombPrefab;
            duckButton.GetComponentInChildren<TMP_Text>().text = "Bomb";
        }
    }
}
