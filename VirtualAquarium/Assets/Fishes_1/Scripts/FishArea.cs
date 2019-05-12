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

    Fish[] fishes;
    BoxCollider collider;
    Vector3 center;
    Vector3 max;
    Vector3 min;
    
    void Start()
    {
        AquariumProperties.aquariumTemperature = 25.0f;       
        fishes = transform.GetComponentsInChildren<Fish>();        
        InitializeAllFishes();
    }    

    private void updateAquariumHealth()
    {
        int totalLife = 0;
        int countFishes = 0;
        for (int i = 0; i < fishes.Length; i++)
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
        for (int i = 0; i < fishes.Length; i++)
            fishes[i].Initialize(this);
    }

    public void SpawnFishes()
    {
        fishes = transform.GetComponentsInChildren<Fish>();
        if (collider == null)
            collider = GetComponent<BoxCollider>();
        center = collider.center;
        max = center + collider.size / 2f;
        min = center - collider.size / 2f;
        if (fishes.Length < count)
            for (int i = fishes.Length; i < count; i++)
                SpawnFish();
        else
            if (fishes.Length > count)
            for (int i = 0; i < fishes.Length - count; i++)
#if UNITY_EDITOR
                DestroyImmediate(fishes[i].gameObject);
#else
                Destroy(fishes[i].gameObject);
#endif
        fishes = transform.GetComponentsInChildren<Fish>();
        MixPositions();
    }

    public void RemoveFishes()
    {
        fishes = transform.GetComponentsInChildren<Fish>();
        for (int i = 0; i < fishes.Length; i++)
#if UNITY_EDITOR
            DestroyImmediate(fishes[i].gameObject);
#else
                Destroy(fishes[i].gameObject);
#endif
    }

    public void Update()
    {
        updateAquariumHealth();
        if (Input.GetKeyDown(KeyCode.Space)) 
        {            
            particleFood.Play();            
        }
        if (feedPoint.totalFood() > 0)
        {            
            feedFishes();
        }
        UpdateFishes();
    }

    private void UpdateFishes()
    {
        for (int i = 0; i < fishes.Length; i++)
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
        for (int i = 0; i < fishes.Length; i++)
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
    }

    private bool hasFishesFeeding()
    {
        for (int i = 0; i < fishes.Length; i++)
        {
            if (fishes[i].state == Fish.FStates.Feed)
            {                                
                return true;
            }
        }        
        return false;
    }

    private void feedFishes()
    {
        int index = UnityEngine.Random.Range(0, fishes.Length - 1);        
        if (fishes[index].life < 80 && !hasFishesFeeding())
        {            
            fishes[index].goToFeed();
        }        
    }
}
