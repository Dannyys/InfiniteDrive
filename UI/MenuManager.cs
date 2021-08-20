using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject optionsCanvas;
    public GameObject shopCanvas;
    public GameObject creditsCanvas;
    public GameObject pickCarCanvas;
    public GameObject buyCarCanvas;
    public GameObject cannotBuyCanvas;
    public GameObject menuFader;
    public GameObject sceneFader;
    private GameObject currentCanvas;
    // private GameObject backCanvas;
    private Canvasses currentCanvasEnum;
    private List<Canvasses> backQue;
    private Image menuFaderImg;
    private Image sceneFaderImg;
    private DataManager dataManager;
    // Start is called before the first frame update
    void Start()
    {
        dataManager = GameObject.FindWithTag(CustomTags.DataManager).GetComponent<DataManager>();
        backQue = new List<Canvasses>();
        currentCanvas = mainMenuCanvas;
        currentCanvasEnum = Canvasses.MainMenuCanvas;
        menuFaderImg =  menuFader.GetComponent<Image>(); 
        sceneFaderImg = sceneFader.GetComponent<Image>();
        if(dataManager.lastSceneWasGame){
            sceneFader.SetActive(true);
            Color tmpColor = sceneFaderImg.color;
            tmpColor.a = 1;
            sceneFaderImg.color = tmpColor;
            SetCurrentCanvas(Canvasses.PickCarCanvas, true, false);
            StartCoroutine(FadeInFromGame());
        }
    }

    IEnumerator FadeInFromGame(){
        Color tmpColor = sceneFaderImg.color;
        WaitForSeconds delay = new WaitForSeconds(0.001f);
        while(sceneFaderImg.color.a > 0){
            tmpColor.a -= 0.075f;
            sceneFaderImg.color = tmpColor;
            yield return delay;
        }
        sceneFader.SetActive(false);
    }
    public void ShowBuyCarCanvas(CarTypes carType, int carValue){
        buyCarCanvas.SetActive(true);
        Text dialogText = buyCarCanvas.transform.Find("Image").gameObject.transform.Find("BuyThisCar").gameObject.GetComponent<Text>();
        dialogText.text = "Buy this car for "+carValue.ToString()+"?";
        ConfirmBuyCar confirmBuyCar = buyCarCanvas.transform.Find("Image").gameObject.transform.Find("ConfirmButton").gameObject.GetComponent<ConfirmBuyCar>();
        confirmBuyCar.carValue = carValue;
        confirmBuyCar.carType = carType;
    }

    public void ShowCannotBuyCarCanvas(){
        cannotBuyCanvas.SetActive(true);
    }

    public void SwitchToGameScene(){
        StartCoroutine(SwitchSceneFade());
    }
    public void SetCurrentCanvas(Canvasses canvas, bool setBackCanvas, bool useTransition){
        if(setBackCanvas) backQue.Add(currentCanvasEnum);
        if(useTransition) {
            StartCoroutine(SwitchCanvasFade(canvas));
            return;
        }
        SwitchCanvas(canvas);
    }

    public void GoBackCanvas( bool useTransition){
        Canvasses canvas = backQue[backQue.Count - 1];
        backQue.RemoveAt(backQue.Count - 1);
        if(useTransition) {
            StartCoroutine(SwitchCanvasFade(canvas));
            return;
        }
        SwitchCanvas(canvas);
        
    }
    private IEnumerator SwitchSceneFade(){
        sceneFader.SetActive(true);
        Color tmpColor = sceneFaderImg.color;
        WaitForSeconds delay = new WaitForSeconds(0.001f);
        while(sceneFaderImg.color.a < 1){
            tmpColor.a += 0.075f;
            sceneFaderImg.color = tmpColor;
            yield return delay;
        }
        // Debug.Log("switch");
        SceneManager.LoadScene("Game");
    }
    private IEnumerator SwitchCanvasFade(Canvasses canvas){
        menuFader.SetActive(true);
        Color tmpColor = menuFaderImg.color;
        WaitForSeconds delay = new WaitForSeconds(0.001f);
        while(menuFaderImg.color.a < 1){
            tmpColor.a += 0.075f;
            menuFaderImg.color = tmpColor;
            yield return delay;
        }
        SwitchCanvas(canvas);
        while(menuFaderImg.color.a > 0){
            tmpColor.a -= 0.075f;
            menuFaderImg.color = tmpColor;
            yield return delay;
        }
        menuFader.SetActive(false);
    }

    private void SwitchCanvas(Canvasses canvas){
        currentCanvas.SetActive(false);
        currentCanvasEnum = canvas;
        if(canvas == Canvasses.OptionsCanvas){
            optionsCanvas.SetActive(true);
            currentCanvas = optionsCanvas;
        }else if(canvas == Canvasses.CreditsCanvas){
            creditsCanvas.SetActive(true);
            currentCanvas = creditsCanvas;
        }else if(canvas == Canvasses.ShopCanvas){
            shopCanvas.SetActive(true);
            currentCanvas = shopCanvas;
        }else if(canvas == Canvasses.MainMenuCanvas){
            mainMenuCanvas.SetActive(true);
            currentCanvas = mainMenuCanvas;
        }else if(canvas == Canvasses.PickCarCanvas){
            pickCarCanvas.SetActive(true);
            currentCanvas = pickCarCanvas;
        }
    }
}
