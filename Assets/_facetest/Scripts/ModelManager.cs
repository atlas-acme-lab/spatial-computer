using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModelManager : MonoBehaviour
{

    public GameObject RoomObject;
    public float speed = 0.01f;

    bool forward;
    bool back;
    bool left;
    bool right;
    bool up;
    bool down;

    // Start is called before the first frame update
    void Start()
    {
        forward = false;
        back = false;
        left = false;
        right = false;
        up = false;
        down = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(forward)
        {
            RoomObject.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        if(back)
        {
            RoomObject.transform.Translate(Vector3.back * Time.deltaTime * speed);
        }

        if(left)
        {
            RoomObject.transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if(right)
        {
            RoomObject.transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        if(up)
        {
            RoomObject.transform.Translate(Vector3.up * Time.deltaTime * speed);
        }

        if(down)
        {
            RoomObject.transform.Translate(Vector3.down * Time.deltaTime * speed);
        }
    }

    public void Forward(bool state)
    {
        forward = state;
    }

        public void Back(bool state)
    {
        back = state;
    }

        public void Left(bool state)
    {
        left = state;
    }

        public void Right(bool state)
    {
        right = state;
    }

        public void Up(bool state)
    {
        up = state;
    }

        public void Down(bool state)
    {
        down = state;
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
