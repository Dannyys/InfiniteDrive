using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;


public class GameUIManager : MonoBehaviour
{
    public GameObject sceneFader;
    public GameObject backgroundFader;
    public GameObject gameUI;
    public GameObject gameOverUI;
    public PlayerController playerController;
    public float currentTouchInput;
    private GameManager gameManager;
    private Image sceneFaderImg;
    private Image backgroundFaderImg;
    private SpeedControl speedController;
    private DataManager dataManager;

    private int tempNewExp;
    private int tempRequiredExp;
    private int tempLevel;
    private int tempCoins;
    private int lastRecord;

    private bool stopAnimations;


    private void Start() {
        currentTouchInput = 0;
        stopAnimations = false;
        sceneFaderImg = sceneFader.GetComponent<Image>();
        backgroundFaderImg = backgroundFader.GetComponent<Image>();
        gameManager = GameObject.FindWithTag(CustomTags.GameManager).GetComponent<GameManager>();
        speedController = GameObject.FindWithTag(CustomTags.SpeedControl).GetComponent<SpeedControl>();
        dataManager = GameObject.FindWithTag(CustomTags.DataManager).GetComponent<DataManager>();
    }

    private void Update() {
        playerController.horizontalTouchInput = currentTouchInput;
    }

    #region StartGame
    IEnumerator FadeIntoGame(){
        Color tmpColor = sceneFaderImg.color;
        WaitForSeconds delay = new WaitForSeconds(0.01f);
        while(sceneFaderImg.color.a > 0){
            tmpColor.a -= 0.075f;
            sceneFaderImg.color = tmpColor;
            yield return delay;
        }
        sceneFader.SetActive(false);
        gameManager.StartGame();
    }
    public void StartGame(){
        sceneFader.SetActive(true);
        StartCoroutine(FadeIntoGame());
    }
    #endregion
    #region EndGame
    IEnumerator FadeOutGameUI(){
        CanvasGroup gameUIGroup = gameUI.GetComponent<CanvasGroup>();
        WaitForSeconds delay = new WaitForSeconds(0.001f);
        while(gameUIGroup.alpha > 0){
            gameUIGroup.alpha -= 0.075f;
            yield return delay;
        }
    }
    IEnumerator FadeInGameOverBackground(){
        backgroundFader.SetActive(true);
        Color tmpColor = backgroundFaderImg.color;
        WaitForSeconds delay = new WaitForSeconds(0.001f);
        while(backgroundFaderImg.color.a < 0.5){
            tmpColor.a += 0.075f;
            backgroundFaderImg.color = tmpColor;
            yield return delay;
        }
        tmpColor.a = 0.5f;
        backgroundFaderImg.color = tmpColor;
    }
    IEnumerator FadeInGameOverUI(){
    
        CanvasGroup gameOverUIGroup = gameOverUI.GetComponent<CanvasGroup>();
        Debug.Log(gameOverUIGroup.alpha);
        WaitForSeconds delay = new WaitForSeconds(0.001f);
        while(gameOverUIGroup.alpha < 1){
            gameOverUIGroup.alpha += 0.075f;
            yield return delay;
        }
        StartCoroutine(MetersAnimation());
    }
    IEnumerator ExperienceAnimation(){
        Text levelText = gameOverUI.transform.Find("TopBarCanvas").gameObject.transform.Find("Level").GetComponent<Text>();
        Image expBar = gameOverUI.transform.Find("TopBarCanvas").gameObject.transform.Find("ExperienceBar").gameObject.transform.Find("Experience").GetComponent<Image>();
        int levelsGained = 0;
        int levelInText = tempLevel;
        WaitForSeconds delay =  new WaitForSeconds(0.001f);
        while(tempNewExp > tempRequiredExp){
            if(stopAnimations) break;
            levelsGained++;
            tempLevel++;
            tempNewExp = tempNewExp - tempRequiredExp;
            tempRequiredExp = ((tempLevel * 10000) + 10000);
        }
        while(levelsGained > 0){
            if(stopAnimations) break;
            while(expBar.fillAmount < 1){
                expBar.fillAmount += 0.01f;
                yield return delay;
            }
            expBar.fillAmount = 0;
            levelInText++;
            levelText.text = "lvl " + levelInText.ToString();
            levelsGained--;
        }
        float targetFill = ((100 / ((dataManager.data.level * 10000.0f) + 10000.0f)) * dataManager.data.exp) / 100.0f;
        while(expBar.fillAmount < targetFill){
            if(stopAnimations) break;
            expBar.fillAmount += 0.01f;
            yield return delay;
        }
        levelText.text = "lvl " + dataManager.data.level;
        expBar.fillAmount = targetFill;
        if(!stopAnimations)
            StartCoroutine(CoinsAnimation());
    }
    IEnumerator CoinsAnimation(){
        Text coinText = gameOverUI.transform.Find("TopBarCanvas").gameObject.transform.Find("CoinAmount").GetComponent<Text>();
        WaitForSeconds delay = new WaitForSeconds(0.01f);
        while(tempCoins < dataManager.data.coins){
            if(stopAnimations) break;
            coinText.text = tempCoins.ToString();
            tempCoins++;
            yield return delay;
        }
        coinText.text = dataManager.data.coins.ToString();
    }
    IEnumerator MetersAnimation(){
        int currentMeters = 0;
        bool doneNewRecordAnimation = false;
        Text metersText = gameOverUI.transform.Find("MetersPassed").GetComponent<Text>();
        metersText.text = "0 m";
        Text recordText = gameOverUI.transform.Find("Record").GetComponent<Text>();
        WaitForSeconds delay = new WaitForSeconds(0.001f);
        while(currentMeters < speedController.metersPassed){
            if(stopAnimations) break;
            currentMeters += 25;
            metersText.text = currentMeters.ToString() +" m";
            if(currentMeters > lastRecord 
            && currentMeters <= speedController.metersPassed 
            && !doneNewRecordAnimation){
                doneNewRecordAnimation = true;
                StartCoroutine(NewRecordAnimation());
            }
            yield return delay;
            if(currentMeters > lastRecord){
                recordText.text = "best: " + currentMeters.ToString() + " m";
            }
        }
        if(speedController.metersPassed > lastRecord){
                recordText.text = "best: " + Mathf.RoundToInt(speedController.metersPassed).ToString() + " m";
        }
        metersText.text = Mathf.RoundToInt(speedController.metersPassed).ToString() +" m";
        if(!stopAnimations)
            StartCoroutine(ExperienceAnimation());
    }
    IEnumerator NewRecordAnimation(){
        GameObject newRecordObj = gameOverUI.transform.Find("NewRecord").gameObject;
        Text newRecordText = newRecordObj.GetComponent<Text>();
        newRecordObj.SetActive(true);
        newRecordText.fontSize = 300;
        WaitForSeconds delay = new WaitForSeconds(0.001f);
        while(newRecordText.fontSize > 80){
            newRecordText.fontSize-=10;
            yield return delay;
        }
    }
    IEnumerator LateStopGame(){
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FadeOutGameUI());
        StartCoroutine(FadeInGameOverBackground());
        StartCoroutine(FadeInGameOverUI());
    }

    public void StopGame(){
        gameOverUI.SetActive(true);
        Text levelText = gameOverUI.transform.Find("TopBarCanvas").gameObject.transform.Find("Level").GetComponent<Text>();
        Image expBar = gameOverUI.transform.Find("TopBarCanvas").gameObject.transform.Find("ExperienceBar").gameObject.transform.Find("Experience").GetComponent<Image>();
        Text coinText = gameOverUI.transform.Find("TopBarCanvas").gameObject.transform.Find("CoinAmount").GetComponent<Text>();
        Text coinsGainedText = gameOverUI.transform.Find("CoinsGained").GetComponent<Text>();
        Text recordText = gameOverUI.transform.Find("Record").GetComponent<Text>();
        int coinsCollectedThisRun = Mathf.RoundToInt((speedController.metersPassed / 100)) + playerController.coinsCollected;
        tempCoins = dataManager.data.coins;
        
        coinText.text =  dataManager.data.coins.ToString();
        levelText.text = "lvl " + dataManager.data.level.ToString();
        expBar.fillAmount = ((100 / ((dataManager.data.level * 10000.0f) + 10000.0f)) * dataManager.data.exp) / 100.0f;
        CalculateNewRecord();
        recordText.text = "best: " + lastRecord.ToString() + " m";
        dataManager.data.coins += coinsCollectedThisRun;
        CalculateNewExpAndLevel();
        coinsGainedText.text = "+" + (dataManager.data.coins - tempCoins).ToString();
        dataManager.SaveData();
        
        StartCoroutine(LateStopGame());
    }

    private void CalculateNewExpAndLevel(){
        int newExp = Mathf.RoundToInt(speedController.metersPassed);
        int requiredExp = ((dataManager.data.level * 10000) + 10000) - dataManager.data.exp;
        tempNewExp = newExp;
        tempRequiredExp = requiredExp;
        tempLevel = dataManager.data.level;
        while(newExp > requiredExp){
            dataManager.data.exp = 0;
            dataManager.data.level++;
            dataManager.data.coins += 100;
            newExp = newExp - requiredExp;
            requiredExp = ((dataManager.data.level * 10000) + 10000);
        }
        dataManager.data.exp += newExp;
    }

    private void CalculateNewRecord(){
        
        switch(dataManager.pickedCar){
            case CarTypes.StationWagon:
            lastRecord = dataManager.data.stationWagonRecord;
            if(dataManager.data.stationWagonRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.stationWagonRecord = Mathf.RoundToInt(speedController.metersPassed);
            }
            break;
            case CarTypes.DeliveryBoy:
            lastRecord = dataManager.data.deliveryBoyRecord;
            if(dataManager.data.deliveryBoyRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.deliveryBoyRecord = Mathf.RoundToInt(speedController.metersPassed);
            }
            break;
            case CarTypes.Derby:
            lastRecord = dataManager.data.derbyRecord;
            if(dataManager.data.derbyRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.derbyRecord = Mathf.RoundToInt(speedController.metersPassed);
            }   
            break;
            case CarTypes.FormulaOne:
            lastRecord = dataManager.data.formulaOneRecord;
            if(dataManager.data.formulaOneRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.formulaOneRecord = Mathf.RoundToInt(speedController.metersPassed);
            }   
            break;
            case CarTypes.MonsterTruck:
            lastRecord = dataManager.data.monsterTruckRecord;
            if(dataManager.data.monsterTruckRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.monsterTruckRecord = Mathf.RoundToInt(speedController.metersPassed);
            }   
            break;
            case CarTypes.MuscleSport:
            lastRecord = dataManager.data.muscleSportRecord;
            if(dataManager.data.muscleSportRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.muscleSportRecord = Mathf.RoundToInt(speedController.metersPassed);
            }   
            break;
            case CarTypes.OffRoadTruck:
            lastRecord = dataManager.data.offRoadTruckRecord;
            if(dataManager.data.offRoadTruckRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.offRoadTruckRecord = Mathf.RoundToInt(speedController.metersPassed);
            }   
            break;
            case CarTypes.Rally:
            lastRecord = dataManager.data.rallyRecord;
            if(dataManager.data.rallyRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.rallyRecord = Mathf.RoundToInt(speedController.metersPassed);
            }   
            break;
            case CarTypes.Sprint:
            lastRecord = dataManager.data.sprintRecord;
            if(dataManager.data.sprintRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.sprintRecord = Mathf.RoundToInt(speedController.metersPassed);
            }   
            break;
            case CarTypes.Stock:
            lastRecord = dataManager.data.sprintRecord;
            if(dataManager.data.sprintRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.sprintRecord = Mathf.RoundToInt(speedController.metersPassed);
            }   
            break;
            case CarTypes.StreetRacer:
            lastRecord = dataManager.data.streetRacerRecord;
            if(dataManager.data.streetRacerRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.streetRacerRecord = Mathf.RoundToInt(speedController.metersPassed);
            }   
            break;
            case CarTypes.SuperSport:
            lastRecord = dataManager.data.superSportRecord;
            if(dataManager.data.superSportRecord < Mathf.RoundToInt(speedController.metersPassed)){
                dataManager.data.superSportRecord = Mathf.RoundToInt(speedController.metersPassed);
            }
            break;
        }
    }
    #endregion

    IEnumerator RetryFade(){
        sceneFader.SetActive(true);
        Color tmpColor = sceneFaderImg.color;
        WaitForSeconds delay = new WaitForSeconds(0.001f);
        while(sceneFaderImg.color.a < 1){
            tmpColor.a += 0.075f;
            sceneFaderImg.color = tmpColor;
            yield return delay;
        }
        tmpColor.a = 1;
        sceneFaderImg.color = tmpColor;
        
        //possible fix for lag?
        Resources.UnloadUnusedAssets();
        GC.Collect();
        SceneManager.LoadScene("Game");
    }

    IEnumerator ChangeCarFade(){
        sceneFader.SetActive(true);
        Color tmpColor = sceneFaderImg.color;
        WaitForSeconds delay = new WaitForSeconds(0.001f);
        while(sceneFaderImg.color.a < 1){
            tmpColor.a += 0.075f;
            sceneFaderImg.color = tmpColor;
            yield return delay;
        }
        tmpColor.a = 1;
        sceneFaderImg.color = tmpColor;
        SceneManager.LoadScene("MainMenu");
    }
    public void RetryGame(){
        stopAnimations = true;
        StartCoroutine(RetryFade());
    }
    public void ChangeCar(){
        stopAnimations = true;
        dataManager.lastSceneWasGame = true;
        StartCoroutine(ChangeCarFade());
    }
}
