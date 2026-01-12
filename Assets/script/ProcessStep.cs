using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProcessStep
{
    public WorkbenchType workbenchType;
    public List<Ingredient> inputs;   // 消費する
    public List<Ingredient> outputs;  // 生成する（粉や完成品など）
}
