using UnityEngine;
using HomographySharp;
using System.Collections.Generic;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;

public class HomographyExample : MonoBehaviour
{
    void Start()
    {
        // Define 4 points in source (2D image) space
        List<Point2<float>> sourcePoints = new List<Point2<float>>
        {
            new Point2<float>(0, 0),
            new Point2<float>(1, 0),
            new Point2<float>(1, 1),
            new Point2<float>(0, 1)
        };

        // Define 4 corresponding points in destination (transformed) space
        List<Point2<float>> destinationPoints = new List<Point2<float>>
        {
            new Point2<float>(10, 10),
            new Point2<float>(20, 10),
            new Point2<float>(20, 20),
            new Point2<float>(10, 20)
        };

        // Compute the homography matrix
        HomographyMatrix<float> homographyMatrix = Homography.Find(sourcePoints, destinationPoints);

        Matrix<float> homoMat = homographyMatrix.ToMathNetMatrix();

        // Log the homography matrix
        // Matrix3x3 matrix = homographyMatrix.Matrix;
        Debug.Log($"Homography Matrix: \n{homoMat.ToString()}");

        // Use the matrix to transform a new point
        //Point2<float> pointToTransform = new Point2<float>(0.5, 0.5);
        Point2<float> transformedPoint = homographyMatrix.Translate(0.5f, 0.5f);

        Debug.Log($"Transformed Point: ({transformedPoint.X}, {transformedPoint.Y})");
    }
}