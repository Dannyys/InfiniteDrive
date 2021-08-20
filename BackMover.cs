using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMover : MonoBehaviour
{
    private static BackMover _instance;
    public static BackMover instance { get { return _instance; }}
    public static Vector3 backMovement;
    private void Awake() {
        if(_instance == null) _instance = this;
    }
    private void OnDestroy() {
        _instance = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(BackMover.backMovement, Space.World);
    }
}
