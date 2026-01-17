using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MathNet.Numerics;
using HomographySharp;
using MathNet.Numerics.LinearAlgebra;
public class PerspectiveTransformation : MonoBehaviour
{


    public GameObject WhiteboardPlane;

    public ObjectTransitionHandler WhiteboardTransition;
    public Texture2D ScreenTexture;

    public Texture2D blitScreenTexture;

    public Texture2D pngScreenTexture;

    public Texture2D inputTexture;

    public Texture2D outputTexture;
    public RawImage outputImage;
    public int outputWidth = 1200;
    public int outputHeight = 1200;

    public GameObject C1;
    public GameObject C2;
    public GameObject C3;
    public GameObject C4;

    private LineRenderer lineRenderer;

    void Start()
    {
        // Create a new LineRenderer or get the existing one
        // lineRenderer = gameObject.AddComponent<LineRenderer>();
        // lineRenderer.positionCount = 5; // 4 corners + 1 to close the rectangle
        // lineRenderer.loop = true; // Automatically closes the shape
        // lineRenderer.useWorldSpace = false; // Set to screen space coordinates
        // lineRenderer.startWidth = 0.001f; // Adjust the width as needed
        // lineRenderer.endWidth = 0.001f;
    }



    public void TransitionWhiteboard (bool value)
    {
        if(value)
        {
            WhiteboardTransition.Transition(value);
            StartCoroutine(DelayPanelOff());
        }
        else
        {
            WhiteboardPlane.SetActive(false);
            BeginRecordFrame();
            StartCoroutine(DelayTransitionBack());
        }

    }

    IEnumerator DelayPanelOff()
    {
        yield return new WaitForSeconds(1.2f);
        WhiteboardPlane.SetActive(true);
        outputImage.gameObject.SetActive(false);
    }

    IEnumerator DelayTransitionBack()
    {
        yield return new WaitForSeconds(1.2f);
        outputImage.gameObject.SetActive(true);
        WhiteboardTransition.Transition(false);
        WhiteboardPlane.SetActive(true);

    }


    IEnumerator PauseCapture()
    {

        // blitScreenTexture = DuplicateTexture(ScreenTexture);

        // blitScreenTexture = new Texture2D(ScreenTexture.width, ScreenTexture.height, TextureFormat.RGBA32, false);

        //     RenderTexture currentRT = RenderTexture.active;

        //     RenderTexture renderTexture = new RenderTexture(ScreenTexture.width, ScreenTexture.height, 32);
        //     Graphics.Blit(ScreenTexture, renderTexture);

        //     RenderTexture.active = renderTexture;
        //     blitScreenTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        //     blitScreenTexture.Apply();


        // string filename = Application.persistentDataPath + "/Screenshot.png";
        // var rawData = System.IO.File.ReadAllBytes(filename);
        // pngScreenTexture = new Texture2D(2, 2); // Create an empty Texture; size doesn't matter (she said)
        yield return new WaitForSeconds(2);
        // pngScreenTexture.LoadImage(rawData);
        TranslateImage();


    }


    public void BeginRecordFrame()
    {
        StartCoroutine(RecordFrame());
    }

