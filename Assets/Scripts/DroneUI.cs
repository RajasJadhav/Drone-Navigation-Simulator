using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DroneUI : MonoBehaviour
{
    public DroneController drone;

    public TMP_Text speedValue;
    public TMP_Text altitudeValue;
    public TMP_Text statusValue;

    public Image statusCircle;

    void Update()
    {
        speedValue.text = drone.GetSpeed().ToString("F1") + " m/s";

        altitudeValue.text = drone.GetAltitude().ToString("F1") + " m";

        if (drone.isArmed)
        {
            statusValue.text = "ARMED";
            statusCircle.color = Color.green;
        }
        else
        {
            statusValue.text = "DISARMED";
            statusCircle.color = Color.red;
        }
    }
}