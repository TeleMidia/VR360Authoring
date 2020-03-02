using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Movement
{
    public float start_time;
    public Vector3 end_pos;
    public float duration;
    public float delta_r,delta_phi, delta_theta;

    public Movement(float r, float theta, float phi, float duration)
    {
        this.start_time = -1;
        this.delta_r = r;
        this.delta_theta = theta;
        this.delta_phi = phi;
        this.end_pos = Utils.PolarToCartesian(new Vector3(0,0,r), theta, phi) ;
        this.duration = duration;
    }
    public abstract Vector3 Move(float start_r, float start_theta, float start_phi);
}
