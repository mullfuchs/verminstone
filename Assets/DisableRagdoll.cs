using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRagdoll : MonoBehaviour
{
    public float secondsToDisable = 2.5f;

    Collider[] ragdollColliders;
    Rigidbody[] ragdollRigidBodies;

    // Start is called before the first frame update
    void Start()
    {
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidBodies = GetComponentsInChildren<Rigidbody>();
        Invoke("freezeRagdoll", secondsToDisable);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void freezeRagdoll()
    {
        foreach(Rigidbody rigidbody in ragdollRigidBodies)
        {
            rigidbody.isKinematic = true;
        }

        foreach(Collider collider in ragdollColliders)
        {
            collider.enabled = false;
        }
    }
}
