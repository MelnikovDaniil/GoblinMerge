using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BonusManager))]
public class BobusEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var bonusManager = (BonusManager)target;

        DrawDefaultInspector();
        if (GUILayout.Button("Spawn Bonus"))
        {
            var createdBonus = Instantiate(bonusManager.bonusPrefab, UnitShopManager.Instance.GetRandomSpawnPostion(), Quaternion.identity);
            createdBonus.action = () => bonusManager.SpawnUnit(UnitType.Goblin, bonusManager.buttonSpawnGoblinLevel);
        }
    }
}
