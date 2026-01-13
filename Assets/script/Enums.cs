
public enum IngredientType
{
    Raw,
    Liquid,
    Powder
}

public enum ProcessType
{
    Mortar,
    Mixing,
    Heating_Cooling,
    Packing,
    Weighing
}

public enum BaseMedicineType
{
    Syrup,   // シロップ
    Tablet,  // 錠剤
    Powder   // 粉薬
}

public enum MedicineEffect   // ★薬の効果そのもの（基本素材）
{
    Activator,
    Adjusting,
    Recovery,
    Sedation
}

public enum TraitTag         // ★補助素材（特性）
{
    Deliciousness,
    Durability,
    Immediate,
    Stability
}

public enum WorkbenchType
{
    Mix,
    Heating_Cooling,
    Press,
    Weigh,
    Packing
}