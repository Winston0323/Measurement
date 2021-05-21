using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FOVController : MonoBehaviour
{
    // Start is called before the first frame update
    private float dTime;
    public float countFrom = 3.0f;

    private GameObject player;
    public Vector3 fowardDir;
    public Vector3 leftEyePos;
    public Vector3 rightEyePos;
    public Vector2 rightStk;

    public TextMeshPro prompt;
    public TextMeshPro counter;

    private GameObject currFov;
    public GameObject wideFov;
    public GameObject heightFov;
    
    public Vector3 leftPos;
    public Vector3 rightPos;

    public bool change = false;
    public bool back = false;
    public bool protect = false;
    public bool startTimer = false;
    
    void Start()
    {

        player = GameObject.Find("OVRCameraRig");
        currFov = wideFov;
/*        wideFov.transform.position = wideFov.transform.position + fowardDir;
        wideFov.transform.SetParent(player.transform.GetChild(0).GetChild(1).transform);
        heightFov.transform.SetParent(player.transform.GetChild(0).GetChild(1).transform);*/
    }
    // Update is called once per frame
    void Update()
    {
        //update time
        dTime = dTime + Time.deltaTime;
        //update eye
        /*        leftEyePos = GameManagerController.GMC.PlayerLeftEyePos();
                rightEyePos = GameManagerController.GMC.PlayerRightEyePos();
                fowardDir = GameManagerController.GMC.FowardDirection();*/

        //fowardDir = GameManagerController.GMC.FowardDirection();
        
        rightStk = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        //move the current wall
        if (rightStk.y > 0)
        {
            currFov.transform.position = currFov.transform.position + new Vector3(0.0f, 0.0f, 0.01f);
        }
        else if (rightStk.y < 0)
        {
            currFov.transform.position = currFov.transform.position - new Vector3(0.0f, 0.0f, 0.01f);
        }
        //update wall side position
        leftPos = currFov.transform.GetChild(1).position;
        rightPos = currFov.transform.GetChild(2).position;
        

        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.9)
        {
            startTimer = true;
            
            leftEyePos = player.transform.GetChild(0).GetChild(0).position;
            rightEyePos = player.transform.GetChild(0).GetChild(2).position;
            Vector3 leftVec = leftEyePos - leftPos;
            Vector3 rightVec = rightEyePos - rightPos;
            float FOV = Vector3.Angle(leftVec, rightVec);
            
            if (change == false)
            {
                prompt.gameObject.SetActive(true);
                prompt.text = "Horizontal FOV is "+ FOV.ToString("000")+ " degree";
                GameManagerController.GMC.ChangeText("horiFOV", FOV);
                
                change = true;
                protect = true;
            }
            else {
                //heightFov.SetActive(false);
                //loadMenu();
                if (!protect) {
                    prompt.gameObject.SetActive(true);
                    prompt.text = "Vertical FOV is " + FOV.ToString("000") + " degree";
                    GameManagerController.GMC.ChangeText("vertFOV", FOV);
                    back = true;
                }
                
            }
            
        }


        // gp back and load menu
        if (back == true)
        {
            if (countFrom < 0)
            {
                heightFov.SetActive(false);
                loadMenu();
            }
        }
        //switch to second scene
        if (change == true && back!=true){
            //switch testing wall
            if (countFrom < 0)
            {
                wideFov.SetActive(false);
                heightFov.SetActive(true);
                currFov = heightFov;
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

    private void loadMenu() {
        SceneManager.UnloadSceneAsync("FOV");
        GameManagerController.GMC.ShowMenu();
    }
}
