using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

public class Server : MonoBehaviour {
    NetworkManager manager;
    public List<Camera> cameras;
    internal GameController gameController;

    void Awake () {
        manager = GetComponent<NetworkManager> ();
    }

    // Start is called before the first frame update
    void Start () {
        if (!gameController) {
            gameController = GameObject.FindObjectOfType<GameController> ();
        }
        if (gameController.multi) {
            manager.StartHost ();
        }
    }

    // Update is called once per frame
    void Update () {
        cameras = new List<Camera> (GameObject.FindObjectsOfType<Camera> ());
        for (int i = 0; i < cameras.Count; i++) {
            if (cameras[i].name != "Main Camera") {
                cameras[i].enabled = false;
            }
        }
    }
}