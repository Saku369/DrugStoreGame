using UnityEngine;

public class Customer : MonoBehaviour
{
    public Order order;

    public void Init(Order o)
    {
        order = o;
    }
}

