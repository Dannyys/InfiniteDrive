using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DirectionalButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float direction;
    public GameObject uiCanvas;
    private GameUIManager uIManager;

    private void Start() {
        uIManager = uiCanvas.GetComponent<GameUIManager>();
    }
    // Start is called before the first frame update
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData){
        uIManager.currentTouchInput += direction;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData){
        uIManager.currentTouchInput -= direction;
        // if(direction == -1)
        //     uIManager.RetryGame();
    }
}
