using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class BrushElement : MonoBehaviour
{

    [SerializeField]
    RawImage BrushImage;

    [SerializeField]
    TextMeshProUGUI BrushName;

    public UnityEvent<BaseBrush> OnBrushSelected = new();

    BaseBrush LinkedBrush;

    public void BindToBrush(BaseBrush InBrush)
    {
        LinkedBrush = InBrush;
        BrushImage.texture = InBrush.BrushTexture;
        BrushName.text = InBrush.DisplayName;
    }

    public void OnBrushElementClicked(BaseEventData InEventData)
    {
        if(InEventData is PointerEventData)
        {
            OnBrushSelected.Invoke(LinkedBrush);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
