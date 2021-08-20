﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Level : MonoBehaviour
{
    DataManager dataManager;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        dataManager = GameObject.FindWithTag(CustomTags.DataManager).GetComponent<DataManager>();
        text = GetComponent<Text>();
        text.text = "lvl " + dataManager.data.level.ToString();
    }

}