    IEnumerator RecordFrame()
{
    yield return new WaitForEndOfFrame();
    ScreenTexture = ScreenCapture.CaptureScreenshotAsTexture();

    // Debug.Log($"ScreenTexture Size: {ScreenTexture.width}x{ScreenTexture.height}");
    // Debug.Log($"Sample Color at (0,0): {ScreenTexture.GetPixel(0, 0)}");

    blitScreenTexture = DuplicateTexture(ScreenTexture);
    TranslateImage();
}

void TranslateImage()
{



    // Get screen space coordinates of the corner points
    Camera cam = Camera.main;
    Vector3 screenCoord1 = cam.WorldToScreenPoint(C1.transform.position);
    Vector3 screenCoord2 = cam.WorldToScreenPoint(C2.transform.position);
    Vector3 screenCoord3 = cam.WorldToScreenPoint(C3.transform.position);
    Vector3 screenCoord4 = cam.WorldToScreenPoint(C4.transform.position);


    // Convert screen space coordinates to world space positions
        // Vector3 worldCoord1 = cam.ScreenToWorldPoint(new Vector3(screenCoord1.x, screenCoord1.y, cam.nearClipPlane));
        // Vector3 worldCoord2 = cam.ScreenToWorldPoint(new Vector3(screenCoord2.x, screenCoord2.y, cam.nearClipPlane));
        // Vector3 worldCoord3 = cam.ScreenToWorldPoint(new Vector3(screenCoord3.x, screenCoord3.y, cam.nearClipPlane));
        // Vector3 worldCoord4 = cam.ScreenToWorldPoint(new Vector3(screenCoord4.x, screenCoord4.y, cam.nearClipPlane));

        // // Update the positions of the line renderer to form the rectangle
        // lineRenderer.SetPosition(0, worldCoord1);
        // lineRenderer.SetPosition(1, worldCoord2);
        // lineRenderer.SetPosition(2, worldCoord3);
        // lineRenderer.SetPosition(3, worldCoord4);
        // lineRenderer.SetPosition(4, worldCoord1); // Close the loop

    // Debug log to verify screen coordinates
    // Debug.Log($"Screen Coordinates: C1({screenCoord1}), C2({screenCoord2}), C3({screenCoord3}), C4({screenCoord4})");

    // Define source points from screen coordinates
    Vector2[] srcPoints = {
        new Vector2(screenCoord1.x, screenCoord1.y),
        new Vector2(screenCoord2.x, screenCoord2.y),
        new Vector2(screenCoord3.x, screenCoord3.y),
        new Vector2(screenCoord4.x, screenCoord4.y)
    };

    // Vector2[] srcPoints = {
    //     new Vector2(0, 0),
    //     new Vector2(blitScreenTexture.width - 1, 0),
    //     new Vector2(blitScreenTexture.width - 1, blitScreenTexture.height - 1),
    //     new Vector2(0, blitScreenTexture.height - 1)
    // };

    // Define destination points (corners of the output rectangle)
    Vector2[] dstPoints = {
        new Vector2(0, 0),
        new Vector2(outputWidth - 1, 0),
        new Vector2(outputWidth - 1, outputHeight - 1),
        new Vector2(0, outputHeight - 1)
    };

    List<Point2<float>> sourcePoints = new List<Point2<float>>
        {
            new Point2<float>(srcPoints[0].x, srcPoints[0].y),
            new Point2<float>(srcPoints[1].x, srcPoints[1].y),
            new Point2<float>(srcPoints[2].x, srcPoints[2].y),
            new Point2<float>(srcPoints[3].x, srcPoints[3].y)
        };

        // Define 4 corresponding points in destination (transformed) space
        List<Point2<float>> destinationPoints = new List<Point2<float>>
        {
            new Point2<float>(dstPoints[0].x, dstPoints[0].y),
            new Point2<float>(dstPoints[1].x, dstPoints[1].y),
            new Point2<float>(dstPoints[2].x, dstPoints[2].y),
            new Point2<float>(dstPoints[3].x, dstPoints[3].y)
        };

        // Compute the homography matrix
        HomographyMatrix<float> homographyMatrix = Homography.Find(destinationPoints, sourcePoints);

        Matrix<float> homoMat = homographyMatrix.ToMathNetMatrix();

        // Log the homography matrix
        // Matrix3x3 matrix = homographyMatrix.Matrix;
        // Debug.Log($"Homography Matrix: \n{homoMat.ToString()}");

    // Homography.Find(srcList,dstList);

    // Get the perspective transformation matrix
    // Matrix4x4 transformMatrix = GetPerspectiveTransform(srcPoints, dstPoints);
    // Matrix4x4 transformMatrix = ComputeHomography(srcPoints, dstPoints);

    // Create a new Texture2D for the output
    outputTexture = new Texture2D(outputWidth, outputHeight, TextureFormat.RGBA32, false);

    // Loop through each pixel in the output image
    for (int y = 0; y < outputHeight; y++)
    {
        for (int x = 0; x < outputWidth; x++)
        {
            // Calculate the corresponding point in the source image
            // Vector2 srcPos = ApplyPerspectiveTransform(transformMatrix, new Vector2(x, y));

            Point2<float> transformedPoint = homographyMatrix.Translate(x, y);
            Vector2 srcPos = new Vector2(transformedPoint.X, transformedPoint.Y);
            
            
            // Sample the color only if the srcPos is within the source image bounds
            if (srcPos.x >= 0 && srcPos.x < blitScreenTexture.width && srcPos.y >= 0 && srcPos.y < blitScreenTexture.height)
            {
                Color color = blitScreenTexture.GetPixelBilinear(srcPos.x / blitScreenTexture.width, srcPos.y / blitScreenTexture.height);
                outputTexture.SetPixel(x, y, color);
            }
            else
            {
                outputTexture.SetPixel(x, y, Color.clear); // Set to transparent if out of bounds
            }
        }
    }

    // Apply the changes to the texture
    outputTexture.Apply();

    // Debug log to verify output texture generation
    // Debug.Log($"Output texture generated: {outputTexture.width}x{outputTexture.height}");

    // Display the output image
    outputImage.texture = outputTexture;
}

Texture2D DuplicateTexture(Texture2D source)
{
    RenderTexture renderTex = RenderTexture.GetTemporary(
        source.width,
        source.height,
        0,
        RenderTextureFormat.Default,
        RenderTextureReadWrite.Linear);

    Graphics.Blit(source, renderTex);
    RenderTexture previous = RenderTexture.active;
    RenderTexture.active = renderTex;
    Texture2D readableText = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
    readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
    readableText.Apply();
    RenderTexture.active = previous;
    RenderTexture.ReleaseTemporary(renderTex);
    return readableText;
}

