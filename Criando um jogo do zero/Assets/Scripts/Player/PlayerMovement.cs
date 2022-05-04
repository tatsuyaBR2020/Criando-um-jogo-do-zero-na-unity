using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move Config")]
    [SerializeField] float speed;
    [SerializeField] float distance;

    [Header("Jump Config")]
    [SerializeField] float jumpForce;
    [SerializeField] float timeMaxInAir;
    [SerializeField] int maxJump;
    [SerializeField] LayerMask groundLayer;

    [Header("Imports")]
    [SerializeField] Rigidbody2D rb;

    private GlobalActions actions;
    private PlayerAnimations anim;
    private int jumpCount;
    private IEnumerator timerInAir;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<PlayerAnimations>();
        actions = new GlobalActions();
        actions.Enable();
        jumpCount = maxJump;
    }

    void Update()
    {
        Movement();
        CheckJump();
    }

    void Movement()
    {
        float hspd = speed * actions.Player.Movement.ReadValue<float>();
        rb.velocity = new Vector2(hspd, rb.velocity.y);
        anim.ExecuteAnimMove(hspd);

        Flip(hspd);
    }

    void Flip(float hspd)
    {
        if (hspd != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(hspd), 1, 0);
        }
    }

    void CheckJump()
    {
        if (actions.Player.Jump.triggered && jumpCount < maxJump)
        {
            Jump();
        }
    }

    void Jump()
    {
        anim.ExecuteAnimJump(true);
        if (rb.velocity.y <=0)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForce);
        
        jumpCount++;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 6)
        {
            if (other.contacts[0].normal == Vector2.up)
            {
                if (timerInAir != null){
                    StopCoroutine(timerInAir);
                    timerInAir = null;
                }
                anim.ExecuteAnimJump(false);
                jumpCount = 0;
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == groundLayer)
        {
            var ray = Physics2D.Raycast(transform.position, Vector2.down, distance, groundLayer);
            if (jumpCount == 0 && ray.collider == null)
            {
                this.timerInAir = TimeInAir();
                StartCoroutine(timerInAir);
            }
        }
    }

    IEnumerator TimeInAir()
    {
        yield return new WaitForSeconds(timeMaxInAir);
        jumpCount = (maxJump > 1) ? maxJump - 1 : maxJump;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -distance, 0));
    }
}
