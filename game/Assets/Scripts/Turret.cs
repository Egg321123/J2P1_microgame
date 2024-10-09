using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform firingPoint;
    [SerializeField] private LayerMask enemyMask;
    private Transform target;
    private float timeUntillShooting;
    [SerializeField] TrailRenderer bulletTrail;

    [SerializeField] private float range = 5f;
    [SerializeField] private float AtkSpeed = 5f;
    [SerializeField] private float damage = 5f;
    Quaternion shootingAngle;




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


            AimTowardsTarget();



            if (!CheckIfTargetInRange())
            {
                print("target set to null");
                target = null;
            }

            else
            {
                timeUntillShooting += Time.deltaTime;
                if (timeUntillShooting >= AtkSpeed)
                {
                    Shoot();
                    timeUntillShooting = 0;
                }
                
            }
            
        }
        
    }
    private void FindTarget()
    {
        Debug.Log("looking for target");
        
        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyMask);
        if (hits.Length > 0)
        {
            Debug.Log("target found");

            target = hits[0].transform;
        }
    }
    /// <summary>
    /// if enemy distance is less than or equal to the towers range
    /// </summary>
    /// <returns>returns a bool that will return true there are no enemies in range</returns>
    private bool CheckIfTargetInRange()
    {
        return Vector3.Distance(target.position, transform.position) < range;
    }
    private void AimTowardsTarget()
    {
        //unused
        float angle = Mathf.Atan2(target.position.y - target.position.y, target.position.x - target.position.x) * Mathf.Rad2Deg;
        shootingAngle = Quaternion.Euler(new Vector3(0, 0, angle));
        
    }
    private void Shoot()
    {
        Debug.Log("Shoot");
        Vector3 dir = (target.position - firingPoint.position).normalized;
        Debug.DrawRay(firingPoint.position, dir, Color.red, 1000f);
        if (Physics.Raycast(firingPoint.position, dir,out hit, enemyMask))
        {
            // enemy take damage equal to towers damage variable
            TrailRenderer trail = Instantiate(bulletTrail, firingPoint.position, quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
        }
        
    }
    private IEnumerator SpawnTrail(TrailRenderer _trail, RaycastHit _hit)
    {
        float time = 0;
        Vector3 startPosition = _trail.transform.position;

        while(time < 1)
        {
            _trail.transform.position = Vector3.Lerp(startPosition, _hit.point, time);
            time += Time.deltaTime / _trail.time;
            yield return null;
            
        }
        _trail.transform.position = hit.point;
        Destroy(_trail.gameObject, _trail.time);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
