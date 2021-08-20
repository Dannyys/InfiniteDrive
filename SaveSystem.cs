using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem{
    public static void SaveData(PlayerData data){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/dangeroadsave.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        // PlayerData data = new PlayerData();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void DestroyData(){
        string path = Application.persistentDataPath + "/dangeroadsave.data";
        File.Delete(path);
    }
    public static PlayerData LoadData(){
        string path = Application.persistentDataPath + "/dangeroadsave.data";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = (PlayerData)formatter.Deserialize(stream);
            stream.Close();
            return data;

        }else{
            PlayerData data = new PlayerData();
            data.coinDoubler = false;
            data.coins = 0;
            data.level = 0;
            data.exp = 0;
            data.sfxVolume = 0.5f;
            data.musicVolume = 0.5f;
            data.stationWagonRecord = 0;
            data.deliveryBoyRecord = 0;
            data.monsterTruckRecord = 0;
            data.offRoadTruckRecord = 0;
            data.derbyRecord = 0;
            data.rallyRecord = 0;
            data.stockRecord = 0;
            data.sprintRecord = 0;
            data.muscleSportRecord = 0;
            data.streetRacerRecord = 0;
            data.superSportRecord = 0;
            data.monsterTruck = false;
            data.offRoadTruck = false;
            data.derby = false;
            data.rally = false;
            data.stock = false;
            data.sprint = false;
            data.muscleSport = false;
            data.streetRacer = false;
            data.superSport = false;
            data.formulaOne = false;
            SaveSystem.SaveData(data);
            return data;
        }
    }
}
