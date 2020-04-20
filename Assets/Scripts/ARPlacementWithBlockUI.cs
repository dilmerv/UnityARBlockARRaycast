using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlacementWithBlockUI : MonoBehaviour
{
    [SerializeField]
    private GameObject placedPrefab;

    [SerializeField]
    private GameObject uiPanel;

    [SerializeField]
    private Button toggleButton;

    [SerializeField]
    private TextMeshProUGUI log;

    private ARRaycastManager arRaycastManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    public void Toggle()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
        var toggleButtonText = toggleButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        toggleButtonText.text = uiPanel.activeSelf ? "UI OFF" : "UI ON";
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                var touchPosition = touch.position;

                bool isOverUI = touchPosition.IsPointOverUIObject();
                
                if(isOverUI)
                {
                    log.text = $"{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")} blocked raycast";
                }

                if(!isOverUI && arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = hits[0].pose;
                    Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                }
            }
        }
    }
}
