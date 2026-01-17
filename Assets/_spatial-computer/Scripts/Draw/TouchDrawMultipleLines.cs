using System.Collections.Generic;
using UnityEngine;

public class TouchDrawMultipleLines : MonoBehaviour
{
    public GameObject WhiteboardObject;   // Assign the cube object in the Inspector
    public GameObject linePrefab;    // Prefab that has a LineRenderer component
    public float offsetAboveSurface = 0.01f;  // Small offset above the cube surface to avoid z-fighting

    public DrawingManager WhiteboardManager;
    public OSCPlayerSender OSCSender;

    public GameObject cursor;

    private bool isDrawing = false;  // To track when the user is drawing


    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
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
                if (hit.collider.gameObject == WhiteboardObject)
                {


                    Vector3 relativeHit = WhiteboardObject.transform.InverseTransformPoint(hit.point);

                    // Vector3 mousePoint = AdjustPointAboveSurface(hit.point, hit.normal);  // Offset the intersection point
                    Vector3 relativePoint = WhiteboardObject.transform.InverseTransformPoint(hit.point);

                    cursor.transform.position = screenHit;
                    
                    // WhiteboardManager.StartDrawing(relativePoint);
                    WhiteboardManager.StartDrawing(relativePoint);
                    
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
                if (hit.collider.gameObject == WhiteboardObject)
                {

                    // Debug.Log(screenHit);


                    cursor.transform.position = screenHit;

                    // Vector3 screenCoord = Camera.main.WorldToScreenPoint(hit.point); 

                    Vector3 mousePoint = AdjustPointAboveSurface(hit.point, hit.normal);  // Offset the intersection point
                    
                    Vector3 relativePoint = WhiteboardObject.transform.InverseTransformPoint(hit.point);
                    WhiteboardManager.KeepDrawing(relativePoint); 
                    // OSCSender.WhiteboardKeepDrawing(relativePoint);
                    
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
            
            WhiteboardManager.StopDrawing(); 
            // OSCSender.WhiteboardStopDrawing();
            
            // touchPoints.Clear();
            // currentLineRenderer = null;
        }
    }

    public void ClearDrawing()
    {
        WhiteboardManager.ClearDrawing();
        // OSCSender.WhiteboardClearDrawing();
    }

    // void HandleTouchInput()
    // {
    //     if (Input.touchCount > 0)
    //     {
    //         Touch touch = Input.GetTouch(0);
    //         Ray ray = Camera.main.ScreenPointToRay(touch.position);
    //         RaycastHit hit;

    //         // Perform a raycast to see if the touch hits the cube
    //         if (Physics.Raycast(ray, out hit))
    //         {
    //             if (hit.collider.gameObject == WhiteboardObject)
    //             {
    //                 Vector3 touchPoint = AdjustPointAboveSurface(hit.point, hit.normal);  // Offset the intersection point

    //                 if (touch.phase == TouchPhase.Began)
    //                 {
    //                     CreateNewLine();
    //                     touchPoints.Clear();  // Clear previous points
    //                     AddPointToCurrentLine(touchPoint);
    //                     isDrawing = true;  // Start drawing
    //                 }
    //                 else if (touch.phase == TouchPhase.Moved && isDrawing)
    //                 {
    //                     AddPointToCurrentLine(touchPoint);
    //                 }
    //             }
    //         }

    //         if (touch.phase == TouchPhase.Ended && isDrawing)
    //         {
    //             isDrawing = false;  // Stop drawing
    //             touchPoints.Clear();
    //             currentLineRenderer = null;
    //         }
    //     }
    // }

    // void CreateNewLine()
    // {
    //     // Instantiate a new LineRenderer from the prefab
    //     GameObject newLine = Instantiate(linePrefab);
    //     currentLineRenderer = newLine.GetComponent<LineRenderer>();

    //     if (currentLineRenderer != null)
    //     {
    //         currentLineRenderer.startWidth = lineWidth;
    //         currentLineRenderer.endWidth = lineWidth;
    //         currentLineRenderer.positionCount = 0;

    //         // Store the new LineRenderer in the list of lines
    //         lines.Add(currentLineRenderer);
    //     }
    // }

    // void AddPointToCurrentLine(Vector3 point)
    // {
    //     if (currentLineRenderer != null)
    //     {
    //         // Add the new point to the list of touch points
    //         touchPoints.Add(point);

    //         // Update the LineRenderer with the new points
    //         currentLineRenderer.positionCount = touchPoints.Count;
    //         currentLineRenderer.SetPositions(touchPoints.ToArray());
    //     }
    // }

    /// <summary>
    /// Adjusts the point slightly above the surface by applying an offset in the direction of the surface normal.
    /// </summary>
    Vector3 AdjustPointAboveSurface(Vector3 point, Vector3 normal)
    {
        return point + normal * offsetAboveSurface;
    }
}
