using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumProperties : ScriptableObject {

    public static float aquariumTemperature;
    public static float externalTemperature;
    public static float heaterTemperature;
    public static float aquariumHealth;
    public static float lightIntensity;
    public static float foodAvailable;
    public static Wheater currentWheater;
    public static DateTime aquariumHour;
    public static TimeSpeed currentTimeSpeed;
    public static float timeSpeedMultiplier;
    public static float lifeLostPerHour = 33.3f;
    public static float temperatureCoefficient;
    public static float lossLifeCoefficient;
    public enum TimeSpeed
    {
        Slow, Normal, Fast, RealTime        
    }
    public enum Wheater
    {
        Sun = 0,  SunAndCloud = 1, Moon = 2, Rain = 3, Snow = 4
    }
}
