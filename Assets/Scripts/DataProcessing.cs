using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class DataProcessing
{
    /// <summary>
    /// Load a CSV of float data from a relative file path.
    /// <returns>The data as a grid of non-normalised floats.</returns>
    /// <param name="filePath">File path.</param>
    /// <param name="skipColumns">Controls whether or not included column titles should be skipped.</param>> 
    public static float[][] loadCSV(string filePath, bool skipColumns)
    {
        string line = "";
        List<List<float>> grid = new List<List<float>>();
        try
        {
            using (StreamReader reader = new StreamReader((Application.dataPath) + filePath))
            {
                int count = 0;
                if (skipColumns)
                {
                    line = reader.ReadLine();
                }
                while ((line = reader.ReadLine()) != null)
                {
                    string[] vals = line.Split(',');
                    List<float> row = new List<float>();
                    foreach (string s in vals)
                    {
                        row.Add(float.Parse(s));
                    }
                    grid.Add(row);
                    count++;
                }
            }
            float[][] outputList = new float[grid.Count][];
            for (int i = 0; i < grid.Count; i++)
            {
                outputList[i] = grid[i].ToArray();
            }
            return outputList;
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            Debug.Log(e.Message);
        }
        return null;
    }

    /// <summary>
    /// Normalises a grid of floats into values in the range 0f - 1f.
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="input">Input.</param>
    public static float[][] normaliseValues(float[][] input)
    {
        if (input == null)
            return input;
        float max = float.MinValue;
        float min = float.MaxValue;

        // Retrieve min and max to do normalising process
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] > max)
                {
                    max = input[i][j];
                }
                else if (input[i][j] < min)
                {
                    min = input[i][j];
                }
            }
        }

        // Perform normalisation
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                input[i][j] = (input[i][j] - min) / (max - min);
            }
        }
        return input;
    }

    /// <summary>
    /// Rescales a float[][] representing data using BiCubic interpolation.
    /// Algorithm modified from 
    /// https://stackoverflow.com/questions/32089277/correctly-executing-bicubic-resampling, accessed 2017-08-17.
    /// </summary>
    /// <param name="input">the input array.</param> 
    /// <param name="xScaleFactor">factor to scale with in the x axis (width, first dimension)</param> 
    /// <param name="zScaleFactor">factor to scale with in the z axis (height, second dimension)</param> 
    /// <returns>the scaled output array, with interpolation applied.</returns>
    public static float[][] resize(float[][] input, float xScaleFactor, float zScaleFactor)
    {
        // Width and height decreased by 1
        int inputHeight = input[0].Length - 1;
        int inputWidth = input.Length - 1;

        int outputWidth = (int) (inputWidth * xScaleFactor);
        int outputHeight = (int) (inputHeight * zScaleFactor);

        float[][] output = new float[outputWidth][];

        for (int x = 0; x < outputWidth; x++)
        {
            output[x] = new float[outputHeight];
            // X coordinates
            float ox = (x / xScaleFactor) - 0.5f;
            int ox1 = (int) ox;
            float dx = ox - ox1;

            for (int z = 0; z < outputHeight; z++)
            {
                // Z coordinates
                float oz = (z / zScaleFactor) - 0.5f;
                int oz1 = (int) oz;
                float dz = oz - oz1;
                
                float val = 0;
                for (int n = -1; n < 3; n++)
                {
                    // Get Z coefficient
                    float k1 = BiCubicKernel(dz - n);
                    int oz2 = Mathf.Clamp(oz1 + n, 0, inputHeight);

                    for (int m = -1; m < 3; m++)
                    {
                        // Get X coefficient
                        float k2 = k1 * BiCubicKernel(m - dx);
                        int ox2 = Mathf.Clamp(ox1 + m, 0, inputWidth);
                        val += k2 * input[ox2][oz2];
                    }
                }
                output[x][z] = val;
            }
        }
        return output;
    }
    /// <summary>
    /// The function implements a bicubic kernel with an a value of -0.5.
    /// The algorithm was sourced from
    /// <a href="https://en.wikipedia.org/wiki/Bicubic_interpolation#Bicubic_convolution_algorithm">Wikipedia</a>, accessed 2017-08-17.
    /// </summary>
    public static float BiCubicKernel( float x )
    {
        float absX = Mathf.Abs(x);
        if (absX > 2)
        {
            return 0;
        }
        if (absX <= 1)
        {
            return 1.5f * Mathf.Pow(absX, 3) - 2.5f * Mathf.Pow(absX, 2) + 1;
        }
        return -0.5f * Mathf.Pow(absX, 3) + 2.5f * Mathf.Pow(absX, 2) - 4f * absX + 2; 
    }
}