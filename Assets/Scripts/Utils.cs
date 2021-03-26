using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// This class has static methods that can be used in situations related to different contexts.
/// </summary>
public class Utils
{
    /// <summary>
    /// This method converts a polar coordinate to carthesian.
    /// </summary>
    /// <param name="origin">Vector origin</param>
    /// <param name="theta">Vertical angle in degrees from origin</param>
    /// <param name="phi">Horizontal angle in degrees from origin</param>
    /// <returns>Point in cartesian coordinate system</returns>
    public static Vector3 PolarToCartesian(Vector3 origin, float theta, float phi)
    {
        var rotation = Quaternion.Euler(theta, phi, 0);
        Vector3 point = rotation * origin;
        return point;
    }
    /// <summary>
    /// This method reads a csv file with the lat long positions for an object
    /// </summary>
    /// <param name="file_path">The path of the csv file</param>
    /// <param name="fps_used">The fps used to extract frames</param>
    /// <returns>A dictionary with the timed positions</returns>
    public Dictionary<double, double[]> ReadTimedPositions(string file_path, out float fps_used)
    {
        Dictionary<double, double[]> timed_positions = new Dictionary<double, double[]>();

        using (var reader = new StreamReader(file_path))
        {
            //First Line: fps used in the frames extraction
            fps_used = float.Parse(reader.ReadLine().Split(':')[1]);
            //Console.WriteLine("FPS used for extraction: "+fps_used);
            string headers = reader.ReadLine();
            //Console.WriteLine("Headers: "+headers);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Length > 0)
                {
                    string[] values = line.Split(',');

                    double time = double.Parse(values[0], CultureInfo.InvariantCulture.NumberFormat);
                    double lat = double.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat);
                    double lon = double.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat);

                    timed_positions.Add(time, new double[] { lat, lon });
                }
            }
        }

        return timed_positions;
    }
}