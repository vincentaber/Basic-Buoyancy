using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    //I'm not the best at writing code, so feel free to make this a better script.

    public Transform[] floaters; //Set a series of empty gameobjects, children of the floating one, that will work as buoys : for a square put one at each corner, for a simple object, you can just use the transform of the object (if it is at the center of mass)
    public Transform Sea; // Only works for a flat sea, but easily adaptable to a wavy ocean

    [SerializeField] float underWaterDrag = 1; //Play with these values
    [SerializeField] float underWaterAngularDrag = 0.5f; //Play with these values

    float airDrag;
    float airAngularDrag;

    Rigidbody m_RigidBody;

    int floatersUnderwater;
    float seaY; 

    bool underwater;

    [SerializeField] float floatingPower = 1; //Play with these values
    [SerializeField] float gizmoRadius = 0.1f;

    void Start()
    {
        seaY = Sea.position.y; // This variable is only useful in the flat sea scenario
        m_RigidBody = GetComponent<Rigidbody>();
        airDrag = m_RigidBody.drag;
        airAngularDrag = m_RigidBody.angularDrag;
    }

    void FixedUpdate()
    {
        floatersUnderwater = 0;


        for (int i = 0; i < floaters.Length; i++)
        {
            float diff = floaters[i].position.y - seaY; // for a wavy sea, simply make a public function GetHeightAtLocation(Vector2 position) in your sea script, and use it to retrieve the height of the sea at each floater's position
            
            
            
            if (diff < 0)
            {
                m_RigidBody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(diff), floaters[i].position, ForceMode.Force);
                floatersUnderwater += 1;
                if (!underwater)
                {
                    underwater = true;
                    SwitchState(true);
                }
            }
            if (underwater && floatersUnderwater == 0)
            {
                underwater = false;
                SwitchState(false);
            }
        }
    }

    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            m_RigidBody.drag = underWaterDrag;
            m_RigidBody.angularDrag = underWaterAngularDrag;
        }
        else
        {
            m_RigidBody.drag = airDrag;
            m_RigidBody.angularDrag = airAngularDrag;
        }
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < floaters.Length; i++)
        {
            Gizmos.color = Color.yellow;
            if (floaters[i].position.y < seaY)
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawSphere(floaters[i].position, gizmoRadius);

        }
    }
}
