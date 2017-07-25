using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class LoadData {
	/// <summary>
	/// Load a CSV of float data from a relative file path.
	/// <returns>The data as a grid of non-normalised floats.</returns>
	/// <param name="filePath">File path.</param>
	/// <param name="skipColumns">Controls whether or not included column titles should be skipped.</param>> 
	public static float[][] loadCSV (string filePath, bool skipColumns) {
		string line = "";
		List<List<float>> grid = new List<List<float>> ();
		try {
				
			using (StreamReader reader = new StreamReader ((Application.dataPath) + filePath)) {
				int count = 0;
				if (skipColumns) {
					line = reader.ReadLine ();
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
			for (int i = 0; i < grid.Count; i++) {
				outputList[i] = grid [i].ToArray();
			}
			return outputList;
		} catch (IOException e) {
			Debug.Log (e.Message);
		} catch (UnauthorizedAccessException e) {
			Debug.Log (e.Message);
		}
		return null;
	}

	/// <summary>
	/// Normalises a grid of floats into values in the range 0f - 1f.
	/// </summary>
	/// <returns>The values.</returns>
	/// <param name="input">Input.</param>
	public static float[][] normaliseValues(float[][] input) {
		if (input == null)
			return input;
		float max = float.MinValue;
		float min = float.MaxValue;

		// Retrieve min and max to do normalising process
		for (int i = 0; i < input.Length; i++) {
			for (int j = 0; j < input [i].Length; j++) {
				if (input [i] [j] > max) {
					max = input [i] [j];
				} else if (input [i] [j] < min) {
					min = input [i] [j];
				}
			}
		}

		// Perform normalisation
		for (int i = 0; i < input.Length; i++) {
			for (int j = 0; j < input [i].Length; j++) {
				input [i] [j] = (input [i] [j] - min) / (max - min);
			}
		}
		return input;
	}
}
