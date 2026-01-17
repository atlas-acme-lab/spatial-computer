using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IterateChildDisplay : MonoBehaviour
{


    int currentIndex = 0;

    void Start()
    {
        currentIndex = 0;
        SetChildActive(currentIndex);
    }


    public void SetChildActive(int index)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i==index);
        }
    }

    public void NextChild()
    {
        currentIndex++;
        currentIndex = currentIndex >= transform.childCount ? 0 : currentIndex;
        SetChildActive(currentIndex);
    }
}
