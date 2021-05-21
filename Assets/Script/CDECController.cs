using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CDECController : MonoBehaviour
{
    public GameObject player;
    public GameObject cylinder;
    public Vector2 rightStk;

    public float distant;

    public TextMeshPro counter;
    public TextMeshPro prompt;

    public bool startTimer;
    public float countFrom = 3.0f; 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("OVRCameraRig");
        cylinder = player.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        rightStk = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        //move the current wall
        if (rightStk.y > 0)
        {
            cylinder.transform.position = cylinder.transform.position + player.transform.GetChild(0).GetChild(1).forward * 0.01f;
        }
        else if (rightStk.y < 0)
        {
            cylinder.transform.position = cylinder.transform.position - player.transform.GetChild(0).GetChild(1).forward *0.01f;
        }
        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.9) {
            distant = cylinder.transform.position.z;
            
            prompt.gameObject.SetActive(true);
            GameManagerController.GMC.HideCylinder();
            prompt.text = "Closest converge is " + distant.ToString("0.00");
            GameManagerController.GMC.ChangeText("CDEC", distant);
            counter.gameObject.SetActive(true);
            startTimer = true;
        }
        if (startTimer == true) {
            countFrom = countFrom - Time.deltaTime;
            counter.text = "Heading back in " + countFrom.ToString("0");
            if (countFrom <= 0.0f) {
                
                loadMenu();
            }
        }
    }
    private void loadMenu()
    {
        SceneManager.UnloadSceneAsync("CDEC");
        GameManagerController.GMC.ShowMenu();
    }
}
