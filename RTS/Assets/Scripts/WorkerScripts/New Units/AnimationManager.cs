using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public UnitMovement unitMovement;
    public Animator animator;
    void Update()
    {
        //if (!unitMovement.isAttackingBuilding)
        //{
        //    animator.SetBool("Fighting", false);
        //    animator.SetBool("Idle", false);
        //    animator.SetBool("Walking",true);
        //}
        //if (unitMovement.isAttackingBuilding)
        //{
        //    animator.SetBool("Walking", false);
        //    animator.SetBool("Idle", false);
        //    animator.SetBool("Fighting", true);
        //}
    }
}
