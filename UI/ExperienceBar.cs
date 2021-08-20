using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    DataManager dataManager;
    Image image;
    
    // Start is called before the first frame update
    void Start()
    {
        dataManager = GameObject.FindWithTag(CustomTags.DataManager).GetComponent<DataManager>();
        image = GetComponent<Image>();
        image.fillAmount = ((100 / ((dataManager.data.level * 10000.0f) + 10000.0f)) * dataManager.data.exp) / 100.0f;
        // image.fillAmount = ((((dataManager.data.level * 10000)+10000) / 100)*dataManager.data.exp) / 100;
    }
}
