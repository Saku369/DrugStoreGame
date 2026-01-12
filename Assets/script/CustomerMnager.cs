using UnityEngine;

public class Customer : MonoBehaviour
{
    public Recipe order;
    public OrderQueue orderQueue;

    public void Init(Recipe recipe, OrderQueue queue)
    {
        order = recipe;
        orderQueue = queue;

        if (orderQueue != null && order != null)
        {
            orderQueue.activeOrders.Add(order);
        }
    }
}