    // Calculate the perspective transform matrix
    Matrix4x4 GetPerspectiveTransform(Vector2[] src, Vector2[] dst)
    {
        // The matrix should be set up based on solving linear equations.
        float[,] a = new float[8, 8];
        float[] b = new float[8];
        for (int i = 0; i < 4; i++)
        {
            a[i * 2, 0] = src[i].x;
            a[i * 2, 1] = src[i].y;
            a[i * 2, 2] = 1;
            a[i * 2, 3] = 0;
            a[i * 2, 4] = 0;
            a[i * 2, 5] = 0;
            a[i * 2, 6] = -dst[i].x * src[i].x;
            a[i * 2, 7] = -dst[i].x * src[i].y;
            b[i * 2] = dst[i].x;
            a[i * 2 + 1, 0] = 0;
            a[i * 2 + 1, 1] = 0;
            a[i * 2 + 1, 2] = 0;
            a[i * 2 + 1, 3] = src[i].x;
            a[i * 2 + 1, 4] = src[i].y;
            a[i * 2 + 1, 5] = 1;
            a[i * 2 + 1, 6] = -dst[i].y * src[i].x;
            a[i * 2 + 1, 7] = -dst[i].y * src[i].y;
            b[i * 2 + 1] = dst[i].y;
        }
        // Solve the linear system
        float[] h = GaussianElimination(a, b);
        // Create a transformation matrix
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetRow(0, new Vector4(h[0], h[1], h[2], 0));
        matrix.SetRow(1, new Vector4(h[3], h[4], h[5], 0));
        matrix.SetRow(2, new Vector4(h[6], h[7], 1, 0));
        matrix.SetRow(3, new Vector4(0, 0, 0, 1));
        return matrix;
    }
    // Apply the perspective transformation
    Vector2 ApplyPerspectiveTransform(Matrix4x4 matrix, Vector2 point)
{
    Vector4 transformed = matrix.MultiplyPoint3x4(new Vector3(point.x, point.y, 1));
    // Avoid division by zero
    if (transformed.z != 0)
    {
        return new Vector2(transformed.x / transformed.z, transformed.y / transformed.z);
    }
    return new Vector2(transformed.x, transformed.y);
}
    // Gaussian elimination to solve linear equations
    float[] GaussianElimination(float[,] a, float[] b)
    {
        int n = b.Length;
        for (int i = 0; i < n; i++)
        {
            // Pivot
            int maxRow = i;
            for (int j = i + 1; j < n; j++)
            {
                if (Mathf.Abs(a[j, i]) > Mathf.Abs(a[maxRow, i]))
                {
                    maxRow = j;
                }
            }
            // Swap rows
            for (int k = i; k < n; k++)
            {
                float temp = a[maxRow, k];
                a[maxRow, k] = a[i, k];
                a[i, k] = temp;
            }
            float tempB = b[maxRow];
            b[maxRow] = b[i];
            b[i] = tempB;
            // Make all rows below this one 0 in current column
            for (int j = i + 1; j < n; j++)
            {
                float factor = a[j, i] / a[i, i];
                for (int k = i; k < n; k++)
                {
                    a[j, k] -= factor * a[i, k];
                }
                b[j] -= factor * b[i];
            }
        }
        // Solve equation Ax=b for an upper triangular matrix A
        float[] x = new float[n];
        for (int i = n - 1; i >= 0; i--)
        {
            float sum = 0;
            for (int j = i + 1; j < n; j++)
            {
                sum += a[i, j] * x[j];
            }
            x[i] = (b[i] - sum) / a[i, i];
        }
        return x;
    }

