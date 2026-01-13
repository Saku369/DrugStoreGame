using UnityEngine;

public class HeldItemVisual : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float heldScale = 1.5f; // ★ここを調整


    private Ingredient lastHeld;
    private GameObject currentObj;

    void Awake()
    {
        if (inventory == null) inventory = GetComponent<PlayerInventory>();
    }

    void LateUpdate()
    {
        if (inventory == null || holdPoint == null) return;

        // held が変わったら更新
        if (inventory.held == lastHeld) return;

        lastHeld = inventory.held;
        Refresh();
    }

    void Refresh()
    {
        // いったん消す
        if (currentObj != null)
        {
            Destroy(currentObj);
            currentObj = null;
        }

        // 何も持ってない
        if (lastHeld == null) return;

        // Prefab 未設定
        if (lastHeld.worldPrefab == null)
        {
            Debug.LogWarning($"[{nameof(HeldItemVisual)}] {lastHeld.ingredientName} の worldPrefab が未設定です");
            return;
        }

        // 生成して手元へ
        currentObj = Instantiate(lastHeld.worldPrefab, holdPoint);
        currentObj.transform.localPosition = Vector3.zero;
        currentObj.transform.localRotation = Quaternion.identity;
        currentObj.transform.localScale = Vector3.one * heldScale;


        // 手持ちなので、当たり判定や物理が邪魔なら無効化
        foreach (var col in currentObj.GetComponentsInChildren<Collider>())
            col.enabled = false;

        var rb = currentObj.GetComponentInChildren<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }
}

