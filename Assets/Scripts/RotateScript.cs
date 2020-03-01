using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(target.transform.position, Vector3.up, Input.GetAxis("Mouse X") * speed * Time.deltaTime);
            //transform.RotateAround(target.transform.position, Vector3.left, Input.GetAxis("Mouse Y") * speed * Time.deltaTime);
        }
        if (Input.GetMouseButton(1))
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
