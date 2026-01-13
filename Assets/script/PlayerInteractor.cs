using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Refs")]
    private PlayerInventory inventory;
    public OrderQueueSO orderQueue;

    [Header("Ray Settings")]
    public Transform rayOrigin;          // ここからRayを出す（未設定ならtransform）
    public float interactDistance = 1.2f;
    public LayerMask interactMask;       // 棚/作業台のレイヤーを入れる（強く推奨）

    private void Start()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    // Input System: Action名 Interact 前提（PlayerInput Send Messages）
    public void OnInteract()
    {
        if (inventory == null) return;

        Transform origin = rayOrigin != null ? rayOrigin : transform;
        Vector3 from = origin.position + Vector3.up * 0.5f; // 少し浮かせる
        Vector3 dir = origin.forward;

        if (!Physics.Raycast(from, dir, out RaycastHit hit, interactDistance, interactMask))
        {
            // 何にも当たってない → インベントリ確認
            inventory.DebugPrintContents();
            return;
        }

        // 棚
        var shelf = hit.collider.GetComponentInParent<Shelf>();
        if (shelf != null)
        {
            var ing = shelf.Take();
            inventory.TryHold(ing);
            return;
        }

        // 作業台
        var bench = hit.collider.GetComponentInParent<Workbench>();
        if (bench != null)
        {
            if (orderQueue == null || orderQueue.activeOrders.Count == 0)
            {
                Debug.Log("注文がありません");
                return;
            }

            var order = orderQueue.activeOrders[0];
            bench.Use(inventory, order);
            return;
        }

        // 何かには当たったが対象じゃない
        inventory.DebugPrintContents();
    }

    // デバッグ：SceneビューでRayを見えるように
    private void OnDrawGizmosSelected()
    {
        Transform origin = rayOrigin != null ? rayOrigin : transform;
        Vector3 from = origin.position + Vector3.up * 0.5f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(from, from + origin.forward * interactDistance);
    }
}
