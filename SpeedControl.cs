using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SpeedControl : MonoBehaviour
{
    public float currentSpeed = 0.0f;

    private float accelerationGoal = 0.0f;
    private bool stopping = false;
    public float metersPassed = 0.0f;
    public GameObject metersPassedObject;
    private Text metersPassedText;
    public float maxSpeed;
    private StringBuilder metersPassedStringBuilder;
    // Start is called before the first frame update
    void Start()
    {
        metersPassedText = metersPassedObject.GetComponent<Text>();
        metersPassedStringBuilder = new StringBuilder(10);
    }

    // Update is called once per frame
    void Update()
    {
        //debugging meterspassed calc *100;
        // metersPassed += currentSpeed * Time.deltaTime * 1 * 100;
        metersPassed += currentSpeed * Time.deltaTime * 1 ;
        metersPassedStringBuilder.Clear();
        // metersPassedText.text = Mathf.RoundToInt(metersPassed).ToString() + " M";
        metersPassedText.text = metersPassedStringBuilder.AppendFormat("{0} M", Mathf.RoundToInt(metersPassed)).ToString(); //.ToString() + " M";
        if(currentSpeed > accelerationGoal) currentSpeed = accelerationGoal;
        if(currentSpeed < 0 && stopping) currentSpeed = 0;
        if(stopping && currentSpeed > 0) currentSpeed -= Time.deltaTime * 15;
        if(currentSpeed < accelerationGoal && !stopping) currentSpeed += Time.deltaTime * 5 * (1 + (currentSpeed / 50));
        BackMover.backMovement = Vector3.back * Time.deltaTime * currentSpeed;
    }

    public void AccelerateBy(float speed){
        accelerationGoal += speed;
        if(accelerationGoal > maxSpeed) accelerationGoal = maxSpeed;
    }

    public void Stop(){
        stopping = true;
        accelerationGoal = 0.0f;
    }
}
