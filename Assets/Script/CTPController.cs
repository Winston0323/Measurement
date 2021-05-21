using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CTPController : MonoBehaviour
{
    // Start is called before the first frame update
    public float dTime;
    public float lstTime;
    public float countFrom = 5.0f;
    public float waitTime = 3.0f;
    public GameObject player;
    public ArrayList posArr;
    public ArrayList angArr;
    public Vector3 posSum;
    public Vector3 angSum;
    public Vector3 posTemp;
    public Vector3 angTemp;

    public TextMeshPro prompt;
    public TextMeshPro counter;

    public bool start;
    public bool back;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("OVRCameraRig");
        posArr = new ArrayList();
        angArr = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        dTime = dTime + Time.deltaTime;
        //when grabbed
        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.9)
        {
            Debug.Log("reached here");
            if (!start)
            {
                start = true;
                lstTime = dTime;
            }
        }

        //when counter start
        if (start == true) {
            

            //when duration bigger than 0.05
            if (dTime - lstTime > 0.05f) {

                //get the position
                Vector3 handPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);
                posArr.Add(handPos);
                posSum = posSum + handPos;
                //get quaternion and turn it to angle
                Vector3 handAng = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RHand).eulerAngles;
                angArr.Add(handAng);
                angSum = angSum + handAng;
                
                Debug.Log("adding");
                //update time
                lstTime = dTime;
            }

            if (countFrom <= 0.0f)
            {
                //calculate mean 
                Vector3 posMean = posSum / posArr.Count;
                Vector3 angMean = angSum / angArr.Count;

                Debug.Log("pos size is " + posArr.Count);
                Debug.Log("ang size is " + angArr.Count);
                //calculate std 
                for (int i = 0; i < posArr.Count; i++)
                {
                    /*                    float xPos = ((Vector3)posArr[i]).x - posMean.x;
                                        float yPos = ((Vector3)posArr[i]).y - posMean.y;
                                        float zPos = ((Vector3)posArr[i]).z - posMean.z;
                                        posTemp = posTemp + new Vector3(xPos*xPos, yPos*yPos, zPos*zPos);*/
                    Vector3 a = (Vector3)posArr[i];
                    Debug.Log("Coverting arraylist to vector3" + a.ToString());
                    //Get the difference between vectors
                    Vector3 currPos = ((Vector3)posArr[i]) - posMean;
                    Vector3 currAng = ((Vector3)angArr[i]) - angMean;
                    
                    //add up the square of differences
                    posTemp = posTemp + Vector3.Scale(currPos, currPos);
                    angTemp = angTemp + Vector3.Scale(currAng, currAng);

                }
                //devide the size of array
                posTemp = posTemp / posArr.Count;
                angTemp = angTemp / angArr.Count;

                //calculate squareroot
                posTemp.x = Mathf.Sqrt(posTemp.x);
                posTemp.y = Mathf.Sqrt(posTemp.y);
                posTemp.z = Mathf.Sqrt(posTemp.z);

                angTemp.x = Mathf.Sqrt(angTemp.x);
                angTemp.y = Mathf.Sqrt(angTemp.y);
                angTemp.z = Mathf.Sqrt(angTemp.z);
                Debug.Log(posTemp.ToString());
                Debug.Log(angTemp.ToString());

                //record the result
                GameManagerController.GMC.ChangeText("posStd", posTemp);
                GameManagerController.GMC.ChangeText("angStd", angTemp);
                prompt.text = "Position std is" + posTemp.ToString();
                counter.text = "Angular std is" + angTemp.ToString();
                start = false;
                back = true;
                countFrom = 3.0f;
            }
            else {
                //update counter
                countFrom = countFrom - Time.deltaTime;
                counter.text = countFrom.ToString("0");
            }
        }
        if (back) {
            countFrom = countFrom - Time.deltaTime;
            if (countFrom < 0) {
                loadMenu();
            }
        }
        
    }
    private void loadMenu()
    {
        SceneManager.UnloadSceneAsync("CTP");
        GameManagerController.GMC.ShowMenu();
    }
}
