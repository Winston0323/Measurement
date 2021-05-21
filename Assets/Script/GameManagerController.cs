using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerController : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManagerController GMC;
    bool gameStart = false;
    public float horiFOV;
    public float vertFOV;
    public TextMeshPro horiValue;
    public TextMeshPro vertValue;

    //resolution value
    public float horiRes;
    public float vertRes;
    public TextMeshPro horiRValue;
    public TextMeshPro vertRValue;

    //CTP
    public Vector3 posStd;
    public Vector3 angStd;
    public TextMeshPro posValue;
    public TextMeshPro angValue;

    //PPC
    public float PPC;
    public TextMeshPro PPCValue;

    //closest conversion
    public float CDEC;
    public TextMeshPro CDECValue;


    public GameObject menu;
    public GameObject player;
    public GameObject cylinder;

    public bool hit;
    private void Awake()
    { 
        if (!gameStart) {
            GMC = this;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowMenu() {
        menu.SetActive(true);
    }
    public void HideMenu()
    {
        menu.SetActive(false);
    }
    public void ChangeText(string name, float value) {
        switch (name)
        {
            case "horiFOV":
                Debug.Log("Changing horiFOV");
                horiFOV = value;
                horiValue.text = horiFOV.ToString("000");
                break;
            case "vertFOV":
                Debug.Log("Changing vertFOV");
                vertFOV = value;
                vertValue.text = vertFOV.ToString("000");
                break;
            case "horiRes":
                Debug.Log("Changing horiRes");
                horiRes = value;
                horiRValue.text = horiRes.ToString("00.00");
                break;
            case "vertRes":
                Debug.Log("Changing vertRes");
                vertRes = value;
                vertRValue.text = vertRes.ToString("00.00");
                break;
            case "PPC":
                Debug.Log("Changing PPC");
                PPC = value;
                PPCValue.text = (PPC-1).ToString("00") + " <  "+"  < " + PPC.ToString("00");
                break;
            case "CDEC":
                Debug.Log("Changing CDEC");
                CDEC = value;
                CDECValue.text = value.ToString("0.00");
                break;
            default:
                break;

        }    
    }
    public void ChangeText(string name, Vector3 value)
    {
        switch (name)
        {
            case "posStd":
                Debug.Log("Changing posStd");
                posStd = value;
                posValue.text = posStd.ToString();
                break;
            case "angStd":
                Debug.Log("Changing angStd");
                angStd = value;
                angValue.text = angStd.ToString();
                break;
            default:
                break;
        }
    }
    public void AddChild(GameObject obj) {
        obj.transform.SetParent(player.transform);
    }
    public void ShowCylinder() {
        cylinder = player.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        cylinder.gameObject.SetActive(true);
    }
    public void HideCylinder()
    {
        cylinder = player.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        cylinder.gameObject.SetActive(false);
    }

    public Vector3 FowardDirection() {
        return player.transform.GetChild(0).GetChild(1).GetChild(0).forward;
    }

    public void HidePlayer() {
        player.SetActive(false);
    }
    public void SetHit(bool val)
    {
        this.hit = val;
    }
    public bool GetHit()
    {
        return this.hit;
    }

}
