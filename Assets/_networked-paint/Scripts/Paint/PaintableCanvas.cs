using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PaintableCanvas : MonoBehaviour
{

    enum EPaintingMode
    {
        Off,
        Draw
    }

    [SerializeField]
    float RaycastDistance = 10f;

    [SerializeField]
    LayerMask PaintableCanvasLayerMask = ~0;

    [SerializeField]
    MeshFilter CanvasMeshFilter;

    [SerializeField]
    MeshRenderer CanvasMeshRenderer;

    [SerializeField]
    int PixelsPerMeter = 200;

    [SerializeField]
    float BrushScale = 0.25f;

    [SerializeField]
    float BrushWeight = 0.25f;

    [SerializeField]
    Color CanvasDefaultColor = Color.white;

    public Color ActiveColor = Color.black;

    EPaintingMode PaintingMode_PrimaryMouse = EPaintingMode.Draw;
    EPaintingMode PaintingMode_SecondaryMouse = EPaintingMode.Draw;

    public bool ShouldPaint = true;



    int CanvasWidthInPixels;
    int CanvasHeightInPixels;

    public Texture2D PaintableTexture;

    public RenderTexture PaintableRenderTexture;

    BaseBrush ActiveBrush;

    public BaseBrush StartBrush;


    void Start()
    {
        CanvasWidthInPixels = Mathf.CeilToInt(CanvasMeshFilter.mesh.bounds.size.x * CanvasMeshFilter.transform.localScale.x * PixelsPerMeter);
        CanvasHeightInPixels = Mathf.CeilToInt(CanvasMeshFilter.mesh.bounds.size.y * CanvasMeshFilter.transform.localScale.y * PixelsPerMeter);

        Debug.Log("Width: " + CanvasWidthInPixels.ToString() + ", Height: " + CanvasHeightInPixels.ToString());

        PaintableTexture = new Texture2D(CanvasWidthInPixels, CanvasHeightInPixels, TextureFormat.ARGB32, false);

        for (int y = 0; y < CanvasHeightInPixels; y++)
        {
            for (int x = 0; x < CanvasWidthInPixels; x++)
            {
                PaintableTexture.SetPixel(x, y, CanvasDefaultColor);
            }
        }

        PaintableTexture.Apply();

        Graphics.Blit(PaintableTexture, PaintableRenderTexture);

        CanvasMeshRenderer.material.mainTexture = PaintableTexture;

        ShouldPaint = true;

        ActiveBrush = StartBrush;
        
    }

void Update()
    {
        if(ActiveBrush != null && ShouldPaint)
        {
            if (PaintingMode_PrimaryMouse == EPaintingMode.Draw && Input.GetMouseButton(0) )
            {
                Update_PerformDrawing(PaintingMode_PrimaryMouse);
            }
        }

        
    }

    RaycastHit[] HitResults = new RaycastHit[1];

    void Update_PerformDrawing(EPaintingMode PaintingMode)
    {
        Ray DrawingRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.RaycastNonAlloc(DrawingRay, HitResults, RaycastDistance, PaintableCanvasLayerMask) > 0)
        {
            // Debug.Log(HitResults[0].normal);
            Debug.Log(HitResults[0].textureCoord);

            PerformDrawignWith(ActiveBrush, ActiveColor, HitResults[0].textureCoord);
        }

    }

    void PerformDrawignWith(BaseBrush ActiveBrush, Color ActiveColor, Vector2 LocationUV)
    {
        int DrawingOriginX = Mathf.RoundToInt(LocationUV.x * CanvasWidthInPixels);
        int DrawingOriginY = Mathf.RoundToInt(LocationUV.y * CanvasHeightInPixels);
        int ScaledBrushWidth = Mathf.RoundToInt(ActiveBrush.BrushTexture.width * BrushScale);
        int ScaledBrushHeight = Mathf.RoundToInt(ActiveBrush.BrushTexture.height * BrushScale);

        ScaledBrushWidth = 8;
        ScaledBrushHeight = 8;

        for (int y = 0; y < ScaledBrushHeight; y++)
        {
            int pixelY = DrawingOriginY + y - (ScaledBrushHeight/2);

            if (pixelY < 0 || pixelY >= CanvasHeightInPixels)
                continue;
            
            // float BrushUV_Y = (float)y / (float)ScaledBrushHeight;


            for (int x = 0; x < ScaledBrushWidth; x++)
            {
                int pixelX = DrawingOriginX + x - (ScaledBrushWidth/2);

                if (pixelX < 0 || pixelX >= CanvasWidthInPixels)
                    continue;

                // float BrushUV_X = (float)x / (float)ScaledBrushWidth;

                // Color BrushPixel = ActiveBrush.BrushTexture.GetPixelBilinear(BrushUV_X, BrushUV_Y);
                // Color CanvasPixel = PaintableTexture.GetPixel(pixelX, pixelY);

                // CanvasPixel = ActiveBrush.Apply(CanvasPixel, BrushPixel, ActiveColor, BrushWeight);

                // PaintableTexture.SetPixel(pixelX, pixelY, CanvasPixel);
                PaintableTexture.SetPixel(pixelX, pixelY, Color.black);
            }

        }

        PaintableTexture.Apply();
        Graphics.Blit(PaintableTexture, PaintableRenderTexture);
    }


    public void SelectBrush(BaseBrush InBrush)
    {
        ActiveBrush = InBrush;
    }

    public void SetActiveColor(Color color)
    {
        ActiveColor = color;
    }

    public void ClearBoard()
    {
        for (int y = 0; y < CanvasHeightInPixels; y++)
        {
            for (int x = 0; x < CanvasWidthInPixels; x++)
            {
                PaintableTexture.SetPixel(x, y, CanvasDefaultColor);
            }
        }

        PaintableTexture.Apply();

        Graphics.Blit(PaintableTexture, PaintableRenderTexture);

        // CanvasMeshRenderer.material.mainTexture = PaintableTexture;
    }

}
 