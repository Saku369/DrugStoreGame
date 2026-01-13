using System.Collections.Generic;

[System.Serializable]
public class Order
{
    public string orderName;
    public BaseMedicineType baseType;
    public RecipeTemplate template;

    // ★基本素材＝薬効（必ず1つ）
    public MedicineEffect medicineEffect;
    public Ingredient baseIngredient;

    // ★補助素材＝特性（1〜2）
    public List<TraitTag> traits = new List<TraitTag>();

    // 実際に投入が必要な材料（基本 + 補助 + (必要なら水 etc)）
    public List<Ingredient> requiredInputs = new List<Ingredient>();

    // UI表示用
    public List<string> lines = new List<string>();

    public int progressIndex = 0;
    public Ingredient finalProduct;
}
