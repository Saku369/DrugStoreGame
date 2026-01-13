using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProcessStep
{
    public WorkbenchType workbenchType;

    // テンプレ固定で使うinput/output
    public List<Ingredient> inputs = new();
    public List<Ingredient> outputs = new();

    // ★生成方式用フラグ
    public bool useOrderInputs = false;        // trueなら inputs の代わりに order.requiredInputs を使う
    public bool outputFinalProduct = false;    // trueなら outputs の代わりに order.finalProduct を出す
}

