using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraLock : MonoBehaviour
{

    public GameObject FaceCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = FaceCamera.transform.position;
        if(this.transform.localPosition.z>=-0.1f)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, -0.1f);
        }
    }
}
