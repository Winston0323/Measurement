using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    private float dTime;
    public float countFrom = 3.0f;

    // Start is called before the first frame update
    public Vector2 rightStk;
    private GameObject player;

    public GameObject hStripBox;
    public GameObject vStripBox;
    public GameObject currStripBox;
    
    public float SBLP = 0.1f;
    public float dist;
    public float resolution;
    public float temp;

    public TextMeshPro prompt;
    public TextMeshPro counter;

    public bool change = false;
    public bool back = false;
    public bool protect = false;
    public bool startTimer = false;

    void Start()
    {
        player = GameObject.Find("OVRCameraRig");
        currStripBox = hStripBox;
        
    }

    // Update is called once per frame
    void Update()
    {
        dTime = dTime + Time.deltaTime;
        rightStk = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        //moving current strip box
        if (rightStk.y > 0)
        {
            currStripBox.transform.position = currStripBox.transform.position + new Vector3(0.0f, 0.0f, 0.05f);
        }
        else if (rightStk.y < 0)
        {
            currStripBox.transform.position = currStripBox.transform.position - new Vector3(0.0f, 0.0f, 0.03f);
        }
        //update distance
        dist = currStripBox.transform.position.z - player.transform.position.z;
        //when controller being grabbed
        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.9) {
            
            startTimer = true;
            
            //calculate resolution
            temp = Mathf.Tan(3.14f/180f);
            resolution = dist * temp / 0.1f;

            if (change == false)
            {
                prompt.gameObject.SetActive(true);
                prompt.text = "Horizontal Res is " + resolution.ToString("00.00");
                GameManagerController.GMC.ChangeText("horiRes", resolution);
                change = true;
                protect = true;
            }
            else
            {
                if (!protect)
                {
                    prompt.gameObject.SetActive(true);
                    prompt.text = "Vertical Res is " + resolution.ToString("00.00");
                    GameManagerController.GMC.ChangeText("vertRes", resolution);
                    back = true;
                }

            }
        }


        // gp back and load menu
        if (back == true)
        {
            if (countFrom < 0)
            {
                vStripBox.SetActive(false);
                loadMenu();
            }
        }
        //switch to second scene
        if (change == true && back != true)
        {
            //switch testing wall
            if (countFrom < 0)
            {
                hStripBox.SetActive(false);
                vStripBox.SetActive(true);
                currStripBox = vStripBox;
                countFrom = 3.0f;
                protect = false;
            }
            else if (countFrom < 1) {
                prompt.gameObject.SetActive(false);
            }
        }

        //start the timer
        if (startTimer)
        {
            counter.gameObject.SetActive(true);
            countFrom = countFrom - Time.deltaTime;
            counter.text = countFrom.ToString("000");
            //stop the timer
            if (countFrom < 1)
            {
                
            }
            if (countFrom < 0)
            {
                startTimer = false;
                counter.gameObject.SetActive(false);
            }
        }

    }
    private void loadMenu()
    {
        SceneManager.UnloadSceneAsync("Resolution");
        GameManagerController.GMC.ShowMenu();
    }
}
