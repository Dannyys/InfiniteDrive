using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag(CustomTags.GameManager).GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.z < -20){
            if(gameObject.CompareTag(CustomTags.Road))
                gameManager.RespawnRoad(gameObject);
            else if(gameObject.CompareTag(CustomTags.EnvironmentObject))
                gameManager.RespawnEnvironmentObject(gameObject);
            else if(gameObject.CompareTag(CustomTags.FoliageObject))
                gameManager.RespawnFoliageObject(gameObject);
            else if(gameObject.CompareTag(CustomTags.Coin)
                || gameObject.CompareTag(CustomTags.Fuel)
                || gameObject.CompareTag(CustomTags.Obstacle))
                gameObject.SetActive(false);
        }
    }
}
