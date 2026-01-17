using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using Unity.VisualScripting;

public class OSCPlayerReceiver : MonoBehaviour
{


    public DrawingManager WhiteboardManager;
    OSCReceiver _receiver;

    void Start()
    {
        _receiver = GetComponent<OSCReceiver>();
        _receiver.Bind("/whiteboard/color", OSCWhiteboardColorChange);
        _receiver.Bind("/whiteboard/start", OSCWhiteboardStartDrawing);
        _receiver.Bind("/whiteboard/keep", OSCWhiteboardKeepDrawing);
        _receiver.Bind("/whiteboard/stop", OSCWhiteboardStopDrawing);
        _receiver.Bind("/whiteboard/clear", OSCWhiteboardClearDrawing);
    }

    public void OSCWhiteboardColorChange(OSCMessage message)
    {
        var color = new Color(message.Values[0].FloatValue, message.Values[1].FloatValue, message.Values[2].FloatValue);
        WhiteboardManager.SetColor(color);
    }

    public void OSCWhiteboardStartDrawing(OSCMessage message)
    {
        var position = new Vector3(message.Values[0].FloatValue, message.Values[1].FloatValue, message.Values[2].FloatValue);
        WhiteboardManager.StartDrawing(position);
    }

    public void OSCWhiteboardKeepDrawing(OSCMessage message)
    {
        Debug.Log(message.ToString());
        var position = new Vector3(message.Values[0].FloatValue, message.Values[1].FloatValue, message.Values[2].FloatValue);
        WhiteboardManager.KeepDrawing(position);
    }

    public void OSCWhiteboardStopDrawing(OSCMessage message)
    {
        WhiteboardManager.StopDrawing();
    }

    public void OSCWhiteboardClearDrawing(OSCMessage message)
    {
        WhiteboardManager.ClearDrawing();
    }


}