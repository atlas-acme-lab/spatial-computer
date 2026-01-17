using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class NewFusionPlayer : NetworkBehaviour
{

    [Networked]
    public Vector3 relPosition { get; set; }

    [Networked]
    public Quaternion relRotation { get; set; }

    [Networked]
    public Vector3 pointerPosition { get; set; }

    [Networked]
    public bool isPointerActive { get; set; }


    public bool IsLocalPlayer => Object.HasStateAuthority;

    private Vector3 velocity = Vector3.zero;

        private bool isDrawing = false;  // To track when the user is drawing


    public override void Spawned()
    {
        base.Spawned();
        if (!IsLocalPlayer)
        {
            BasicNetworkManager.Instance.RemotePlayer = this.gameObject;
            //BasicSpawner.Instance.RemotePlayerFrustum.transform.parent = this.gameObject.transform;
            //BasicSpawner.Instance.RemotePlayerFrustum.transform.localPosition = Vector3.zero;
            //BasicSpawner.Instance.RemotePlayerFrustum.transform.localRotation = Quaternion.identity;
        }
    }

    public override void FixedUpdateNetwork()
    {
        // if (GetInput(out NetworkInputData data))
        // {
        //     //this.transform.position = BasicDeviceManager.Instance.Origin.transform.TransformPoint(data.relativePosition);
        //     relPosition = data.relativePosition;
        //     //this.transform.rotation = BasicDeviceManager.Instance.Origin.transform.rotation * data.relativeRotation;
        //     relRotation = data.relativeRotation;

        //     pointerPosition = data.pointerPosition;

        //     isPointerActive = data.isPointerActive;

        // }

        //relPosition = BasicDeviceManager.Instance.relativePosition;
        //relRotation = BasicDeviceManager.Instance.relativeRotation;

    }

    private void Update()
    {

        if(IsLocalPlayer)
        {
            HandleMouseInput();
            Debug.Log("lOCAL");
        }
    }

    public void HandleMouseInput()
    {
        // Detect mouse press (left button) equivalent to touch
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 screenHit = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit hit;

            // Perform a raycast to see if the mouse click hits the cube
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == BasicNetworkManager.Instance.WhiteboardObject)
                {
                    Vector3 relativeHit = BasicNetworkManager.Instance.WhiteboardObject.transform.InverseTransformPoint(hit.point);

                    // Vector3 mousePoint = AdjustPointAboveSurface(hit.point, hit.normal);  // Offset the intersection point
                    Vector3 relativePoint = BasicNetworkManager.Instance.WhiteboardObject.transform.InverseTransformPoint(hit.point);

                    
                    // WhiteboardManager.StartDrawing(relativePoint);
                    //BasicNetworkManager.Instance.WhiteboardManager.StartDrawing(relativePoint);
                    RPC_StartLine(relativePoint);

                    
                    // OSCSender.WhiteboardStartDrawing(relativePoint);                    
                    // OSCSender.WhiteboardStartDrawing(relativePoint);                    
                    // CreateNewLine();
                    // touchPoints.Clear();  // Clear previous points
                    // AddPointToCurrentLine(mousePoint);
                    isDrawing = true;  // Start drawing
                }
            }
        }

        // Add points while the mouse is being dragged (mouse button held)
        if (Input.GetMouseButton(0) && isDrawing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 screenHit = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == BasicNetworkManager.Instance.WhiteboardObject)
                {

                    // Debug.Log(screenHit);



                    // Vector3 screenCoord = Camera.main.WorldToScreenPoint(hit.point); 

                    // Vector3 mousePoint = AdjustPointAboveSurface(hit.point, hit.normal);  // Offset the intersection point
                    
                    Vector3 relativePoint = BasicNetworkManager.Instance.WhiteboardObject.transform.InverseTransformPoint(hit.point);
                    
                    //WhiteboardManager.KeepDrawing(relativePoint); 
                    
                    // OSCSender.WhiteboardKeepDrawing(relativePoint);
                    RPC_KeepLine(relativePoint);
                    
                    // if(Vector3.Distance(mousePoint, previousPoint)>0.02f)
                    // AddPointToCurrentLine(mousePoint);

                    // previousPoint = mousePoint;
                }
            }
        }

        // Stop drawing when the mouse button is released
        if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            isDrawing = false;  // Stop drawing
            
            // WhiteboardManager.StopDrawing(); 
            // OSCSender.WhiteboardStopDrawing();
            RPC_StopLine();
            
            // touchPoints.Clear();
            // currentLineRenderer = null;
        }
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_StartLine(Vector3 pos)
    {
        //BasicNetworkManager.Instance.LocalDrawingManager.StartDrawing(pos);
        StartLine(pos);
    }

    void StartLine(Vector3 pos)
    {
        BasicNetworkManager.Instance.LocalDrawingManager.StartDrawing(pos);

    }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_KeepLine(Vector3 pos)
    {
        // BasicNetworkManager.Instance.LocalDrawingManager.KeepDrawing(pos);
        KeepLine(pos);
    }

    void KeepLine(Vector3 pos)
    {
        BasicNetworkManager.Instance.LocalDrawingManager.KeepDrawing(pos);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)] 
    public void RPC_StopLine()
    {
            StopLine();
    }

    void StopLine()
    {
                BasicNetworkManager.Instance.LocalDrawingManager.StopDrawing();

    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_ClearLine()
    {
        ClearLine();
    }

    void ClearLine()
    {
        BasicNetworkManager.Instance.LocalDrawingManager.ClearDrawing();

    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetColor(Color color)
    {
        // Color newcolor = new Color(x, y, z);
        // BasicNetworkManager.Instance.LocalDrawingManager.SetColor(color);
        SetColor(color);
    }

    void SetColor(Color color)
    {
        BasicNetworkManager.Instance.LocalDrawingManager.SetColor(color);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetModel0(bool state)
    {
        SetModel(0, state);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetModel1(bool state)
    {
        SetModel(1, state);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetModel2(bool state)
    {
        SetModel(2, state);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetModel3(bool state)
    {
        SetModel(3, state);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetModel4(bool state)
    {
        SetModel(4, state);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_SetModel5(bool state)
    {
        SetModel(5, state);
    }

    public void SetModel(int i, bool state)
    {
        BasicNetworkManager.Instance.SetModel(i, state);
    }

    // [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    // public void RPC_SetToggle(bool state)
    // {
    //     SetRecord(state);
    // }

    // public void SetRecord(bool state)
    // {
    //     BasicNetworkManager.Instance.SetRecordToggle(state);
    // }    







}
