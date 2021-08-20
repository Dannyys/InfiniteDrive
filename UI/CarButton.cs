using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum CarTypes{
    StationWagon,
    DeliveryBoy,
    MonsterTruck,
    OffRoadTruck,
    Derby,
    Rally,
    Stock,
    Sprint,
    MuscleSport,
    StreetRacer,
    SuperSport,
    FormulaOne
}
public class CarButton : MonoBehaviour, IPointerUpHandler
{
    private DataManager dataManager;
    private MenuManager menuManager;
    
    private Text unlockRecordText;
    private GameObject lockObj;
    public CarTypes carType;
    private bool isLocked;

    public int carValue;
    // Start is called before the first frame update


    void OnEnable(){
        dataManager = GameObject.FindWithTag(CustomTags.DataManager).GetComponent<DataManager>();
        menuManager = GameObject.FindWithTag(CustomTags.MenuManager).GetComponent<MenuManager>();
        lockObj = gameObject.transform.Find("Lock").gameObject;
        unlockRecordText = gameObject.transform.Find("UnlockRecordText").gameObject.GetComponent<Text>();
        
        unlockRecordText.text = "Cost: " + carValue.ToString() + " coins";
        // if(carType == CarTypes.FormulaOne) unlockRecordText.text = "unlock all cars";
        isLocked = true;
        if(carType == CarTypes.StationWagon || carType == CarTypes.DeliveryBoy){
            lockObj.SetActive(false);
            isLocked = false;
            SetCarData();
        }
        TryUnlockCar();
    }

    private void Update() {
        TryUnlockCar();
    }
    private void TryUnlockCar(){
        if((carType == CarTypes.MonsterTruck && dataManager.data.monsterTruck) 
        || (carType == CarTypes.OffRoadTruck && dataManager.data.offRoadTruck)
        || (carType == CarTypes.Derby && dataManager.data.derby)
        || (carType == CarTypes.Rally && dataManager.data.rally)
        || (carType == CarTypes.Stock && dataManager.data.stock)
        || (carType == CarTypes.Sprint && dataManager.data.sprint)
        || (carType == CarTypes.MuscleSport && dataManager.data.muscleSport)
        || (carType == CarTypes.StreetRacer && dataManager.data.streetRacer)
        || (carType == CarTypes.SuperSport && dataManager.data.superSport)
        || (carType == CarTypes.FormulaOne && dataManager.data.formulaOne)){
            lockObj.SetActive(false);
            isLocked = false;
            SetCarData();
        }
    }
    void SetCarData(){
        switch(carType){
            case CarTypes.StationWagon:
            unlockRecordText.text = "record: " + dataManager.data.stationWagonRecord + "m";
            break;
            case CarTypes.DeliveryBoy:
            unlockRecordText.text = "record: " + dataManager.data.deliveryBoyRecord + "m";
            break;
            case CarTypes.MonsterTruck:
            unlockRecordText.text = "record: " + dataManager.data.monsterTruckRecord + "m";
            break;
            case CarTypes.OffRoadTruck:
            unlockRecordText.text = "record: " + dataManager.data.offRoadTruckRecord + "m";
            break;
            case CarTypes.Derby:
            unlockRecordText.text = "record: " + dataManager.data.derbyRecord + "m";
            break;
            case CarTypes.Rally:
            unlockRecordText.text = "record: " + dataManager.data.rallyRecord + "m";
            break;
            case CarTypes.Stock:
            unlockRecordText.text = "record: " + dataManager.data.stockRecord + "m";
            break;
            case CarTypes.Sprint:
            unlockRecordText.text = "record: " + dataManager.data.sprintRecord + "m";
            break;
            case CarTypes.MuscleSport:
            unlockRecordText.text = "record: " + dataManager.data.muscleSportRecord + "m";
            break;
            case CarTypes.StreetRacer:
            unlockRecordText.text = "record: " + dataManager.data.streetRacerRecord + "m";
            break;
            case CarTypes.SuperSport:
            unlockRecordText.text = "record: " + dataManager.data.superSportRecord + "m";
            break;
            case CarTypes.FormulaOne:
            unlockRecordText.text = "record: " + dataManager.data.formulaOneRecord + "m";
            break;
        }
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData){
        // if(isLocked && carType == CarTypes.FormulaOne){
        //     menuManager.ShowCannotBuyCarCanvas();
        //     return;
        // }
        if(isLocked){
            menuManager.ShowBuyCarCanvas(carType, carValue);
            return;
        }
        dataManager.pickedCar = carType;
        menuManager.SwitchToGameScene();
    }
}