    public static Matrix4x4 ComputeHomography(Vector2[] src, Vector2[] dst)
    {
        float[,] mat = new float[8, 9]; // 8 equations and 9 terms for h0 to h8

        // Construct the matrix of equations
        for (int i = 0; i < 4; i++)
        {
            float x = src[i].x;
            float y = src[i].y;
            float xp = dst[i].x;
            float yp = dst[i].y;

            mat[2 * i, 0] = x;
            mat[2 * i, 1] = y;
            mat[2 * i, 2] = 1;
            mat[2 * i, 3] = 0;
            mat[2 * i, 4] = 0;
            mat[2 * i, 5] = 0;
            mat[2 * i, 6] = -xp * x;
            mat[2 * i, 7] = -xp * y;
            mat[2 * i, 8] = xp;

            mat[2 * i + 1, 0] = 0;
            mat[2 * i + 1, 1] = 0;
            mat[2 * i + 1, 2] = 0;
            mat[2 * i + 1, 3] = x;
            mat[2 * i + 1, 4] = y;
            mat[2 * i + 1, 5] = 1;
            mat[2 * i + 1, 6] = -yp * x;
            mat[2 * i + 1, 7] = -yp * y;
            mat[2 * i + 1, 8] = yp;
        }

        // Solve the system of equations using Gaussian elimination
        float[] h = SolveHomography(mat);

        // Construct the homography matrix from the solution
        Matrix4x4 H = new Matrix4x4();
        H.SetRow(0, new Vector4(h[0], h[1], h[2], 0));
        H.SetRow(1, new Vector4(h[3], h[4], h[5], 0));
        H.SetRow(2, new Vector4(h[6], h[7], 1, 0));
        H.SetRow(3, new Vector4(0, 0, 0, 1));

        return H;
    }

    private static float[] SolveHomography(float[,] mat)
    {
        int rows = 8;
        int cols = 9;

        // Perform Gaussian elimination
        for (int i = 0; i < rows; i++)
        {
            // Find the pivot row
            int maxRow = i;
            for (int k = i + 1; k < rows; k++)
            {
                if (Mathf.Abs(mat[k, i]) > Mathf.Abs(mat[maxRow, i]))
                {
                    maxRow = k;
                }
            }

            // Swap the current row with the pivot row
            for (int k = i; k < cols; k++)
            {
                float temp = mat[i, k];
                mat[i, k] = mat[maxRow, k];
                mat[maxRow, k] = temp;
            }

            // Normalize the pivot row
            float pivot = mat[i, i];
            if (Mathf.Abs(pivot) < Mathf.Epsilon)
            {
                Debug.LogError("Matrix is singular and cannot be solved.");
                return null;
            }

            for (int k = i; k < cols; k++)
            {
                mat[i, k] /= pivot;
            }

            // Eliminate the current column in the other rows
            for (int k = 0; k < rows; k++)
            {
                if (k != i)
                {
                    float factor = mat[k, i];
                    for (int j = i; j < cols; j++)
                    {
                        mat[k, j] -= factor * mat[i, j];
                    }
                }
            }
        }

        // Extract the solution
        float[] h = new float[8];
        for (int i = 0; i < 8; i++)
        {
            h[i] = mat[i, 8];
        }

        return h;
    }
}






