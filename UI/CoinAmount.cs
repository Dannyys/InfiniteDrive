using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinAmount : MonoBehaviour
{
    DataManager dataManager;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        dataManager = GameObject.FindWithTag(CustomTags.DataManager).GetComponent<DataManager>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = dataManager.data.coins.ToString();
    }
}
