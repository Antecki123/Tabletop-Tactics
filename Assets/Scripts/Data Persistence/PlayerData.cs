using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currency;
    public int metalScrap;

    public PlayerData()
    {
        this.currency = 0;
        this.metalScrap = 0;
    }
}