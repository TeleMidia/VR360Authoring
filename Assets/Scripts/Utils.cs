using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Utils
{
    public static Vector3 PolarToCartesian(Vector3 origin, float theta, float phi)
    {
        var rotation = Quaternion.Euler(theta, phi, 0);
        Vector3 point = rotation * origin;
        return point;
    }
}