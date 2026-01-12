using UnityEngine;

public class CompleteButton : MonoBehaviour
{
    public OrderQueue orderQueue;

    // 今回は「一番上の注文を完成させる」
    public void CompleteFirstOrder()
    {
        if (orderQueue.activeOrders.Count == 0) return;

        Recipe completed = orderQueue.activeOrders[0];
        orderQueue.CompleteOrder(completed);
    }
}