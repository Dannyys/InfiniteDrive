using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBrowser : MonoBehaviour
{
    public List<GameObject> pages;
    private GameObject currentPage;
    private int currentPageIndex;
    // Start is called before the first frame update

    void OnEnable(){
        currentPageIndex = 0;
        currentPage = pages[currentPageIndex];
        int i = 0;
        foreach(GameObject page in pages){
            if(page.activeSelf == true){
                currentPage = page;
                currentPageIndex = i;
            }
            i++;
        }
    }
    public void BrowseLeft(){
        if(currentPageIndex == 0) currentPageIndex = pages.Count -1;
        else currentPageIndex--;
        currentPage.SetActive(false);
        currentPage = pages[currentPageIndex];
        currentPage.SetActive(true);
    }
    public void BrowseRight(){
        if(currentPageIndex == pages.Count -1) currentPageIndex = 0;
        else currentPageIndex++;
        currentPage.SetActive(false);
        currentPage = pages[currentPageIndex];
        currentPage.SetActive(true);
    }
}
