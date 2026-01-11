using UnityEngine;

public class Customer : MonoBehaviour
{
    public Recipe order;
    public OrderQueue orderQueue;

    void Start()
    {
        if (order != null && orderQueue != null)
        {
            orderQueue.activeOrders.Add(order);
        }
    }
}