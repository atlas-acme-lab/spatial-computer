using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public Camera cam; // Reference to the camera
    public Transform C1, C2, C3, C4; // Corner transforms

    private LineRenderer lineRenderer;

    void Start()
    {
        // Create a new LineRenderer or get the existing one
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 5; // 4 corners + 1 to close the rectangle
        lineRenderer.loop = true; // Automatically closes the shape
        lineRenderer.useWorldSpace = false; // Set to screen space coordinates
        lineRenderer.startWidth = 0.02f; // Adjust the width as needed
        lineRenderer.endWidth = 0.02f;
    }

    void Update()
    {
        DrawScreenSpaceRectangle();
    }

    private void DrawScreenSpaceRectangle()
    {
        // Get the screen space coordinates of each corner
        Vector3 screenCoord1 = cam.WorldToScreenPoint(C1.position);
        Vector3 screenCoord2 = cam.WorldToScreenPoint(C2.position);
        Vector3 screenCoord3 = cam.WorldToScreenPoint(C3.position);
        Vector3 screenCoord4 = cam.WorldToScreenPoint(C4.position);

        // Convert screen space coordinates to world space positions
        Vector3 worldCoord1 = cam.ScreenToWorldPoint(new Vector3(screenCoord1.x, screenCoord1.y, cam.nearClipPlane));
        Vector3 worldCoord2 = cam.ScreenToWorldPoint(new Vector3(screenCoord2.x, screenCoord2.y, cam.nearClipPlane));
        Vector3 worldCoord3 = cam.ScreenToWorldPoint(new Vector3(screenCoord3.x, screenCoord3.y, cam.nearClipPlane));
        Vector3 worldCoord4 = cam.ScreenToWorldPoint(new Vector3(screenCoord4.x, screenCoord4.y, cam.nearClipPlane));

        // Update the positions of the line renderer to form the rectangle
        lineRenderer.SetPosition(0, worldCoord1);
        lineRenderer.SetPosition(1, worldCoord2);
        lineRenderer.SetPosition(2, worldCoord3);
        lineRenderer.SetPosition(3, worldCoord4);
        lineRenderer.SetPosition(4, worldCoord1); // Close the loop
    }
}
