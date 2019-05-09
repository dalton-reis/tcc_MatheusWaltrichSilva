using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
    Animator animator;
    float timer;
    FStates state;
    List<Transform> playersAround = new List<Transform>();
    private FishArea fishArea;
    private Vector3 target;    
    private bool swimmedAway = false;
    private bool wasStayed = false;
    public int life;
    public float lifeTime;
    public enum FStates
    {
        Patrol, Stay, SwimAway, Feeding
    }

    private void Start()
    {
        life = 100;
    }

    private void lifeTreatment()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > 5 && life > 0)
        {            
            life -= 2;
            if (swimmedAway)
            {
                --life;
                swimmedAway = false;
            } else if (wasStayed) {
                ++life;
                wasStayed = false;
            }
            lifeTime = 0;
            Debug.Log(this.gameObject.name + " perdeu vida. Está com " + life);
        }
        else if (life == 0)
        {
            Debug.Log(this.gameObject.name + " morreu");
            this.gameObject.SetActive(false);
        }
    }    

    internal void goToFeed()
    {
        state = FStates.Feeding;
        Debug.Log("Feeding");
    }

    void Feeding()
    {
        target = fishArea.feedPoint.transform.position;
        transform.position += transform.forward * Time.deltaTime * fishArea.speed;
        transform.forward = Vector3.MoveTowards(transform.forward, target - transform.position, Time.deltaTime * fishArea.rotationSpeed);
    }

    void Patrol()
    {
        animator.SetInteger("State", 1);
        Ray ray = new Ray(transform.position, transform.forward);
        var casts = Physics.RaycastAll(ray, fishArea.raycastDistance);
        foreach (var cast in casts)
        {
            if (cast.collider.transform != this.transform)
            {
                if (cast.collider.tag.Equals("Vase") || cast.collider.tag.Equals("Terrain"))
                {
                    do
                    {
                        target = fishArea.GetRandomPoint();
                    } while (target.y > 2);
                }

                timer = UnityEngine.Random.Range(0, 10f);
                //Debug.Log(this.gameObject.name + " collided with: " + cast.collider.gameObject.name);
                //Debug.Log(this.transform.position.y);
                break;
            }
        }       
        transform.position += transform.forward * Time.deltaTime * fishArea.speed;
        transform.forward = Vector3.MoveTowards(transform.forward, target - transform.position, Time.deltaTime * fishArea.rotationSpeed);
        if ((transform.position - target).magnitude < fishArea.speed * Time.deltaTime * 3 || timer < 0f)
        {
            target = fishArea.GetRandomPoint();
            timer = UnityEngine.Random.Range(0, 10f);
            if (UnityEngine.Random.Range(0f, 1f) > 0.9f)
                state = FStates.Stay;
        }        
    }

    void SwimAway()
    {
        this.swimmedAway = true;
        Vector3 runVector = Vector3.zero;
        foreach (var t in playersAround)
            runVector += (t.transform.position - transform.position).normalized;
        runVector.Normalize();
        transform.forward = Vector3.MoveTowards(transform.forward, -runVector, Time.deltaTime * fishArea.rotationSpeed * 10);
        transform.position += transform.forward * Time.deltaTime * fishArea.speed;
    }

    void Stay()
    {
        wasStayed = true;
        transform.position += transform.forward * Time.deltaTime * fishArea.speed / 20f;
        animator.SetInteger("State", 0);
        if (timer < 0f)
        {
            if (UnityEngine.Random.Range(0f, 1f) < 0.9f)
                state = FStates.Patrol;
        }
    }

    internal void Move()
    {
        timer -= Time.deltaTime;
        lifeTreatment();
        switch (state)
        {
            case FStates.Patrol:
                Patrol();
                break;
            case FStates.Stay:
                Stay();
                break;
            case FStates.SwimAway:
                SwimAway();
                break;
            case FStates.Feeding:
                Feeding();
                break;
        }
    }

    internal void Initialize(FishArea fishArea)
    {
        state = FStates.Patrol;
        this.fishArea = fishArea;
        animator = GetComponent<Animator>();
        target = fishArea.GetRandomPoint();
    }

    public void AddPlayer(Transform t)
    {
        playersAround.Add(t);
    }

    public void RemovePlayer(Transform t)
    {
        playersAround.Remove(t);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(this.gameObject.name + " collided with (SwimAway): " + other.gameObject.name);
            AddPlayer(other.transform);
            state = FStates.SwimAway;
            if (animator!=null)
            animator.SetInteger("State", 1);
        } else if (other.gameObject.name == "FeedPoint" && state == FStates.Feeding)
        {
            Debug.Log(this.gameObject.name + " se alimentou");
            fishArea.removeFood();
            life += 10;
            state = FStates.Patrol;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            RemovePlayer(other.transform);
            if (playersAround.Count == 0)
            {
                state = FStates.Patrol;
                target = fishArea.GetRandomPoint();
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Ray ray = new Ray(transform.position, transform.forward);
        if (transform.parent == null)
            return;
        fishArea = transform.parent.GetComponent<FishArea>();
        if (fishArea == null)
            return;
        float raycastDistance = fishArea.raycastDistance;
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
    }
}
