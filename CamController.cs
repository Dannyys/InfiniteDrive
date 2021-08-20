using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    Camera cam;
    public float desiredFoV = 120.0f;

    public float fixedHorizontalFOV = 100;
    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>(); 

        //either of these work
        cam.fieldOfView = desiredFoV / ((float)cam.pixelWidth / cam.pixelHeight);
        //cam.fieldOfView = 2 * Mathf.Atan(Mathf.Tan(fixedHorizontalFOV * Mathf.Deg2Rad * 0.5f) / GetComponent<Camera>().aspect) * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 30 == 0){
            cam.fieldOfView = desiredFoV / ((float)cam.pixelWidth / cam.pixelHeight);
        }
        //float normalAspect = 16/9f;
        //cam.fieldOfView = desiredFoV * normalAspect / ((float)cam.pixelWidth / cam.pixelHeight);
        
        //cam.fieldOfView = desiredFoV / ((float)cam.pixelWidth / cam.pixelHeight);

        // float aspectRatio = Screen.width / ((float)Screen.height);
        //  float percentage = 1 - (aspectRatio / KEEP_ASPECT);
 
        //  cam.rect = new Rect(0f, (percentage / 2), 1f, (1 - percentage));
        //cam.fieldOfView = desiredFoV / ((float)cam.pixelWidth / cam.pixelHeight);
        //cam.fieldOfView = 2 * Mathf.Atan(Mathf.Tan(fixedHorizontalFOV * Mathf.Deg2Rad * 0.5f) / GetComponent<Camera>().aspect) * Mathf.Rad2Deg;
        
    }
}
