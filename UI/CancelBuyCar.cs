using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CancelBuyCar : MonoBehaviour, IPointerUpHandler
{
    public GameObject buyCarCanvas;
    // Start is called before the first frame update

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData){
        buyCarCanvas.SetActive(false);
    }
}
