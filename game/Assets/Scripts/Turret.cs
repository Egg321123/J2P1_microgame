using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [SerializeField] private float range = 5f;
    [SerializeField] private LayerMask enemyMask;
    private Transform target;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindTarget();
        }
        else
        {

            if (CheckIfTargetInRange())
            {
                target = null;
            }
            AimTowardsTarget();
        }
        
    }
    
    private void FindTarget()
    {
        if(Physics.SphereCast(transform.position, range, transform.forward, out hit , enemyMask))
        {
            target = hit.transform;
        }
    }
    private bool CheckIfTargetInRange()
    {
        return Vector3.Distance(target.position, transform.position) <= range;
    }
    private void AimTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - target.position.y, target.position.x - target.position.x) * Mathf.Rad2Deg;
        Quaternion shootingAngle = Quaternion.Euler(new Vector3(0, 0, angle));
        Shoot();
    }
    private void Shoot()
    {

    }
}
