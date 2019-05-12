using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFood : MonoBehaviour {

    public ParticleSystem part;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    public FoodPoint foodPoint;

    // Use this for initialization
    void Start () {
        part = GetComponent<ParticleSystem>();        
    }

    void OnParticleTrigger()
    {
        int numEnter = part.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);        
        for (int i = 0; i < numEnter; i++)
        {
            part.Stop();
            ParticleSystem.Particle p = enter[i];
            foodPoint.addFood();            
            enter[i] = p;            
        }
        part.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }
}
