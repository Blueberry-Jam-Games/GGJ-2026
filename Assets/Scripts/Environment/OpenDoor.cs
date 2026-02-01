using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public List<Rigidbody> doors;
    public List<bool> invert;

    public bool open = false;

    void OnTriggerEnter(Collider other)
    {
        open = true;
    }

    void OnTriggerExit(Collider other)
    {
        open = false;   
    }

    void FixedUpdate()
    {
        if(open)
        {
            int i = 0;
            foreach(Rigidbody r in doors)
            {
                if(invert[i] == true)
                {
                    r.AddTorque(new Vector3(0, -18, 0));
                }
                else
                {
                    r.AddTorque(new Vector3(0, 18, 0));
                }
                i++;
            }
        }
    }
}
