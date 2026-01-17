using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCaptureTest : MonoBehaviour
{

    public Texture ScreenTexture;

    

    public GameObject C1;
    public GameObject C2;
    public GameObject C3;
    public GameObject C4;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            // StartCoroutine(RecordFrame());
        }
    }

    IEnumerator RecordFrame()
    {
        yield return new WaitForEndOfFrame();
        ScreenTexture = ScreenCapture.CaptureScreenshotAsTexture();
        //ScreenTexture = texture;
        // do something with texture

        // cleanup
        // Object.Destroy(texture);
    }

    // public void LateUpdate()
    // {
        // StartCoroutine(RecordFrame());
    // }
}
