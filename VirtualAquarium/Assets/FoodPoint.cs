using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPoint : MonoBehaviour {

    private int foodCount = 0;    

    // Use this for initialization
    void Start () {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        if (other.name == "ParticleSystem")
        {
            addFood();
            Debug.Log("Caiu comida");
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void addFood()
    {
        this.foodCount++;
    }

    public void removeFood()
    {
        this.foodCount--;
    }

    public int totalFood()
    {
        return this.foodCount;
    }
}
