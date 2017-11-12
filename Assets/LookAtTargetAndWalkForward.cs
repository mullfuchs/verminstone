using UnityEngine;
using System.Collections;

public class LookAtTargetAndWalkForward : MonoBehaviour {
	public float walkSpeed;
	public GameObject target;

    private GameObject AttackHitBox;
    private bool CanAttack = true;
    // Update is called once per frame
    void Start (){
        AttackHitBox = transform.Find("AttackHitBox").gameObject;
        AttackHitBox.SetActive(false);

        if (Random.Range (0, 2) <= 0 && target == null) {
			target = GameObject.FindGameObjectWithTag ("Player");
		} 
		else {
			target = GameObject.FindGameObjectWithTag ("WorkerNPC");
		}

		this.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target = target.transform;
	}

	void Update () {
        if(target != null)
        {
            float distToEnemy = Vector3.Distance(target.transform.position, transform.position);
            if (distToEnemy <= 1.5f && CanAttack)
            {
                PerformAttack();
            }
        }
	}

    public void setTarget(GameObject newTarget)
    {
        this.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = newTarget.transform;
    }

    public void PerformAttack()
    {
        AttackHitBox.SetActive(true);
        CanAttack = false;
        Invoke("HideHitBox", 0.5f);
        Invoke("ResetAttack", 1.5f);
    }

    public void HideHitBox()
    {
        AttackHitBox.SetActive(false);
    }

    public void ResetAttack()
    {
        CanAttack = true;
    }

}
