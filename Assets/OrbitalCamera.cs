using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour {

    float pitch = 0;
    float yaw = 0;

    public float pitchMin = 0;
    public float pitchMax = 80;

    public float yawMin = -80;
    public float yawMax = 80;

    public float pitchSensitivity = 1;
    public float yawSensitivity = 5;

    public bool pitchInvert = true;
    public bool yawInvert = false;
    /// <summary>
    /// The target local position value for the child camera object
    /// </summary>
    private Vector3 dollyPosition;
    float dollyEaseMultiplier = 10;
    public float dollyMaxDistance = 20;
    public float dollyMinDistance = 10;

    public float pitchPercent
    {
        get
        {
            return (pitch - pitchMin) / (pitchMax - pitchMin);
        }
    }

    Transform cam;
    void Start () {
        cam = GetComponentInChildren<Camera>().transform;
        dollyPosition = cam.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
        LookAround();

        Zoom();
    }

    private void Zoom()
    {
        dollyPosition += new Vector3(0, 0, Input.mouseScrollDelta.y);

        dollyPosition.z = Mathf.Clamp(dollyPosition.z, -dollyMaxDistance, -dollyMinDistance);


        cam.localPosition = Vector3.Lerp(cam.localPosition, dollyPosition, Time.deltaTime * dollyEaseMultiplier);
    }

    private void LookAround()
    {
        float lookX = Input.GetAxis("Mouse X");
        float lookY = Input.GetAxis("Mouse Y");

        pitch += lookY * pitchSensitivity * (pitchInvert ? -1 : 1);
        yaw += lookX * yawSensitivity * (yawInvert ? -1 : 1);

        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        transform.eulerAngles = new Vector3(pitch, yaw, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
