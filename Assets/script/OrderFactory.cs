using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderFactory : MonoBehaviour
{
    [Header("Templates (by base type)")]
    public RecipeTemplate syrupTemplate;
    public RecipeTemplate tabletTemplate;
    public RecipeTemplate powderTemplate;

    [Header("Optional: Syrup extra base (e.g. Water)")]
    public Ingredient waterIngredient; // Syrupの時に追加で必要にしたいなら設定（不要ならnullでOK）

    [Header("MedicineEffect -> Base Ingredient (required 1)")]
    public Ingredient activatorBase;
    public Ingredient adjustingBase;
    public Ingredient recoveryBase;
    public Ingredient sedationBase;

    [Header("Trait -> Ingredient mapping (support materials)")]
    public Ingredient deliciousnessIngredient;
    public Ingredient durabilityIngredient;
    public Ingredient immediateIngredient;
    public Ingredient stabilityIngredient;

    [Header("Final product mapping")]
    public Ingredient syrupFinalProduct;
    public Ingredient tabletFinalProduct;
    public Ingredient powderFinalProduct;

    [Header("How many traits per order")]
    [Min(0)] public int minTraits = 1;
    [Min(0)] public int maxTraits = 2;

    public Order CreateRandomOrder()
    {
        var order = new Order();

        // 1) base type（Syrup / Tablet / Powder）
        order.baseType = (BaseMedicineType)Random.Range(0, 3);

        // 2) template
        order.template = order.baseType switch
        {
            BaseMedicineType.Syrup => syrupTemplate,
            BaseMedicineType.Tablet => tabletTemplate,
            BaseMedicineType.Powder => powderTemplate,
            _ => syrupTemplate
        };

        // 3) medicine effect（基本素材＝薬効）を必ず1つ
        (order.medicineEffect, order.baseIngredient) = GetRandomMedicineBase();

        // 4) traits（補助素材＝特性）を1〜2（重複なし）
        order.traits = CreateRandomTraits(minTraits, maxTraits);

        // 5) required inputs（投入が必要な材料）
        order.requiredInputs = new List<Ingredient>();

        // 基本素材は必須
        if (order.baseIngredient != null)
        {
            order.requiredInputs.Add(order.baseIngredient);
        }
        else
        {
            Debug.LogWarning("基本素材(Activator/Adjusting/Recovery/Sedation)のIngredientが未設定です（OrderFactoryのInspectorを確認）");
        }

        // Syrupなら水など追加したい場合（任意）
        if (order.baseType == BaseMedicineType.Syrup && waterIngredient != null)
        {
            order.requiredInputs.Add(waterIngredient);
        }

        // 特性ぶん補助素材を追加
        foreach (var t in order.traits)
        {
            var ing = GetIngredientForTrait(t);
            if (ing != null) order.requiredInputs.Add(ing);
            else Debug.LogWarning($"Trait {t} のIngredientが未設定です（OrderFactoryのInspectorを確認）");
        }

        // 6) final product
        order.finalProduct = order.baseType switch
        {
            BaseMedicineType.Syrup => syrupFinalProduct,
            BaseMedicineType.Tablet => tabletFinalProduct,
            BaseMedicineType.Powder => powderFinalProduct,
            _ => null
        };

        // 7) UI表示用テキスト
        order.lines = new List<string>
        {
            $"効果：{MedicineEffectLabel(order.medicineEffect)}"
        };

        if (order.traits != null && order.traits.Count > 0)
        {
            order.lines.Add("特性：");
            foreach (var t in order.traits)
            {
                order.lines.Add($"・{TraitLine(t)}");
            }
        }

        order.orderName = BuildOrderName(order);

        // 8) progress
        order.progressIndex = 0;

        return order;
    }

    // -------------------------
    // Helpers
    // -------------------------

    List<TraitTag> CreateRandomTraits(int min, int max)
    {
        // 安全化
        int minClamped = Mathf.Max(0, min);
        int maxClamped = Mathf.Max(minClamped, max);
        int count = Random.Range(minClamped, maxClamped + 1);

        var pool = System.Enum.GetValues(typeof(TraitTag)).Cast<TraitTag>().ToList();
        var result = new List<TraitTag>();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int pick = Random.Range(0, pool.Count);
            result.Add(pool[pick]);
            pool.RemoveAt(pick);
        }

        return result;
    }

    (MedicineEffect, Ingredient) GetRandomMedicineBase()
    {
        int r = Random.Range(0, 4);
        return r switch
        {
            0 => (MedicineEffect.Activator, activatorBase),
            1 => (MedicineEffect.Adjusting, adjustingBase),
            2 => (MedicineEffect.Recovery, recoveryBase),
            _ => (MedicineEffect.Sedation, sedationBase),
        };
    }

    Ingredient GetIngredientForTrait(TraitTag tag)
    {
        return tag switch
        {
            TraitTag.Deliciousness => deliciousnessIngredient,
            TraitTag.Durability => durabilityIngredient,
            TraitTag.Immediate => immediateIngredient,
            TraitTag.Stability => stabilityIngredient,
            _ => null
        };
    }

    string MedicineEffectLabel(MedicineEffect effect)
    {
        return effect switch
        {
            MedicineEffect.Activator => "活性",
            MedicineEffect.Adjusting => "調整",
            MedicineEffect.Recovery => "回復",
            MedicineEffect.Sedation => "鎮静",
            _ => effect.ToString()
        };
    }


    string TraitLine(TraitTag tag)
    {
        // 表示名（好きに調整OK）
        return tag switch
        {
            TraitTag.Deliciousness => "おいしい",
            TraitTag.Durability => "持続性",
            TraitTag.Immediate => "即効性",
            TraitTag.Stability => "安定性",
            _ => tag.ToString()
        };
    }

    string BuildOrderName(Order order)
    {
        // 例：Tablet / Recovery + Immediate + Stability
        string baseName = order.baseType.ToString();
        string traits = (order.traits != null && order.traits.Count > 0)
            ? string.Join("+", order.traits.Select(t => t.ToString()))
            : "NoTrait";
        return $"{baseName} / {order.medicineEffect} + {traits}";
    }
}
