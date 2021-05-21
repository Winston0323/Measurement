using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class rayController : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer ray;
    public float lineWidth = 0.1f;
    public float lineMaxLength = 2f;
    public GameObject hitObj;

    void Start()
    {
        Vector3[] lineInit = new Vector3[2] { Vector3.zero, Vector3.zero };
        ray.SetPositions(lineInit);
        ray.enabled = true;
        ray.startWidth = lineWidth;
        ray.endWidth = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        rayProducer(transform.position, transform.forward, lineMaxLength);
    }
    private void rayProducer(Vector3 targetPos, Vector3 direction, float lineLength)
    {
        RaycastHit hit;
        //targetPos = targetPos + new Vector3(0.0f, 0.0f, 0.1f);
        Ray pointRay = new Ray(targetPos, direction);

        Vector3 endPoint = targetPos + (lineLength * direction);
        if (Physics.Raycast(pointRay, out hit))
        {
            endPoint = hit.point;
            hitObj = hit.collider.gameObject;
            if (hitObj.name == "target")
            {
                Debug.Log("hitting targetttttttt");
                GameManagerController.GMC.SetHit(true);
            }
            else{
                GameManagerController.GMC.SetHit(false);
            }
            //Debug.Log("ray hit "+ hitObj.name.ToString());
            if (hitObj.name == "FOVButton")
            {
                //GameManagerController.GMC.ResetPlayer();
                GameManagerController.GMC.HideMenu();
                SceneManager.LoadScene("FOV", LoadSceneMode.Additive);
            }
            else if (hitObj.name == "ResButton")
            {
                GameManagerController.GMC.HideMenu();
                SceneManager.LoadScene("Resolution", LoadSceneMode.Additive);
            }
            else if (hitObj.name == "CTPButton") 
            {
                GameManagerController.GMC.HideMenu();
                SceneManager.LoadScene("CTP", LoadSceneMode.Additive);
            }
            else if (hitObj.name == "PPCButton")
            {
                GameManagerController.GMC.HideMenu();
                SceneManager.LoadScene("PPC", LoadSceneMode.Additive);
            }
            else if (hitObj.name == "CDECButton")
            {
                GameManagerController.GMC.HideMenu();
                GameManagerController.GMC.ShowCylinder();
                SceneManager.LoadScene("CDEC", LoadSceneMode.Additive);
            }
            //targetPos = targetPos + new Vector3(0f, 0f, 0.5f);
            ray.SetPosition(0, targetPos);
            ray.SetPosition(1, endPoint);
        }
    }
}
