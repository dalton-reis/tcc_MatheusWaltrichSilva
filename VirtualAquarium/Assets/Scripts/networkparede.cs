using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

public class networkparede : NetworkManager {

    public GameObject parede;
    public GameObject canvas;
    public GameObject player;

    bool iniciadoParede = false;
    
    public override void OnServerAddPlayer (NetworkConnection conn) {
        Transform start = base.GetStartPosition ();
        player = Instantiate (playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection (conn, player);

        // spawn parede e Canvas
        if (!iniciadoParede) {
            parede = Instantiate (spawnPrefabs.Find (prefab => prefab.name == "cameraParede"));
            NetworkServer.Spawn (parede);
            canvas = Instantiate (spawnPrefabs.Find (prefab => prefab.name == "Canvas"));
            NetworkServer.Spawn (canvas);
            iniciadoParede = true;
        }
    }

    // Update is called once per frame
    void Update () {

    }
}