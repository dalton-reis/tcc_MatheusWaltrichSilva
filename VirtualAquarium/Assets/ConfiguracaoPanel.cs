using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class ConfiguracaoPanel : MonoBehaviour {

    public Button confirmarButton;
    public InputField SSIDInput;
    public InputField PasswordInput;
    public GameObject principalPanel;
    // Use this for initialization
    void Start () {
        confirmarButton.onClick.AddListener(confirmarButtonFunc);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void confirmarButtonFunc()
    {
        string SSID = SSIDInput.text;
        string password = PasswordInput.text;
        string moduleIP = "http://" + IUTModuleProperties.ModuleAddress != null && !"".Equals(IUTModuleProperties.ModuleAddress) ? IUTModuleProperties.ModuleAddress : IUTModuleProperties.DEFAULT_MODULE_ADDRESS;
        UnityWebRequest.Post(moduleIP + "/wifi_config", "ssid=" + SSID + "&password=" + password);
        principalPanel.SetActive(true);
        GameObject.Find("Configuracao_Panel").SetActive(false);        
    }
}
