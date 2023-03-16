using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]

public class PlaceTrackedImages : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    public GameObject[] arPrefabs;
    
    private readonly Dictionary<string, GameObject> instantiatedPrefabs = new Dictionary<string, GameObject>();


    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Funktiossa 3 statea
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Jos uusia kuvia tullut näkymään, jotka löytyy librarystä, haetaan sen kuvan nimi
        // ja luodan prefabi näkyviin
        foreach(var trackedImage in eventArgs.added)
        {
            var imageName = trackedImage.referenceImage.name;

            foreach(var curPrefab in arPrefabs)
            {
                if(string.Compare(curPrefab.name, imageName, StringComparison.OrdinalIgnoreCase) == 0
                    && !instantiatedPrefabs.ContainsKey(imageName))
                {
                    // Verrataan tunnistetun kuvan nimeä prefabiin
                    // Jos sitä ei vielä ollut luotu, luodaan sellainen
                    var newPrefab = Instantiate(curPrefab, trackedImage.transform);
                    instantiatedPrefabs[imageName] = newPrefab;
                }
            }
        }

        foreach(var trackedImage in eventArgs.updated)
        {
            // Jos jo näkyvissä oleviin kuviin tulee jotain muutoksia, muutetaan vain niitä
            instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }

        foreach(var trackedImage in eventArgs.removed)
        {
            // Jos kuva on kadonnut eikä tod näk tuu enää takaisin, tuhotaan se ja poistetaan dictionarystä
            Destroy(instantiatedPrefabs[trackedImage.referenceImage.name]);
            instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
        }
    }
}
