using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderIconUI : MonoBehaviour
{
    [Header("Data")]
    public OrderQueueSO orderQueue;

    [Header("Current Order UI")]
    public TMP_Text currentTitle;
    public Image currentBaseIcon;
    public Transform currentStepsRow;
    public TMP_Text effectsText; // ★追加：箇条書き表示

    [Header("Remaining")]
    public TMP_Text remainingText;

    [Header("Prefabs")]
    public Image iconPrefab;

    [Header("Base Icons")]
    public Sprite syrupBaseIcon;
    public Sprite tabletBaseIcon;
    public Sprite powderBaseIcon;

    [Header("Step Icons")]
    public Sprite mixIcon;
    public Sprite heating_coolingIcon;
    public Sprite pressIcon;
    public Sprite weighIcon;
    public Sprite packingIcon;

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        if (orderQueue == null || iconPrefab == null) return;

        var orders = orderQueue.activeOrders;

        if (orders.Count == 0)
        {
            if (currentTitle != null) currentTitle.text = "注文なし";
            if (currentBaseIcon != null) currentBaseIcon.enabled = false;
            ClearChildren(currentStepsRow);
            if (effectsText != null) effectsText.text = "";
            if (remainingText != null) remainingText.text = "";
            return;
        }

        var order = orders[0];

        if (currentTitle != null) currentTitle.text = "現在の注文";

        // ベース（シロップ等）
        if (currentBaseIcon != null)
        {
            currentBaseIcon.sprite = GetBaseIcon(order.baseType);
            currentBaseIcon.enabled = currentBaseIcon.sprite != null;
        }

        // 工程アイコン
        var stepSprites = order.template != null
            ? order.template.steps.Select(s => GetStepIcon(s.workbenchType))
            : Enumerable.Empty<Sprite>();

        RebuildRow(currentStepsRow, stepSprites);

        // 効果（文字）
        if (effectsText != null)
        {
            if (order.lines != null && order.lines.Count > 0)
            {
                effectsText.text = string.Join("\n", order.lines);
            }
            else
            {
                effectsText.text = "";
            }
        }

        // 残り注文数（先頭を除く）
        if (remainingText != null)
        {
            int remain = Mathf.Max(0, orders.Count - 1);
            remainingText.text = remain > 0 ? $"残り {remain} 件" : "";
        }
    }

    void RebuildRow(Transform row, System.Collections.Generic.IEnumerable<Sprite> sprites)
    {
        if (row == null) return;

        ClearChildren(row);

        foreach (var sp in sprites)
        {
            if (sp == null) continue;

            Image icon = Instantiate(iconPrefab, row);
            icon.sprite = sp;
            icon.enabled = true;
            icon.raycastTarget = false;
            icon.preserveAspect = true;

            // サイズ固定（レイアウト崩れ防止）
            var rt = icon.rectTransform;
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(32, 32); // 工程は小さめが見やすい
            rt.localScale = Vector3.one;
        }
    }

    void ClearChildren(Transform t)
    {
        if (t == null) return;
        for (int i = t.childCount - 1; i >= 0; i--)
            Destroy(t.GetChild(i).gameObject);
    }

    Sprite GetBaseIcon(BaseMedicineType baseType)
    {
        return baseType switch
        {
            BaseMedicineType.Syrup => syrupBaseIcon,
            BaseMedicineType.Tablet => tabletBaseIcon,
            BaseMedicineType.Powder => powderBaseIcon,
            _ => null
        };
    }

    Sprite GetStepIcon(WorkbenchType type)
    {
        return type switch
        {
            WorkbenchType.Mix => mixIcon,
            WorkbenchType.Heating_Cooling => heating_coolingIcon,
            WorkbenchType.Press => pressIcon,
            WorkbenchType.Weigh => weighIcon,
            WorkbenchType.Packing => packingIcon,
            _ => null
        };
    }
}
