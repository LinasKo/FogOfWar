﻿using UnityEngine;
using System.Collections;

public class Resources : MonoBehaviour
{
    public int maxWood;
    public int maxMeet;

    private int wood;
    private int meet;

    // Use this for initialization
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Resources(int startingWood, int startingMeet)
    {
        wood = startingWood;
        meet = startingMeet;
    }
}
