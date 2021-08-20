using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject stationWagonPrefab;
    public GameObject deliveryBoyPrefab;
    public GameObject monsterTruckPrefab;
    public GameObject offRoadTruckPrefab;
    public GameObject derbyPrefab;
    public GameObject formulaOnePrefab;
    public GameObject muscleSportPrefab;
    public GameObject rallyPrefab;
    public GameObject sprintPrefab;
    public GameObject stockPrefab;
    public GameObject streetRacerPrefab;
    public GameObject superSportPrefab;

    public GameObject uiCanvas;
    private GameUIManager uIManager;
    public GameObject roadPrefab;
    public GameObject environmentFloorPrefab;

    public GameObject[] treePrefabs;
    public GameObject[] groundPrefabs;
    public GameObject[] foliagePrefabs;
    // public int treeAmount = 100;
    // public int groundAmount = 25;
    // public int foliageAmount = 300;
        public int treeAmount = 1;
    public int groundAmount = 1;
    public int foliageAmount = 1;
    private Vector3 startLocation = new Vector3(0,0,-10);
    private Vector3 roadLength = new Vector3(0,0,5);
    private Vector3 spawnZPos;
    private float roadXOffset = 15.0f;
    private float roadXMaxOffset = 60.0f;
    private float roadXLength = 7.5f;
    private float currentCoinTimer;
    private float currentFuelTimer;
    private float startMaxCoinTimer = 8.0f;
    private float maxCoinTimer;
    private float maxFuelTimer = 10.0f;
    private int roadAmount = 25;
    private int lastObstacleRow = -1;
    private bool gameIsRunning;
    private GameObject lastSpawn;
    private ObjectPooler objectPooler;
    private SpeedControl speedController;
    private DataManager dataManager;
    private GameObject car;

    private List<GameObject> roadsToRespawn;
        
    // Start is called before the first frame update
    void Start()
    {
        roadsToRespawn = new List<GameObject>();
        gameIsRunning = true;
        objectPooler = ObjectPooler.Instance;
        speedController = GameObject.FindWithTag(CustomTags.SpeedControl).GetComponent<SpeedControl>();
        spawnZPos = startLocation + (roadAmount * roadLength);
        maxCoinTimer = startMaxCoinTimer;
        uIManager = uiCanvas.GetComponent<GameUIManager>();
        
        dataManager = GameObject.FindWithTag(CustomTags.DataManager).GetComponent<DataManager>();
        currentFuelTimer = 11.0f;

        lastSpawn = SpawnRoad(startLocation, false);
        for(int i = 1; i <= roadAmount; i++){
            lastSpawn = SpawnRoad(lastSpawn.transform.localPosition + roadLength, false);
        }
    
        for(int i = 1; i <= treeAmount; i++){
            SpawnEnvironmentObject(treePrefabs[Random.Range(0, treePrefabs.Length)]);
        }
        for(int i = 1; i <= groundAmount; i++){
            SpawnEnvironmentObject(groundPrefabs[Random.Range(0, groundPrefabs.Length)]);
        }
        for(int i = 1; i <= foliageAmount; i++){
            SpawnFoliageObject(foliagePrefabs[Random.Range(0, foliagePrefabs.Length)]);
        }
        //spawn a static ground for smoother road spawning
        SpawnRoad(startLocation + ((roadAmount-1) * roadLength), true);
        SpawnFloor();
        SpawnInitialObstacles(4);
        SpawnCar();
        uIManager.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(currentFuelTimer);
        maxCoinTimer = startMaxCoinTimer - (speedController.currentSpeed/10);
        if(maxCoinTimer < 2.0f) maxCoinTimer = 2.0f;
        TryRespawnRoad();
    }

    #region Start of game 
    public void StartGame(){
        car.GetComponent<PlayerController>().StartCar();
        StartCoroutine(SpawnLoop());
    }
    private void SpawnCar(){
        Vector3 location = new Vector3(0,0,0);
        Quaternion rotation = new Quaternion();
        switch(dataManager.pickedCar){
            case CarTypes.StationWagon:
            car = Instantiate(stationWagonPrefab, location, rotation);
            break;
            case CarTypes.DeliveryBoy:
            car = Instantiate(deliveryBoyPrefab, location, rotation);
            break;
            case CarTypes.Derby:
            car = Instantiate(derbyPrefab, location, rotation);
            break;
            case CarTypes.FormulaOne:
            car = Instantiate(formulaOnePrefab, location, rotation);
            break;
            case CarTypes.MonsterTruck:
            car = Instantiate(monsterTruckPrefab, location, rotation);
            break;
            case CarTypes.MuscleSport:
            car = Instantiate(muscleSportPrefab, location, rotation);
            break;
            case CarTypes.OffRoadTruck:
            car = Instantiate(offRoadTruckPrefab, location, rotation);
            break;
            case CarTypes.Rally:
            car = Instantiate(rallyPrefab, location, rotation);
            break;
            case CarTypes.Sprint:
            car = Instantiate(sprintPrefab, location, rotation);
            break;
            case CarTypes.Stock:
            car = Instantiate(stockPrefab, location, rotation);
            break;
            case CarTypes.StreetRacer:
            car = Instantiate(streetRacerPrefab, location, rotation);
            break;
            case CarTypes.SuperSport:
            car = Instantiate(superSportPrefab, location, rotation);
            break;
        }
        uIManager.playerController = car.GetComponent<PlayerController>();
        speedController.maxSpeed = car.GetComponent<PlayerController>().maxSpeed;
    }
    #endregion

    #region endof game
    
    public void StopGame(){
        gameIsRunning = false;
        uIManager.StopGame();
    }
    #endregion
    


    #region Coins, Fuel and Obstacle spawning
    IEnumerator SpawnLoop(){
        yield return new WaitForSeconds(4.2f);
        while(gameIsRunning){
            float timeToWait = 1.65f - (speedController.currentSpeed/100);
            if(timeToWait < 0.8f) timeToWait = 0.8f;
            SpawnObstacleRow(spawnZPos, true);
            if(currentCoinTimer >= maxCoinTimer) StartCoroutine(SpawnCoin(timeToWait/2));
            if(currentFuelTimer >= maxFuelTimer) StartCoroutine(SpawnFuel(timeToWait/2));
            yield return new WaitForSeconds(timeToWait);
            currentCoinTimer += timeToWait;
            currentFuelTimer += timeToWait;
        }
    }
    IEnumerator SpawnCoin(float timeToWait ){
        currentCoinTimer = 0.0f;
        yield return new WaitForSeconds(timeToWait);
        Vector3 location = new Vector3(Random.Range(-roadXLength, roadXLength),1,0) + spawnZPos;
        GameObject coin = objectPooler.SpawnFromPool(CustomTags.Coin, location, new Quaternion(), false);
        coin.GetComponent<SpawnAnimation>().StartAnimation();
    }
    IEnumerator SpawnFuel(float timeToWait ){
        currentFuelTimer = 0.0f;
        yield return new WaitForSeconds(timeToWait);
        Vector3 location = new Vector3(Random.Range(-roadXLength, roadXLength),1.15f,0) + spawnZPos;
        GameObject fuel = objectPooler.SpawnFromPool(CustomTags.Fuel, location, new Quaternion(), false);
        fuel.GetComponent<SpawnAnimation>().StartAnimation();
    }
    private void SpawnInitialObstacles(int amount){
        for(int i = 1; i <= amount; i++){
            Vector3 zPos = roadLength *(i * (roadAmount / amount));
            SpawnObstacleRow(zPos, false);
        }
    }
    private void SpawnObstacleRow(Vector3 zPos, bool spawnAnimation){
        Vector3[] xPositions = new Vector3[2];

        int rowPrefab = Random.Range(0, 5);
        while(lastObstacleRow == rowPrefab){
            rowPrefab = Random.Range(0, 5);
        }
        lastObstacleRow = rowPrefab;
        switch(rowPrefab){
            case 0:
                xPositions[0] = new Vector3(-7.5f, 0, 0);
                xPositions[1] = new Vector3(-2.5f, 0, 0);
            break;
            case 1:
                xPositions[0] = new Vector3(7.5f, 0, 0);
                xPositions[1] = new Vector3(2.5f, 0, 0);
            break;
            case 2:
                xPositions[0] = new Vector3(-2.5f, 0, 0);
                xPositions[1] = new Vector3(2.5f, 0, 0);
                break;
            default:
            case 3:
                xPositions[0] = new Vector3(7.5f, 0, 0);
                xPositions[1] = new Vector3(-2.5f, 0, 0);
                break;
            case 4:
                xPositions[0] = new Vector3(-7.5f, 0, 0);
                xPositions[1] = new Vector3(2.5f, 0, 0);
                break;
            // case 5:
            //     xPositions[0] = new Vector3(-7.5f, 0, 0);
            //     xPositions[1] = new Vector3(7.5f, 0, 0);
            //     break;
        }
        for(int x = 0; x < 2; x++){
            Vector3 location = xPositions[x] + zPos;
            GameObject obstacle = objectPooler.SpawnFromPool(CustomTags.Obstacle, location, new Quaternion(), true);
            if(spawnAnimation)
                obstacle.GetComponent<SpawnAnimation>().StartAnimation();            
        }
    }
    #endregion
    #region Road and Environment spawning
    private void SpawnEnvironmentObject(GameObject prefab){
        // GameObject prefab = environmentPrefabs[Random.Range(0, environmentPrefabs.Length)];
        bool leftOfRoad = Random.Range(0,2) == 1 ? true : false;
        float xPos = leftOfRoad ? Random.Range(-roadXMaxOffset,-roadXOffset) : Random.Range(roadXOffset, roadXMaxOffset);
        float zPos = Random.Range(-20, spawnZPos.z);
        GameObject result = Instantiate(prefab, new Vector3(xPos,prefab.transform.localPosition.y,zPos), prefab.transform.rotation);
        result.tag = CustomTags.EnvironmentObject;
    }

    private void SpawnFoliageObject(GameObject prefab){
        bool leftOfRoad = Random.Range(0,2) == 1 ? true : false;
        float xPos = leftOfRoad ? Random.Range(-roadXMaxOffset,-roadXOffset) : Random.Range(roadXOffset, roadXMaxOffset);
        float zPos = Random.Range(-20, spawnZPos.z/2);
        GameObject result = Instantiate(prefab, new Vector3(xPos,prefab.transform.localPosition.y,zPos), prefab.transform.rotation);
        result.tag = CustomTags.FoliageObject;
    }
    private GameObject SpawnRoad(Vector3 location, bool isStaticGround){
        GameObject newRoad = Instantiate(roadPrefab, location, roadPrefab.transform.rotation);
        if(!isStaticGround) newRoad.AddComponent(typeof(BackMover));
        newRoad.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        newRoad.tag = CustomTags.Road;
        return newRoad;
    }

    private void SpawnFloor(){
        GameObject floor = Instantiate(environmentFloorPrefab, new Vector3(250.0f, -0.4f, 0.0f) + spawnZPos, environmentFloorPrefab.transform.rotation);
        floor.transform.localScale = new Vector3(170.0f, 1.0f , 45.0f);
    }


    public void RespawnRoad(GameObject toRespawn){
        roadsToRespawn.Add(toRespawn);
    }

    private void TryRespawnRoad(){
        if(roadsToRespawn.Count > 0){
            GameObject toRespawn = roadsToRespawn[roadsToRespawn.Count - 1];
            roadsToRespawn.RemoveAt(roadsToRespawn.Count - 1);
            toRespawn.transform.position = lastSpawn.transform.localPosition + roadLength;
            lastSpawn = toRespawn;
        }
    }

    public void RespawnEnvironmentObject(GameObject toRespawn){
        bool leftOfRoad = Random.Range(0,2) == 1 ? true : false;
        float xPos = leftOfRoad ? Random.Range(-roadXMaxOffset,-roadXOffset) : Random.Range(roadXOffset, roadXMaxOffset);
        toRespawn.transform.position = new Vector3(xPos, toRespawn.transform.localPosition.y, 0) + spawnZPos;
        toRespawn.transform.Rotate(Vector3.up * Random.Range(90, 360));
        toRespawn.GetComponent<SpawnAnimation>().StartAnimation();
    }

    public void RespawnFoliageObject(GameObject toRespawn){
        bool leftOfRoad = Random.Range(0,2) == 1 ? true : false;
        float xPos = leftOfRoad ? Random.Range(-roadXMaxOffset,-roadXOffset) : Random.Range(roadXOffset, roadXMaxOffset);
        toRespawn.transform.position = new Vector3(xPos, toRespawn.transform.localPosition.y, 0) + (spawnZPos/2);
        toRespawn.transform.Rotate(Vector3.up * Random.Range(90, 360));
        toRespawn.GetComponent<SpawnAnimation>().StartAnimation();
    }
    #endregion
}
