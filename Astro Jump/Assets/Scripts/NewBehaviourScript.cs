using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Animator animator;
    private bool isMoving;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        float moveX = Input.GetAxis("Horizontal");

        if (Mathf.Abs(moveX) > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        animator.SetBool("isMoving", isMoving);
    }
}
