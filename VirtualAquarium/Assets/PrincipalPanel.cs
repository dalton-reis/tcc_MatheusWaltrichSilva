using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrincipalPanel : MonoBehaviour {

    public Button jogarButton;
    public Button configurarButton;
    public GameObject configuracaoPanel;

    // Use this for initialization
    void Start () {
        jogarButton.onClick.AddListener(jogarButtonFunc);
        configurarButton.onClick.AddListener(configurarButtonFunc);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void jogarButtonFunc()
    {
        IUTConnect conn = new IUTConnect();
        conn.start("AQUARIUM_01");
    }

    void configurarButtonFunc()
    {
        configuracaoPanel.SetActive(true);
        GameObject.Find("Principal_Panel").SetActive(false);
    }
}
