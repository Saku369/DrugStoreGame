using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;

    [Header("Rotate (script only)")]
    public bool rotateToMoveDirection = true;
    public float rotateSpeed = 12f;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = false; // Dynamic
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // ★物理の回転を全部止める（壁ヒットで勝手に回らない）
        //   ただし見た目の向きは transform.rotation をスクリプトで変える
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationY
                       | RigidbodyConstraints.FreezeRotationZ
                       | RigidbodyConstraints.FreezePositionY;
    }

    // Input System: Move (Vector2)
    public void OnMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        moveInput = new Vector3(v.x, 0f, v.y);
    }

    void FixedUpdate()
    {
        // 速度
        Vector3 v = moveInput.sqrMagnitude > 0.0001f
            ? moveInput.normalized * moveSpeed
            : Vector3.zero;

        // ★壁に当たると止まる：物理に任せる（水平だけ更新）
        // Unityのバージョンによっては linearVelocity が無いので velocity を使う
        rb.linearVelocity = new Vector3(v.x, 0f, v.z);

        // ★向きはスクリプトだけで変える（物理の反作用で回らない）
        if (rotateToMoveDirection && v.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(v, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.fixedDeltaTime
            );
        }
    }
}
