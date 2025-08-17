using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public float moveSpeed = 5f;

    public bool isGrounded = true;

    public float jumpForce = 2.0f;

    public float groundDistanceCheck = 0.1f;

    public LayerMask groundMask;

    private void Awake()
    {
        groundMask = LayerMask.GetMask("Ground");
    }


    [SerializeField]
    private Rigidbody rigidBody;


    private void Update()
    {
        // Only process input for the local player
        if (!IsOwner) return;

        Vector3 input = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical")
        );

        // if the space key is pressed the player is grounded
        if(Input.GetKeyDown(KeyCode.Space)&& IsGrounded())
        {
            Debug.Log("CLIENT:I want to jump");
            Jump();
            JumpServerRpc();   
        }


        Vector3 move = input * moveSpeed * Time.deltaTime;

        // Send the movement to the server
         MoveServerRpc(move);
    }
    // server function that calls the jump function to perform logic
    [ServerRpc]
    private void MoveServerRpc(Vector3 move, ServerRpcParams rpcParams = default)
    {
        // Apply movement on the server
        transform.position += move;
    }
    [ServerRpc]
    private void JumpServerRpc()
    {
        Debug.Log("SERVER:The player wants to jump");
        // Actually jump
        Jump();
    }
    // apply velocity to the rigidbody to jump (works on either server or client)
    private void Jump()
    {
        var vel = rigidBody.linearVelocity;
        vel.y = 0f;
        vel.y += jumpForce;
        rigidBody.linearVelocity = vel;
    }
    bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.05f;
        float radius = 0.25f;
        RaycastHit hit;
        // perform spherecast from the origin and the radius provided only return result
        // that it hits the ground layermask
        return Physics.SphereCast(origin, radius, Vector3.down, out hit, groundDistanceCheck, groundMask, QueryTriggerInteraction.Ignore);
    }

}
