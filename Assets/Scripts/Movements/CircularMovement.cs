using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CircularMovement: Movement
{
    public CircularMovement(float r=0, float theta=0, float phi=0, float duration=1) : base(r:r, theta:theta, phi:phi, duration:duration) { }

    public override Vector3 Move(float start_r, float start_theta, float start_phi)
    {
        if (this.start_time < 0)
        {
            this.start_time = Time.time;
        }
        float frac = (Time.time - this.start_time) / duration;
        frac = Mathf.Min(frac, 1);

        return Utils.PolarToCartesian(origin:new Vector3(0,0,start_r+delta_r*frac), theta:start_theta + delta_theta * frac, phi:start_phi + delta_phi * frac);
    }
}
