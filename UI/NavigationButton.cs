using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavigationButton : MonoBehaviour, IPointerUpHandler
{
    public bool useTransition;
    public bool saveLastCanvas;
    public bool isBackButton;
    public Canvasses canvastTarget;
    private MenuManager menuManager;
    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.FindWithTag(CustomTags.MenuManager).GetComponent<MenuManager>();
    }

     void IPointerUpHandler.OnPointerUp(PointerEventData eventData){
        if(isBackButton){
            menuManager.GoBackCanvas(useTransition);
            return;
        }
        menuManager.SetCurrentCanvas(canvastTarget, saveLastCanvas, useTransition);
    }
}
