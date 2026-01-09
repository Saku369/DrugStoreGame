using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    [Tooltip("プレイヤーの速度")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;//キャラが勝手に倒れたりするのを防ぐ
    }

    // --- 移動入力（WASD / スティック） ---
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>(); // ← 入力が0なら自動で Vector2.zero が入る
    }

    private void FixedUpdate()
    {
        // 入力を3D空間に変換
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);

        // 斜め移動も
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
        // 入力方向へキャラを向ける
        if (dir != Vector3.zero)
        {
            transform.forward = dir.normalized;
        }
       
        if (moveInput != Vector2.zero)
        {
            Debug.Log($"Move Input - X: {moveInput.x}, Y: {moveInput.y}");
        }
    }
}
