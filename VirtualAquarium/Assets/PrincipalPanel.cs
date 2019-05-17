using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrincipalPanel : MonoBehaviour {

    public Button jogarButton;
    public Button configurarButton;
    public GameObject configuracaoPanel;

    // Use this for initialization
    void Start () {
        AquariumProperties.configs = ConfigProperties.loadConfig();
        jogarButton.onClick.AddListener(jogarButtonFunc);
        configurarButton.onClick.AddListener(configurarButtonFunc);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void jogarButtonFunc()
    {
        AquariumProperties.conn = new IUTConnect();
        AquariumProperties.conn.callbackSocket.AddListener(AquariumUpdate.socketCallback);
        AquariumProperties.conn.start(AquariumProperties.configs.token);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    void configurarButtonFunc()
    {
        configuracaoPanel.SetActive(true);
        GameObject.Find("Principal_Panel").SetActive(false);
    }    
}
