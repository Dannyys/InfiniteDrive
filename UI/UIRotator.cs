using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotator : MonoBehaviour
{
    private static UIRotator _instance;
    public static UIRotator instance { get { return _instance; }}

    public static Vector3 eulerRotation;
    
    private void Awake() {
        if(_instance == null) _instance = this;
    }

    private void OnDestroy() {
        _instance = null;
    }
    private void Start() {
        
    }

    void OnEnable(){
        transform.rotation = Quaternion.Euler(UIRotator.eulerRotation);
    }
    // Update is called once per frame
    void Update()
    {
        UIRotator.eulerRotation += new Vector3(0,1,0);
        if(UIRotator.eulerRotation.y == 360) UIRotator.eulerRotation = new Vector3(0,0,0);
        transform.rotation = Quaternion.Euler(UIRotator.eulerRotation);
        // transform.Rotate(0,1,0);
    }
}
