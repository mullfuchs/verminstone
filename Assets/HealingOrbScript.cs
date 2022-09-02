using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrbScript : MonoBehaviour
{
    public float Acceleration;
    public float ProjectileSpeed = 0.0f;
    public GameObject target;
    private Rigidbody orbRigidbody;
    private Quaternion guideRotation;
    // Start is called before the first frame update
    void Start()
    {
        orbRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GuideOrb();
        ProjectileSpeed += Acceleration * Time.deltaTime;
        transform.Translate(Vector3.forward * ProjectileSpeed * Time.deltaTime);

        gameObject.transform.rotation = guideRotation;
        if(Vector3.Distance(target.transform.position, gameObject.transform.position) < 0.5f)
        {
            Invoke("DestroyOrb", 0.1f);
        }
    }

    private void GuideOrb()
    {
        if(target == null)
        {
            return;
        }
        else
        {
            Vector3 relPosition = target.transform.position - gameObject.transform.position;
            guideRotation = Quaternion.LookRotation(relPosition, transform.up);
        }
    }

    private void DestroyOrb()
    {
        Destroy(gameObject);
    }
}
