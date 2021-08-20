using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public PlayerData data;
    public CarTypes pickedCar;
    public bool lastSceneWasGame;
    // Start is called before the first frame update
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        lastSceneWasGame = false;
        //SaveSystem.DestroyData();
        LoadData();
    }

    public void SaveData(){
        SaveSystem.SaveData(data);
    }

    public void LoadData(){
        data = SaveSystem.LoadData();
    }
}
