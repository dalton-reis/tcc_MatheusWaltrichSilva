using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrincipalPanel : MonoBehaviour {

    public Button jogarButton;

	// Use this for initialization
	void Start () {
        jogarButton.onClick.AddListener(jogarButtonFunc);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void jogarButtonFunc()
    {
        IUTConnect conn = new IUTConnect();
        conn.start("AQUARIUM_01");
    }
}
