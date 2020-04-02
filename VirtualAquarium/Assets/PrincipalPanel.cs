using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.VR;

public class PrincipalPanel : MonoBehaviour {

    public Button jogarButton;
    public Button jogarRVButton;
    public Button configurarButton;
    public Button sairButton;
    public GameObject configuracaoPanel;
    public GameObject connectingText;
    private bool startSimulator;
    private bool RV;

    // Use this for initialization
    void Start () {        
        AquariumProperties.configs = ConfigProperties.loadConfig();        
        jogarButton.onClick.AddListener(jogarButtonFunc);
        jogarRVButton.onClick.AddListener(jogarRVButtonFunc);
        configurarButton.onClick.AddListener(configurarButtonFunc);
        sairButton.onClick.AddListener(sairButtonFunc);
        //StartCoroutine(CloseVR());        
    }

    // Update is called once per frame    
    private void Update()
    {
        if (startSimulator && !RV)
        {
            SceneManager.LoadScene("AquariumScene", LoadSceneMode.Single);
        } else if (startSimulator && RV){
            StartCoroutine(LoadVR("MockHMD"));
            SceneManager.LoadScene("AquariumSceneVR", LoadSceneMode.Single);
        }        
    }

    private void OnApplicationQuit()
    {
        //AquariumProperties.conn.stop();
    }    

    void jogarButtonFunc()
    {
        //AquariumProperties.conn = new IUTConnect();
        //AquariumProperties.conn.OnConnect += connectCallback;
        //AquariumProperties.conn.start(AquariumProperties.configs.token);
        startSimulator = true;
        //connectingText.SetActive(true);
    }

    void jogarRVButtonFunc()
    {        
        startSimulator = true;        
        RV = true;        
    }

    IEnumerator LoadVR(string newDevice)
    {        
        //XRSettings.LoadDeviceByName("MockHMD");        
         XRSettings.LoadDeviceByName(newDevice);
            yield return null;
            XRSettings.enabled = true;
    }

    IEnumerator CloseVR()
    {
        UnityEngine.XR.XRSettings.LoadDeviceByName("None");
        yield return null;
        UnityEngine.XR.XRSettings.enabled = false;
    }

    void configurarButtonFunc()
    {
        configuracaoPanel.SetActive(true);
        GameObject.Find("Principal_Panel").SetActive(false);
    }    

    void connectCallback(bool connected)
    {
        startSimulator = true;
    }

    private void sairButtonFunc()
    {
        Application.Quit();
    }
}
