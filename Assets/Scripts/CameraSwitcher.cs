using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    void Start()
    {
        ThirdPersonView();
    }

    public void FirstPersonView()
    {
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
    }

    public void ThirdPersonView()
    {
        firstPersonCamera.enabled = false;
        thirdPersonCamera.enabled = true;
    }
}