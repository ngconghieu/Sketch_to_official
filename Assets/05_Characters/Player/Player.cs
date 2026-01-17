using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField][AutoAssign] protected Rigidbody2D rb;
    [SerializeField][AutoAssign] protected BoxCollider2D col;
    [SerializeField][AutoAssign] protected Animator anim;

    [SerializeField] PlayerContext ctx;

    private IInputReader input;
    private bool isDashing;

    private void Awake()
    {
        ctx = new();

        rb.gravityScale = 8;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        col.offset = new Vector2(0, 1.03f);
        col.size = new Vector2(.6f, 2.07f);
    }

    private void Start()
    {
        input = ServicesLocator.Get<IInputReader>();
    }

    private void Update()
    {
        ctx.isGrounded = CheckGround();
        anim.SetBool(Const.Ground, ctx.isGrounded);
        anim.SetFloat(Const.VelocityY, rb.linearVelocity.y);

        if (input == null || isDashing) return;

        // Move
        var moveInput = input.Move.x;
        ctx.Velocity = ctx.Speed * moveInput;
        Flip(moveInput);
        anim.SetFloat(Const.VelocityX, Mathf.Abs(ctx.Velocity));

        // Dash
        if (input.Dash.IsValid)
        {
            input.Dash.Consume();
            StartCoroutine(DoDash());
        }

        // Jump
        if (input.Jump.IsValid)
        {
            ctx.CanJump = true;
            input.Jump.Consume();
        }
    }

    private void Flip(float moveInput)
    {
        if (moveInput == 0) return;
        transform.localScale = new(moveInput > 0 ? 1 : -1, 1, 1);
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        // Move
        rb.linearVelocityX = ctx.Velocity;

        // Jump
        if (ctx.CanJump)
        {
            ctx.CanJump = false;
            rb.linearVelocityY = 0; // reset Y velocity before jumping
            rb.AddForceY(ctx.JumpForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator DoDash()
    {
        isDashing = true;

        anim.SetTrigger(Const.Dash);
        float gravityBackup = rb.gravityScale;
        rb.gravityScale = 0;
        float dashDirection = transform.localScale.x;
        rb.linearVelocity = new Vector2(dashDirection * ctx.DashForce, 0);

        yield return new WaitForSeconds(ctx.DashDuration);

        rb.gravityScale = gravityBackup;
        isDashing = false;
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask(Const.Ground));
        if (hit.collider == null) return false; 
        Debug.Log(hit.collider.gameObject.name);
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.1f);
    }
}



[System.Serializable]
public class PlayerContext
{
    // Stats
    public float Speed = 10f;
    public float Velocity = 0;
    public float JumpForce = 30f;
    public float DashForce = 50f;
    public float DashDuration = 0.2f;
    public bool isGrounded;

    // Permissions
    public bool CanMove;
    public bool CanJump;

}