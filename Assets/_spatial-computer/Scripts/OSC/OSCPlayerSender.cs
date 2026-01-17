using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class OSCPlayerSender : MonoBehaviour
{

    OSCTransmitter _transmitter;
    void Start()
    {
        _transmitter = GetComponent<OSCTransmitter>();
    }

    public void WhiteboardColorChange(Color _newColor)
    {
        var message = new OSCMessage("/whiteboard/color");
        message.AddValue(OSCValue.Float(_newColor.r));
        message.AddValue(OSCValue.Float(_newColor.g));
        message.AddValue(OSCValue.Float(_newColor.b));
        _transmitter.Send(message);
    }

    public void WhiteboardStartDrawing(Vector3 position)
    {
        var message = new OSCMessage("/whiteboard/start");
        message.AddValue(OSCValue.Float(position.x));
        message.AddValue(OSCValue.Float(position.y));
        message.AddValue(OSCValue.Float(position.z));
        _transmitter.Send(message);
    }

    public void WhiteboardKeepDrawing(Vector3 position)
    {
        var message = new OSCMessage("/whiteboard/keep");
        message.AddValue(OSCValue.Float(position.x));
        message.AddValue(OSCValue.Float(position.y));
        message.AddValue(OSCValue.Float(position.z));
        _transmitter.Send(message);
    }

    public void WhiteboardStopDrawing()
    {
        var message = new OSCMessage("/whiteboard/stop");
        message.AddValue(OSCValue.Int(1));
        _transmitter.Send(message);
    }

    public void WhiteboardClearDrawing()
    {
        var message = new OSCMessage("/whiteboard/clear");
        message.AddValue(OSCValue.Int(1));
        _transmitter.Send(message);
    }


}
