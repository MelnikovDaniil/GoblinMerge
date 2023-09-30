using UnityEngine;

public static class CrystalMapper
{
    private const string MapperName = "Crystal";

    public static void ChangeAmount(float amount)
    {
        PlayerPrefs.SetFloat(MapperName, amount);
    }

    public static float GetAmount()
    {
        return PlayerPrefs.GetFloat(MapperName, 0);
    }
}
