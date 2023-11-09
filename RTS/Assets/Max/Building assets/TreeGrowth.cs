using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowth : MonoBehaviour
{
    public Animator animator;


    void Update()
    {
        // You can trigger the growth animation based on some condition, user input, or a timer.
        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.Play("Tree Growing");
        }
    }
}
