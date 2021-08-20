using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimation : MonoBehaviour
{
    private Vector3 originalScale;
    public void StartAnimation(){
        originalScale = transform.localScale;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        StartCoroutine(animationLoop());
    }
    IEnumerator animationLoop(){
        WaitForSeconds delay = new WaitForSeconds(0.01f);
        while(transform.localScale.x < originalScale.x
        || transform.localScale.y < originalScale.y
        || transform.localScale.z < originalScale.z){
            Vector3 toAdd = new Vector3(0,0,0);
            if(transform.localScale.x < originalScale.x) toAdd.x = 0.1f;
            if(transform.localScale.y < originalScale.y) toAdd.y = 0.1f;
            if(transform.localScale.z < originalScale.z) toAdd.z = 0.1f;
            transform.localScale += toAdd;
            yield return delay;    
        }
        transform.localScale = originalScale;
    }
}
