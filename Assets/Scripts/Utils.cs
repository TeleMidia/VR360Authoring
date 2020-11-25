using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes
/// This class has static methods that can be used in situations related to different contexts.
/// </summary>
public class Utils
{
    /// <summary>
    /// This method converts a polar coordinate to carthesian
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
}