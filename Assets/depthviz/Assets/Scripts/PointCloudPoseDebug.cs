using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointCloudPoseDebug : MonoBehaviour
{

    public float CloudDistance = -1f;
    public float CloudScale = 3f;
    public float FieldOfView = 60f;

    public GameObject DisplayPlane;

    public GameObject DisplayImage;

    public Camera HeadCam;

    public GameObject PointCloud;

    public Text DebugInfo;

    public TMP_InputField DistanceX;

    public TMP_InputField DistanceY;


    public TMP_InputField DistanceZ;


    public TMP_InputField Scale;

    public TMP_InputField Focus;
    public TMP_InputField Size;

    public Material pointMat;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.W))
        {
            DisplayPlane.transform.localPosition += 0.01f*Vector3.up;
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            DisplayPlane.transform.localPosition -= 0.01f*Vector3.up;
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            DisplayPlane.transform.localPosition += 0.01f*Vector3.right;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            DisplayPlane.transform.localPosition -= 0.01f*Vector3.right;
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            DisplayImage.transform.localPosition -= 0.02f*Vector3.forward;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            DisplayImage.transform.localPosition += 0.02f*Vector3.forward;
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            DisplayImage.transform.localPosition -= 0.02f*Vector3.right;
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            DisplayImage.transform.localPosition += 0.02f*Vector3.right;
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            DisplayImage.transform.localPosition -= 0.02f*Vector3.up;
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            DisplayImage.transform.localPosition += 0.02f*Vector3.up;
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            DisplayImage.transform.localScale -= 0.00002f * Vector3.one;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            DisplayImage.transform.localScale += 0.00002f * Vector3.one;
        }

        // if(Input.GetKeyDown(KeyCode.N))
        // {
        //     CloudDistance -= 0.1f;
        // }

        // if(Input.GetKeyDown(KeyCode.M))
        // {
        //     CloudDistance += 0.1f;            
        // }

        // if(Input.GetKeyDown(KeyCode.K))
        // {
        //     FieldOfView -= 1f;
        // }

        // if(Input.GetKeyDown(KeyCode.L))
        // {
        //     FieldOfView += 1f;
        // }


        // if(Input.GetKeyDown(KeyCode.O))
        // {
        //     CloudScale -= 0.25f;
        // }

        // if(Input.GetKeyDown(KeyCode.P))
        // {
        //     CloudScale += 0.25f;
        // }


        // if(Focus.text != null)
        // {
        //     HeadCam.fieldOfView = float.Parse(Focus.text);
        // }

        // if(DistanceX.text != null &&  DistanceY.text != null && DistanceZ.text != null)
        // {
        //     PointCloud.transform.position = new Vector3(float.Parse(DistanceX.text),float.Parse(DistanceY.text),float.Parse(DistanceZ.text));
        // }

        // if(Scale.text != null)
        // {
        //     PointCloud.transform.localScale = Vector3.one * float.Parse(Scale.text);
        // }

        // if(Size.text != null)
        // {
        //     pointMat.SetFloat("pointCloud", float.Parse(Size.text));
        // }

        

                
        //DebugInfo.text = "Dis: " + PointCloud.transform.position.ToString() + '\n' + "Scale: " + PointCloud.transform.localScale + '\n' + "FoV: " + HeadCam.fieldOfView.ToString();
        DebugInfo.text = "Plane: " + DisplayPlane.transform.localPosition.ToString() + '\n' + "Img: " + DisplayImage.transform.localPosition.ToString() + ", Scale: " + (DisplayImage.transform.localScale.x * 1000f).ToString();


    }



}
