using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class AquariumUpdate : MonoBehaviour {

    public FishArea fishArea;
    public Slider healthSlider;
    public Slider aquariumTemperatureSlider;
    public Text foodCountText;
    public Text hourText;
    public RawImage wheaterImage;    
    private float accumulatedTime;
    private DateTime lastFoodHour;
    private float timeChangeWheater;

	// Use this for initialization
	void Start () {
        AquariumProperties.aquariumTemperature = 25.0f;
        AquariumProperties.lightIntensity = 2;
        AquariumProperties.foodAvailable = 10;
        AquariumProperties.currentWheater = AquariumProperties.Wheater.Sun;
        AquariumProperties.aquariumHour = DateTime.ParseExact("08:00", "HH:mm", CultureInfo.InvariantCulture);
        lastFoodHour = DateTime.ParseExact("08:00", "HH:mm", CultureInfo.InvariantCulture);
        switch (AquariumProperties.currentTimeSpeed)
        {
            case AquariumProperties.TimeSpeed.Fast:
                AquariumProperties.timeSpeedMultiplier = 90;
                break;
            case AquariumProperties.TimeSpeed.Normal:
                AquariumProperties.timeSpeedMultiplier = 180;
                break;
            case AquariumProperties.TimeSpeed.Slow:
                AquariumProperties.timeSpeedMultiplier = 600;
                break;
            case AquariumProperties.TimeSpeed.RealTime:
                AquariumProperties.timeSpeedMultiplier = 3600;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        updateTime();
        updateHealth();
        updateFood();
        updateWheater();
	}

    void updateTime()
    {
        accumulatedTime += Time.deltaTime;
        if (AquariumProperties.currentTimeSpeed != AquariumProperties.TimeSpeed.RealTime)
        {
            if (accumulatedTime >= AquariumProperties.timeSpeedMultiplier)
            {
                AquariumProperties.aquariumHour.AddHours(1);
                accumulatedTime = 0;
            }
        } else
        {
            AquariumProperties.aquariumHour.AddSeconds(Time.deltaTime);
        }        
        hourText.text = AquariumProperties.aquariumHour.ToString("HH:ss");
    }

    void updateHealth()
    {        
        float totalLife = 0;
        int countFishes = 0;
        for (int i = 0; i < fishArea.fishes.Count; i++)
        {
            if (fishArea.fishes[i].gameObject.activeSelf)
            {
                totalLife += fishArea.fishes[i].life;
                ++countFishes;
            }

        }
        if (countFishes > 0)
        {
            AquariumProperties.aquariumHealth = totalLife / countFishes;
            healthSlider.value = AquariumProperties.aquariumHealth * 0.01f;
        }
        else
        {
            AquariumProperties.aquariumHealth = 0;
            healthSlider.value = 0;
        }
    }

    void updateFood()
    {      
        if ((AquariumProperties.aquariumHour.Subtract(lastFoodHour).Hours >= 2) && AquariumProperties.foodAvailable < 10)
        {
            AquariumProperties.foodAvailable++;
            lastFoodHour = AquariumProperties.aquariumHour;
        }
        foodCountText.text = AquariumProperties.foodAvailable.ToString();        
    }

    void updateWheater()
    {
        timeChangeWheater += Time.deltaTime;
        if (timeChangeWheater >= AquariumProperties.timeSpeedMultiplier * 4)
        {
            bool acceptable = false;
            do
            {
                int wheater = UnityEngine.Random.Range(0, 4);
                if (AquariumProperties.aquariumHour >= DateTime.ParseExact("18:30", "HH:mm", CultureInfo.InvariantCulture)
                    && AquariumProperties.aquariumHour <= DateTime.ParseExact("04:59", "HH:mm", CultureInfo.InvariantCulture)
                    && (wheater == 0 || wheater == 1)) 
                {
                    acceptable = false;
                } else if (AquariumProperties.aquariumHour >= DateTime.ParseExact("05:00", "HH:mm", CultureInfo.InvariantCulture)
                    && AquariumProperties.aquariumHour <= DateTime.ParseExact("18:29", "HH:mm", CultureInfo.InvariantCulture)
                    && wheater == 4)
                {
                    acceptable = false;
                } else
                {
                    acceptable = true;
                }
            } while (!acceptable);            
            AquariumProperties.currentWheater = (AquariumProperties.Wheater) UnityEngine.Random.Range(0, 4);
            timeChangeWheater = 0;
        }
        switch (AquariumProperties.currentWheater)
        {
            case AquariumProperties.Wheater.Sun:
                wheaterImage.texture = Resources.Load<Texture>("sun.png");
                AquariumProperties.externalTemperature = UnityEngine.Random.Range(32, 43);
                break;
            case AquariumProperties.Wheater.SunAndCloud:
                wheaterImage.texture = Resources.Load<Texture>("sun-and-cloud.png");
                AquariumProperties.externalTemperature = UnityEngine.Random.Range(26, 33);
                break;
            case AquariumProperties.Wheater.Snow:
                wheaterImage.texture = Resources.Load<Texture>("snow.png");
                AquariumProperties.externalTemperature = UnityEngine.Random.Range(-2, 5);
                break;
            case AquariumProperties.Wheater.Rain:
                wheaterImage.texture = Resources.Load<Texture>("rain.png");
                AquariumProperties.externalTemperature = UnityEngine.Random.Range(10, 24);
                break;
            case AquariumProperties.Wheater.Moon:
                wheaterImage.texture = Resources.Load<Texture>("moon.png");
                AquariumProperties.externalTemperature = UnityEngine.Random.Range(8, 21);
                break;
        }
    }
}
