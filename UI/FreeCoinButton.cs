using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FreeCoinButton : MonoBehaviour, IPointerUpHandler
{
  
    DataManager dataManager;
    private void OnEnable() {
        dataManager = GameObject.FindWithTag(CustomTags.DataManager).GetComponent<DataManager>();
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData){
        dataManager.data.coins += 50;
        dataManager.SaveData();
    }
}
