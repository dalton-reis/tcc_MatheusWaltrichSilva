using Mirror;
using UnityEngine;
using UnityEngine.XR;

public class Client : MonoBehaviour {

    public NetworkManager manager;
    GameController gameController;
    public GameObject ParedeJogo, ParedeDesenvolvimento;
    [SerializeField] private bool vrModeEnabled;

    void Awake () {
        manager = GetComponent<NetworkManager> ();
        if (!gameController) {
            gameController = GameObject.FindObjectOfType<GameController> ();
        }
    }

    // Start is called before the first frame update
    void Start () {

        XRSettings.enabled = vrModeEnabled;

        if (gameController.multi) {
            manager.networkAddress = gameController.server;
            manager.StartClient ();
            if (gameController.CameraDesenvolvimento) {
                ParedeJogo.SetActive (false);
                ParedeDesenvolvimento.SetActive (true);
            } else {
                ParedeJogo.SetActive (true);
                ParedeDesenvolvimento.SetActive (false);
            }
        }
    }
    void Update () { }
}