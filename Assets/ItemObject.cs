﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour {
    public Text mytext = null;
    public int counter = 0;
    public void ChangeText()
    {
        counter++;
        if (counter % 2 == 1)
        {
            mytext.text = "Pause";
        }
        else
        {
            mytext.text = "Start";
        }
    }
}
