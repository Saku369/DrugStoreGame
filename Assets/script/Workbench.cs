using System.Collections.Generic;
using UnityEngine;

public abstract class Workbench : MonoBehaviour
{
    public WorkbenchType workbenchType;

    [Header("Safety Filter (optional)")]
    public bool useTypeFilter = false;                 // ONならタイプ制限する
    public List<IngredientType> allowedTypes = new();   // 例: Raw, Powder など

    public void Use(PlayerInventory inventory, Recipe currentRecipe)
    {
        if (inventory == null || currentRecipe == null) return;

        int idx = inventory.currentProcessIndex;
        if (idx >= currentRecipe.processSteps.Count)
        {
            Debug.Log("工程は完了しています");
            return;
        }

        var step = currentRecipe.processSteps[idx];

        // 作業台の種類チェック
        if (step.workbenchType != workbenchType)
        {
            Debug.Log("工程ミス：作業台が違う");
            inventory.ResetProcess();
            return;
        }

        // Safety Filter（任意）
        if (useTypeFilter && step.inputs != null)
        {
            foreach (var ing in step.inputs)
            {
                if (ing == null) { inventory.ResetProcess(); return; }
                if (!allowedTypes.Contains(ing.type))
                {
                    Debug.Log($"この台では {ing.type} を扱えません");
                    inventory.ResetProcess();
                    return;
                }
            }
        }

        // 材料チェック（順不同・個数一致）
        if (!HasRequiredIngredients(inventory.ingredients, step.inputs))
        {
            Debug.Log("工程ミス：材料が足りない/違う");
            inventory.ResetProcess();
            return;
        }

        // 消費
        foreach (var ing in step.inputs)
            inventory.ingredients.Remove(ing);

        // 生成
        if (step.outputs != null)
        {
            foreach (var outIng in step.outputs)
                inventory.AddIngredient(outIng);
        }

        inventory.currentProcessIndex++;
        Debug.Log($"工程成功：{workbenchType} ({inventory.currentProcessIndex}/{currentRecipe.processSteps.Count})");
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
}
