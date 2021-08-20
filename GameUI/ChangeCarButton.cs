using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChangeCarButton : MonoBehaviour, IPointerUpHandler
{
    public GameObject uiCanvas;
    private GameUIManager uiManager;
    bool canClick;
    private void Start() {
        uiManager = uiCanvas.GetComponent<GameUIManager>();
    }
    private void OnEnable() {
        canClick = false;
        StartCoroutine(clickCooldown());
    }
    IEnumerator clickCooldown(){
        yield return new WaitForSeconds(2.0f);
        canClick = true;
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData){
        if(canClick)
            uiManager.ChangeCar();
    }
}
