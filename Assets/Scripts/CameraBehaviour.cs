using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehaviour : MonoBehaviour
{
    // The two components necessary to manipulate the camera
    public GameObject               mainCam;
    public GameObject               playerChar;

    //
    public float                    cameraStoredTime;
    public float                    cameraSnapTime = 1;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            float realSnapTime = Time.time - cameraStoredTime;

            if (realSnapTime >= (cameraSnapTime - 0.5) || realSnapTime <= (cameraSnapTime + 0.5))
            {
                Debug.Log("CamPos: " + mainCam.transform.position);
                Debug.Log("CharPos: "+ playerChar.transform.position);
                mainCam.transform.position -= new Vector3 (23, 23, 0);
                // playerChar.transform.position
            }

        }
        else
            cameraStoredTime = Time.time;
    }
}
