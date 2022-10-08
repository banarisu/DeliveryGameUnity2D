using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private bool isFacingRight;

    [SerializeField]
    public float groundCheckerRadius;

    [SerializeField]
    private Transform groundChecker;

    [SerializeField]
    private LayerMask whatIsGround;
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);
        isNoGround();
    }

    private void isNoGround() //check if no ground and flip enemy
    {
        if (!ThereIsGround())
        {
            if (isFacingRight)
            {
                transform.eulerAngles = Vector2.up * 180;
                isFacingRight = false;

            }
            else
            {
                transform.eulerAngles = Vector2.zero;
                isFacingRight = true;
            }
        }
    }

    private bool ThereIsGround() //check if there is ground
    {
        return Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, whatIsGround);
    }
}
