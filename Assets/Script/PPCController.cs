using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class PPCController : MonoBehaviour
{
    //timers
    public static PPCController PPCC;
    public float countDown = 3.0f;
    public float dTime = 0.0f;
    public float bkCDown = 3.0f;
    public float gameTime = 20.0f;
    public float distance;

    public int hitTime;
    public int missTime;
    public int hitMax = 20;
    
    public TextMeshPro prompt;
    public TextMeshPro bkPrompt;
    public TextMeshPro gameTimer;
    public TextMeshPro hitCntr;
    public TextMeshPro missCntr;
    //the target that we going move
    public GameObject target;
    public GameObject hitObj;
    public Transform hand;


    public bool startTimer;
    public bool gameStart;
    public bool load;
    public bool back;
    // Start is called before the first frame update
    private void Awake()
    {
       PPCC = this;
    }

    void Start()
    {
        hand = GameObject.Find("OVRCameraRig").transform.GetChild(0).GetChild(5);
    }

    // Update is called once per frame
    void Update()
    {
        dTime = dTime + Time.deltaTime;
        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.9)
        {
            if (gameStart != true && !back)
            {
                startTimer = true;
                prompt.gameObject.SetActive(true);
            }

        }
        //starting the count down
        if (startTimer && !gameStart)
        {
            countDown = countDown - Time.deltaTime;
            if (countDown > 0.0f) {
                prompt.text = "Starting in " + countDown.ToString("0");
            }
            
            //count down end
            if (countDown <= 0.0f &&startTimer == true)
            {
                prompt.gameObject.SetActive(false);
                gameStart = true;
                startTimer = false;
            }
        }

        //game started
        if (gameStart)
        {
            gameTime = gameTime - Time.deltaTime;
            gameTimer.text = gameTime.ToString("00");

            if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) < 0.3)
            {
                Debug.Log("loading");
                load = true;
            }
            else if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9)
            {
                //when ok to shoot
                if (load)
                {               
                    if (GameManagerController.GMC.GetHit())
                    {
                        hitTime++;
                        hitCntr.text = hitTime.ToString("00");
                        load = false;
                    }
                    else {
                        missTime++;
                        missCntr.text = missTime.ToString("00");
                        load = false;
                        //when miss too much
                        if (missTime > 10) {
                            distance = target.transform.position.z;
                            Debug.Log("game ended cause miss time too high");
                            prompt.gameObject.SetActive(true);
                            prompt.text = (distance - 1).ToString("00") + " < Precesion < " + distance.ToString("00");
                            GameManagerController.GMC.ChangeText("PPC", distance);
                            gameStart = false;
                            back = true;
                        }    
                    }
                }
            }
            
            //when time up
            if (gameTime <= 0 || (hitTime + missTime == 20))
            {
                if (hitTime < 10)
                {
                    distance = target.transform.position.z;
                    Debug.Log("game ended cause hit time too low");
                    prompt.gameObject.SetActive(true);
                    prompt.text = (distance - 1).ToString("00") + " < Precesion < " + distance.ToString("00");
                    GameManagerController.GMC.ChangeText("PPC", distance);
                    ResetAll();
                    back = true;
                }
                else {
                    prompt.gameObject.SetActive(true);
                    prompt.text = "Sucess!";
                    target.transform.position = target.transform.position + new Vector3(0.0f, 0.0f, 1.0f);
                    ResetAll();
                }
            }
            //reach 20 hits
            if (hitTime == hitMax) {
                prompt.gameObject.SetActive(true);
                prompt.text = "Sucess!";
                target.transform.position = target.transform.position + new Vector3(0.0f, 0.0f, 1.0f);
                ResetAll();
            }

        }
        if (back)
        {
            bkCDown = bkCDown - Time.deltaTime;
            bkPrompt.gameObject.SetActive(true);
            bkPrompt.text = "Heading back in" + bkCDown.ToString("0");
            if (bkCDown <= 0.0f)
            {
                loadMenu();
            }
        }

    }
    private void ResetAll() {
        countDown = 3.0f;
        gameTime = 20.0f;
        hitTime = 0;
        missTime = 0;
        startTimer = false;
        gameStart = false;
        hitCntr.text = "00";
        missCntr.text = "00";
    }

    private void loadMenu()
    {
        SceneManager.UnloadSceneAsync("PPC");
        GameManagerController.GMC.ShowMenu();
    }
}