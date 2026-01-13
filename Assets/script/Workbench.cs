using System.Collections.Generic;
using UnityEngine;

public abstract class Workbench : MonoBehaviour
{
    public WorkbenchType workbenchType;

    [Header("Input Buffer (for single-slot carry)")]
    public int bufferCapacity = 3;               // ★調合用：最大何個まで投入できるか
    protected List<Ingredient> buffer = new();   // ★作業台に投入された材料

    [Header("Safety Filter (optional)")]
    public bool useTypeFilter = false;
    public List<IngredientType> allowedTypes = new();

    /// <summary>
    /// 単一スロット（inventory.held）を前提に、作業台に「投入」→必要数揃えば工程を進める
    /// </summary>
    public void Use(PlayerInventory inventory, Order currentOrder)
    {
        if (inventory == null || currentOrder == null) return;
        if (currentOrder.template == null || currentOrder.template.steps == null)
        {
            Debug.LogWarning("Order に template が設定されていません");
            return;
        }

        int idx = currentOrder.progressIndex;
        inventory.currentProcessIndex = idx;

        if (idx >= currentOrder.template.steps.Count)
        {
            Debug.Log("工程は完了しています（Completeボタンで納品）");
            return;
        }

        var step = currentOrder.template.steps[idx];

        // 作業台の種類チェック
        if (step.workbenchType != workbenchType)
        {
            Debug.Log("工程ミス：作業台が違う");
            ResetProgress(inventory, currentOrder);
            ClearBuffer();
            return;
        }

        // 入力/出力の決定
        List<Ingredient> requiredInputs = step.useOrderInputs ? currentOrder.requiredInputs : step.inputs;

        // ★1) まず「投入」を試みる（持っている場合）
        if (inventory.held != null)
        {
            // タイプ制限（任意）
            if (useTypeFilter && allowedTypes != null && allowedTypes.Count > 0)
            {
                if (!allowedTypes.Contains(inventory.held.type))
                {
                    Debug.Log("投入不可：この作業台では扱えない材料タイプです");
                    return;
                }
            }

            // バッファ容量
            if (buffer.Count >= bufferCapacity)
            {
                Debug.Log("投入不可：作業台がいっぱいです");
                return;
            }

            // 投入
            Ingredient put = inventory.TakeHeld();
            buffer.Add(put);
            Debug.Log($"投入：{put.ingredientName}（{buffer.Count}/{bufferCapacity}）");
        }
        else
        {
            // 何も持ってない時に押された
            Debug.Log($"（{workbenchType}）何も持っていません。必要素材を持ってきて投入してください。");
        }

        // ★2) 揃っていないならここで終了
        if (!HasRequiredIngredients(buffer, requiredInputs))
        {
            Debug.Log($"まだ材料が足りません（必要:{requiredInputs?.Count ?? 0} / 現在:{buffer.Count}）");
            return;
        }

        // ★3) 揃ったので消費（bufferから必要分を削除）
        ConsumeRequiredIngredients(buffer, requiredInputs);

        // ★4) 生成（生成物はPlayerの手に持たせる or 置く）
        // 今回は「Playerが空なら手に持つ」「埋まってたら投入不可で止める」方式にする
        Ingredient produced = null;

        if (step.outputFinalProduct)
        {
            produced = currentOrder.finalProduct;
            if (produced == null)
            {
                Debug.LogWarning("finalProduct が未設定です（OrderFactory の設定を確認）");
                ResetProgress(inventory, currentOrder);
                ClearBuffer();
                return;
            }
        }
        else
        {
            // outputsが複数ある場合、単一スロットなので1つ目だけ採用（必要なら後で拡張）
            if (step.outputs != null && step.outputs.Count > 0)
                produced = step.outputs[0];
        }

        if (produced != null)
        {
            if (inventory.held != null)
            {
                Debug.Log("生成物を持てません（手が埋まっています）。先に手を空にしてください。");
                // 工程を進めない／戻す、どっちでもいい。今回は進めないで止める
                // 消費済みなので、ここで止めるのはゲーム的に厳しいなら、生成物を床に出す方式に変える
                return;
            }

            inventory.TryHold(produced);
        }

        // ★5) 進捗更新
        currentOrder.progressIndex++;
        inventory.currentProcessIndex = currentOrder.progressIndex;

        Debug.Log($"工程成功：{workbenchType} ({currentOrder.progressIndex}/{currentOrder.template.steps.Count})");

        // 次工程へ進むので、バッファは基本クリア（工程ごとに材料持ち越さない想定）
        ClearBuffer();
    }

    private static void ResetProgress(PlayerInventory inventory, Order order)
    {
        if (inventory != null) inventory.ResetProcess();
        if (order != null) order.progressIndex = 0;
    }

    protected bool HasRequiredIngredients(List<Ingredient> inv, List<Ingredient> req)
    {
        if (inv == null || req == null) return false;
        if (inv.Count < req.Count) return false;

        var temp = new List<Ingredient>(inv);
        foreach (var r in req)
        {
            if (r == null) return false;
            if (temp.Contains(r)) temp.Remove(r);
            else return false;
        }
        return true;
    }

    protected void ConsumeRequiredIngredients(List<Ingredient> inv, List<Ingredient> req)
    {
        if (inv == null || req == null) return;

        foreach (var r in req)
        {
            inv.Remove(r); // 1個ずつ消す（重複もOK）
        }
    }

    protected void ClearBuffer()
    {
        buffer.Clear();
    }

    // デバッグ用：今入ってる材料を見たいとき
    public string DebugBuffer()
    {
        if (buffer.Count == 0) return "(empty)";
        var names = new List<string>();
        foreach (var b in buffer) names.Add(b != null ? b.ingredientName : "null");
        return string.Join(", ", names);
    }
}
