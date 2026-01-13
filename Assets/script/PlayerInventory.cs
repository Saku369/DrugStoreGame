using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Single Slot")]
    public Ingredient held; // ★いま持ってる1個

    // 表示/デバッグ用（進捗は基本 Order.progressIndex を正とする）
    public int currentProcessIndex = 0;

    public bool HasItem => held != null;

    public bool TryHold(Ingredient ingredient)
    {
        if (ingredient == null) return false;

        if (held != null)
        {
            Debug.Log("[Inventory] すでに持っています：" + held.ingredientName);
            return false;
        }

        held = ingredient;
        Debug.Log("[Inventory] 取得：" + held.ingredientName);
        return true;
    }

    public Ingredient TakeHeld()
    {
        if (held == null) return null;

        var item = held;
        held = null;
        Debug.Log("[Inventory] 手放した：" + item.ingredientName);
        return item;
    }

    public void Clear()
    {
        held = null;
    }

    public void ResetProcess()
    {
        currentProcessIndex = 0;
    }

    public void DebugPrintContents()
    {
        if (held == null)
        {
            Debug.Log("[Inventory] 空");
        }
        else
        {
            Debug.Log("[Inventory] " + held.ingredientName + " x1");
        }
    }
}
