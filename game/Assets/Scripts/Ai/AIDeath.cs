using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeath : MonoBehaviour
{
    [SerializeField] FollowPath followPath;

    public void Die()
    {
        followPath.IsAllowedToMove = false;
        gameObject.SetActive(false);
    }
}
