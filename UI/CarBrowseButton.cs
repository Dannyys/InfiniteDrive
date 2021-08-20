using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum BrowseDirection{
    Left,
    Right
}
public class CarBrowseButton : MonoBehaviour, IPointerUpHandler
{
    public BrowseDirection direction;
    private CarBrowser browser;
    // Start is called before the first frame update
    void OnEnable(){
        browser = GameObject.Find("PickCarCanvas").GetComponent<CarBrowser>();
    }

     void IPointerUpHandler.OnPointerUp(PointerEventData eventData){
        if(direction == BrowseDirection.Left){
            browser.BrowseLeft();
        }
        else{
            browser.BrowseRight();
        }
    }
}
