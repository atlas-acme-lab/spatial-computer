using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawingManager: MonoBehaviour{

    [SerializeField]
    private GameObject _whiteboardObject;

    [SerializeField]
    private GameObject _linePrefab;

    public Material MeshMaterial;

    private List<Vector3> _touchPoints = new List<Vector3>(); 
    private List<LineRenderer> _lines = new List<LineRenderer>(); 
    private LineRenderer _currentLineRenderer;
    private bool _isDrawing = false;
    private Color _currentLineColor = Color.blue;

    public void StartDrawing(Vector3 linePosition)
    {
        GameObject newLine = Instantiate(_linePrefab);
        newLine.transform.parent = _whiteboardObject.transform;
        // newLine.transform.localPosition = Vector3.zero;
        _currentLineRenderer = newLine.GetComponent<LineRenderer>();

        if(_currentLineRenderer != null)
        {
            _currentLineRenderer.startColor = _currentLineColor;
            _currentLineRenderer.endColor = _currentLineColor;
            _currentLineRenderer.positionCount = 0;
            _lines.Add(_currentLineRenderer);
        }

        _touchPoints.Clear();

        AddPointToCurrentLine(linePosition);

        _isDrawing = true;
    }

    public void KeepDrawing(Vector3 linePosition)
    {
        AddPointToCurrentLine(linePosition);
    }

    public void StopDrawing()
    {
        _isDrawing = false;
        GameObject go = _currentLineRenderer.gameObject;
        BakeLineDebugger(_currentLineRenderer.gameObject);
        // go.transform.SetParent(_whiteboardObject.transform);
        _touchPoints.Clear();
        _currentLineRenderer = null;
    }

    void AddPointToCurrentLine(Vector3 point)
    {
        if (_currentLineRenderer != null)
        {

            var relativePoint = _whiteboardObject.transform.TransformPoint(point);
            
            // Add the new point to the list of touch points
            _touchPoints.Add(relativePoint);
            // _touchPoints.Add(point);

            // Update the LineRenderer with the new points
            _currentLineRenderer.positionCount = _touchPoints.Count;
            _currentLineRenderer.SetPositions(_touchPoints.ToArray());
        }
    }

    public void SetColor(Color lineColor)
    {
        _currentLineColor = lineColor;
    }

    public void ClearDrawing()
    {
        _currentLineRenderer = null;
        _lines.Clear();
        foreach(Transform child in _whiteboardObject.transform)
        {
            Destroy(child.gameObject);
        }       
    }

    public void BakeLineDebugger(GameObject lineObj)
 {
     var lineRenderer = lineObj.GetComponent<LineRenderer>();
     var meshFilter = lineObj.AddComponent<MeshFilter>();
     Mesh mesh = new Mesh();
     lineRenderer.BakeMesh(mesh);
     meshFilter.sharedMesh = mesh;

     var meshRenderer = lineObj.AddComponent<MeshRenderer>();
     meshRenderer.sharedMaterial = MeshMaterial;

     GameObject.Destroy(lineRenderer);
 }



}