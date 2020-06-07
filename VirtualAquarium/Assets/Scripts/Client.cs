using Mirror;
using UnityEngine;


public class Client : MonoBehaviour {

    public NetworkManager manager;
    GameController gameController;

    void Awake () {
        manager = GetComponent<NetworkManager> ();
        if (!gameController) {
            gameController = GameObject.FindObjectOfType<GameController> ();
        }
    }

    // Start is called before the first frame update
    void Start () {

        if (gameController.multi) {
            manager.networkAddress = gameController.server;
            manager.StartClient ();
        }
    }
    void Update () { }
}