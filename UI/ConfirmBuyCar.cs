using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ConfirmBuyCar : MonoBehaviour, IPointerUpHandler
{
    private DataManager dataManager;
    public int carValue;
    public CarTypes carType;
    public GameObject buyCarCanvas;
    public MenuManager menuManager;
    // Start is called before the first frame update
    private void OnEnable() {
        dataManager = GameObject.FindWithTag(CustomTags.DataManager).GetComponent<DataManager>();   
        menuManager = GameObject.FindWithTag(CustomTags.MenuManager).GetComponent<MenuManager>();   
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData){
        if(dataManager.data.coins >= carValue){
            dataManager.data.coins -= carValue;
            EnableCar();
            dataManager.SaveData();
        }else{
            menuManager.ShowCannotBuyCarCanvas();
        }
        buyCarCanvas.SetActive(false);
    }

    void EnableCar(){
        switch(carType){
            case CarTypes.MonsterTruck:
            dataManager.data.monsterTruck = true;
            break;
            case CarTypes.OffRoadTruck:
            dataManager.data.offRoadTruck = true;
            break;
            case CarTypes.Derby:
            dataManager.data.derby = true;
            break;
            case CarTypes.Rally:
            dataManager.data.rally = true;
            break;
            case CarTypes.Stock:
            dataManager.data.stock = true;
            break;
            case CarTypes.Sprint:
            dataManager.data.sprint = true;
            break;
            case CarTypes.MuscleSport:
            dataManager.data.muscleSport = true;
            break;
            case CarTypes.StreetRacer:
            dataManager.data.streetRacer = true;
            break;
            case CarTypes.SuperSport:
            dataManager.data.superSport = true;
            break;
            case CarTypes.FormulaOne:
            dataManager.data.formulaOne = true;
            break;
        }
    }
}
