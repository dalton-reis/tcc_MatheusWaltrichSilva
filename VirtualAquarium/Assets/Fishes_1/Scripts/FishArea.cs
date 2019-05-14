using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishArea : MonoBehaviour {
    public GameObject[] prefabs;
    public int count;
    public float speed = 10;
    public float rotationSpeed = 5f;
    public float raycastDistance = 10f;    
    public FoodPoint feedPoint;
    public ParticleSystem particleFood;
    public Slider healthSlider;
    public Light directionalLight;
    public Text foodCountText;
    private float foodTime;
    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            if (count != value)
            {
                count = value;
                SpawnFishes();
                transform.hasChanged = false;
            }
        }
    }

    List<Fish> fishes;
    List<Fish> fishesOrderByFood;
    Fish.FishComparer fishComparer;
    BoxCollider collider;
    Vector3 center;
    Vector3 max;
    Vector3 min;
    
    void Start()
    {
        fishComparer = new Fish.FishComparer();
        AquariumProperties.aquariumTemperature = 25.0f;
        AquariumProperties.lightIntensity = 2;
        AquariumProperties.foodAvailable = 10;
        fishes = new List<Fish>(transform.GetComponentsInChildren<Fish>());
        fishesOrderByFood = new List<Fish>(fishes);
        InitializeAllFishes();
    }    

    private void updateAquariumHealth()
    {
        foodTime += Time.deltaTime;
        if (foodTime >= 180 && AquariumProperties.foodAvailable < 10)
        {
            AquariumProperties.foodAvailable++;
            foodTime = 0;
        }
        foodCountText.text = AquariumProperties.foodAvailable.ToString();
        int totalLife = 0;
        int countFishes = 0;
        for (int i = 0; i < fishes.Count; i++)
        {
            if (fishes[i].gameObject.activeSelf)
            {
                totalLife += fishes[i].life;
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

    public void InitializeAllFishes()
    {
        if (collider == null)
            collider = GetComponent<BoxCollider>();
        max = center + collider.size / 2f;
        min = center - collider.size / 2f;
        for (int i = 0; i < fishes.Count; i++)
            fishes[i].Initialize(this);
    }

    public void SpawnFishes()
    {
        fishes = new List<Fish>(transform.GetComponentsInChildren<Fish>());
        if (collider == null)
            collider = GetComponent<BoxCollider>();
        center = collider.center;
        max = center + collider.size / 2f;
        min = center - collider.size / 2f;
        if (fishes.Count < count)
            for (int i = fishes.Count; i < count; i++)
                SpawnFish();
        else
            if (fishes.Count > count)
            for (int i = 0; i < fishes.Count - count; i++)
#if UNITY_EDITOR
                DestroyImmediate(fishes[i].gameObject);
#else
                Destroy(fishes[i].gameObject);
#endif
        fishes = new List<Fish>(transform.GetComponentsInChildren<Fish>());
        MixPositions();
    }

    public void removeFish(Fish fish)
    {
        this.fishes.Remove(fish);
        this.fishesOrderByFood.Remove(fish);
#if UNITY_EDITOR
        DestroyImmediate(fish.gameObject);
#else
                Destroy(fish.gameObject);
#endif
    }

    public void RemoveFishes()
    {
        fishes = new List<Fish>(transform.GetComponentsInChildren<Fish>());
        for (int i = 0; i < fishes.Count; i++)
#if UNITY_EDITOR
            DestroyImmediate(fishes[i].gameObject);
#else
                Destroy(fishes[i].gameObject);
#endif
    }

    public void updateAquariumLight(float lightIntensity)
    {
        AquariumProperties.lightIntensity += lightIntensity;
        directionalLight.intensity = AquariumProperties.lightIntensity;
    }

    public void Update()
    {        
        updateAquariumHealth();
        if (Input.GetKeyDown(KeyCode.Space) && AquariumProperties.foodAvailable > 0) 
        {
            AquariumProperties.foodAvailable--;
            particleFood.Play();            
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            updateAquariumLight(0.03f);
        } 
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            updateAquariumLight(-0.03f);
        }
        fishesOrderByFood.Sort(fishComparer);
        if (feedPoint.totalFood() > 0)
        {            
            feedFishes();
        }
        UpdateFishes();
    }

    private void UpdateFishes()
    {
        for (int i = 0; i < fishes.Count; i++)
        {
            if (fishes[i].gameObject.activeSelf)
            {
                fishes[i].Move();
            }            
        }
    }

    public void SpawnFish()
    {
        Instantiate(GetRandomPrefab(), transform);

    }

    public void MixPositions()
    {
        for (int i = 0; i < fishes.Count; i++)
        {
            Transform temp = fishes[i].transform;
            temp.position = GetRandomPoint();
            temp.Rotate(0f, UnityEngine.Random.Range(0f, 360f), 0f, Space.Self);
        }

    }

    protected GameObject GetRandomPrefab()
    {
        int id = UnityEngine.Random.Range(0, prefabs.Length);
        return prefabs[id];
    }

    public Vector3 GetRandomPoint()
    {
        
        float x = UnityEngine.Random.Range(min.x, max.x);
        float y = UnityEngine.Random.Range(min.y, max.y);
        float z = UnityEngine.Random.Range(min.z, max.z);
        Vector3 p = new Vector3(x, y, z);
        return transform.TransformPoint(p);
    }
    
    public void removeFood()
    {
        feedPoint.removeFood();
        feedPoint.foods[feedPoint.foods.Count - 1].SetActive(false);
        feedPoint.foods.RemoveAt(feedPoint.foods.Count-1);
    }

    private bool hasFishesFeeding()
    {
        for (int i = 0; i < fishesOrderByFood.Count; i++) 
        {
            if (fishesOrderByFood[i].state == Fish.FStates.Feed)
            {

                return true;
            }
        }        
        return false;
    }

    private void feedFishes()
    {
        if (fishesOrderByFood.Count > 0 && !hasFishesFeeding())
        {
            fishesOrderByFood[0].goToFeed();
        }        
    }
}
