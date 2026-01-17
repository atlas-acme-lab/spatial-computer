using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrushPicker : MonoBehaviour
{
    [SerializeField]
    List<BaseBrush> Brushes = new();

    [SerializeField]
    GameObject BrushUIPrefab;

    [SerializeField]
    Transform BrushUIRoot;

    [SerializeField]
    UnityEvent<BaseBrush> OnBrushChanged = new();

    void Start()
    {
        foreach(var Brush in Brushes)
        {
            var NewBrushUIGameObject = GameObject.Instantiate(BrushUIPrefab, BrushUIRoot);
            var NewBrushUILogic = NewBrushUIGameObject.GetComponent<BrushElement>();
            NewBrushUILogic.BindToBrush(Brush);
            NewBrushUILogic.OnBrushSelected.AddListener(OnBrushSelectedInternal);
        }
    }

    void Update()
    {
        
    }

    void OnBrushSelectedInternal(BaseBrush InBrush)
    {
        OnBrushChanged.Invoke(InBrush);
        Debug.Log(InBrush);
    }
}
