using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    SpeedControl speedController;
    // Start is called before the first frame update
    void Start()
    {
        speedController = GameObject.FindWithTag(CustomTags.SpeedControl).GetComponent<SpeedControl>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * speedController.currentSpeed * 15 * Time.deltaTime);
    }
}
