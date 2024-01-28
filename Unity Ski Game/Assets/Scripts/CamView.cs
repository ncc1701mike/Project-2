using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamView : MonoBehaviour
{
    public Camera xrCam;
    public CinemachineVirtualCamera followCam;
    private SkiGameInputActions skiGameInputActions;

    private void Awake()
    {
        skiGameInputActions = new SkiGameInputActions();
        skiGameInputActions.GamePlay.ToggleView.performed += _ => ToggleCameraView();
    }


    private void OnEnable()
    {
        skiGameInputActions.Enable();
    }


    private void OnDisable()
    {
        skiGameInputActions.Disable();
    }


    private void ToggleCameraView()
    {
        bool isXRCamActive = xrCam.gameObject.activeSelf;

        xrCam.gameObject.SetActive(!isXRCamActive);
        followCam.gameObject.SetActive(isXRCamActive);
    }
}