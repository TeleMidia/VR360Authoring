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
    public static Dictionary<float, float[]> ReadTimedPositions(string file_path, out float pos_frequency)
    {
        Dictionary<float, float[]> timed_positions = new Dictionary<float, float[]>();
        pos_frequency = -1;

        if (file_path == "") return timed_positions;

        using (var reader = new StreamReader(file_path))
        {
            //First Line: fps used in the frames extraction
            pos_frequency = 1.0f/float.Parse(reader.ReadLine().Split(':')[1]);
            //Console.WriteLine("FPS used for extraction: "+fps_used);
            string headers = reader.ReadLine();
            //Console.WriteLine("Headers: "+headers);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Length > 0)
                {
                    string[] values = line.Split(',');

                    float time = float.Parse(values[0], CultureInfo.InvariantCulture.NumberFormat);
                    float lat = float.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat);
                    float lon = float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat);

                    if(!timed_positions.ContainsKey(time))
                        timed_positions.Add(time, new float[] { lat, lon });
                }
            }
        }

        return timed_positions;
    }
    /// <summary>
    /// This function converts a dictionary of lat long positions into vector 3 positions.
    /// </summary>
    /// <param name="origin">The origin vector</param>
    /// <param name="timedPositions">The dictionary of lat long positions</param>
    /// <returns>Dictionary of vector3 positions indexed by time</returns>
    public static Dictionary<float, Vector3> Vector3TimedPositions(Vector3 origin, Dictionary<float, float[]> timedPositions)
    {
        Dictionary<float, Vector3> vector3_timed_positions = new Dictionary<float, Vector3>();

        foreach(float key in timedPositions.Keys){
            float theta = timedPositions[key][0];
            float phi = timedPositions[key][1];

            vector3_timed_positions.Add(key, PolarToCartesian(origin:origin, theta:theta, phi:phi));
        }

        return vector3_timed_positions;
    }
}