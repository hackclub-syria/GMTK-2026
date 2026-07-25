using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public cursor_script cursorScript;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 checkSize = new Vector2(0.8f, 0.2f);
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        if (cursorScript == null || groundCheckPoint == null) return;
        bool isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, checkSize, 0f, groundLayer);

        cursorScript.SetGroundedState(isGrounded);
    }
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckPoint.position, checkSize);
        }
    }
}
