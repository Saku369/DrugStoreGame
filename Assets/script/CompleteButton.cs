using UnityEngine;

public class CompleteButton : MonoBehaviour
{
    [Header("Order Queue (SO)")]
    public OrderQueueSO orderQueue;

    [Header("Optional: clear inventory on complete")]
    public PlayerInventory playerInventory;

    [Header("Optional: add score")]
    public ScoreManager scoreManager;
    public int scorePerOrder = 10;

    // 「一番上の注文を完成（納品）させる」
    public void CompleteFirstOrder()
    {
        if (orderQueue == null) return;
        if (orderQueue.activeOrders == null || orderQueue.activeOrders.Count == 0) return;

        Order order = orderQueue.activeOrders[0];
        if (order == null) return;

        int stepCount = (order.template != null && order.template.steps != null) ? order.template.steps.Count : 0;

        if (stepCount == 0)
        {
            Debug.LogWarning("注文の template/steps が未設定です");
            return;
        }

        // まだ工程が終わっていない場合は納品不可
        if (order.progressIndex < stepCount)
        {
            Debug.Log($"まだ完成していません：({order.progressIndex}/{stepCount})");
            return;
        }

        // 納品（キューから削除）
        orderQueue.activeOrders.RemoveAt(0);
        Debug.Log($"納品完了：{order.orderName} / 残り注文数={orderQueue.activeOrders.Count}");

        // 後片付け（任意）
        if (playerInventory != null)
        {
            playerInventory.Clear();
            playerInventory.ResetProcess();
        }

        // スコア加算（任意）
        if (scoreManager != null)
        {
            scoreManager.score += scorePerOrder;
        }
    }
}
