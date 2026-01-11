using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    [Tooltip("プレイヤーの速度")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    // === 掴み・投げ用の追加フィールド ===
    [Header("Pickup Settings")]
    [Tooltip("掴めるオブジェクトを検出する距離")]
    [SerializeField] private float grabDistance = 2f;

    [Tooltip("掴めるオブジェクトの LayerMask")] 
    [SerializeField] private LayerMask grabbableLayers;

    [Tooltip("掴んだオブジェクトを保持する位置（プレイヤーの子オブジェクトなどを指定）")]
    [SerializeField] private Transform holdPoint;

    [Tooltip("投げる時に与える力の大きさ")]
    [SerializeField] private float throwForce = 8f;

    private Rigidbody heldObjectRb;           // 掴んでいるオブジェクトの Rigidbody
    private FixedJoint holdJoint;             // プレイヤー側に付ける Joint

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;//キャラが勝手に倒れたりするのを防ぐ

        // holdPoint が未設定なら、プレイヤーの正面少し先を自動で作る
        if (holdPoint == null)
        {
            GameObject go = new GameObject("HoldPoint");
            holdPoint = go.transform;
            holdPoint.SetParent(transform);
            holdPoint.localPosition = new Vector3(0, 1.0f, 0.8f);
        }
    }

    // --- 移動入力（WASD / スティック） ---
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>(); // ← 入力が0なら自動で Vector2.zero が入る
    }

    // === 掴む／離すボタン（Input System で "Grab" Action を作って紐付け） ===
    private void OnGrab(InputValue value)
    {
        // ボタンを押した瞬間だけ処理したい場合
        if (!value.isPressed) return;

        if (heldObjectRb == null)
        {
            TryGrab();
        }
        else
        {
            Release(false); // 投げずに離す
        }
    }

    // === 投げるボタン（"Throw" Action を作って紐付け） ===
    private void OnThrow(InputValue value)
    {
        if (!value.isPressed) return;

        if (heldObjectRb != null)
        {
            Release(true); // 投げて離す
        }
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

    // === 掴み処理 ===
    private void TryGrab()
    {
        // プレイヤーの胸あたりから前方へ Ray を飛ばす
        Vector3 origin = transform.position + Vector3.up * 1.0f;
        Vector3 dir = transform.forward;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, grabDistance, grabbableLayers))
        {
            Rigidbody targetRb = hit.rigidbody;
            if (targetRb == null) return;

            heldObjectRb = targetRb;
            heldObjectRb.useGravity = false;
            heldObjectRb.angularVelocity = Vector3.zero;
            heldObjectRb.linearVelocity = Vector3.zero;

            // Joint をプレイヤー側に付けて、オブジェクトを保持
            holdJoint = gameObject.AddComponent<FixedJoint>();
            holdJoint.connectedBody = heldObjectRb;
            holdJoint.autoConfigureConnectedAnchor = false;
            holdJoint.connectedAnchor = Vector3.zero;

            // オブジェクトを holdPoint の位置に移動
            heldObjectRb.MovePosition(holdPoint.position);
        }
    }

    // === 離す／投げる処理 ===
    private void Release(bool throwObject)
    {
        if (heldObjectRb == null) return;

        // Joint を削除
        if (holdJoint != null)
        {
            Destroy(holdJoint);
            holdJoint = null;
        }

        // 物理挙動を戻す
        heldObjectRb.useGravity = true;

        if (throwObject)
        {
            // プレイヤーの前方に向けて力を加える
            heldObjectRb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        }

        heldObjectRb = null;
    }
}
