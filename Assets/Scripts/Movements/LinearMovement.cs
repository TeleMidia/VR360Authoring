using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LinearMovement: Movement
{
    public LinearMovement(float r=0, float theta=0, float phi=0, float duration=1) : base(r: r, theta: theta, phi: phi, duration: duration) { }
    public override Vector3 Move(float start_r, float start_theta,float start_phi)
    {
        Vector3 start_pos = Utils.PolarToCartesian(new Vector3(0,0, start_r), start_theta, start_phi);
        if(this.start_time < 0)
        {
            this.start_time = Time.time;
        }

        return Vector3.Lerp(start_pos, this.end_pos, (Time.time-this.start_time)/duration);
    }
}