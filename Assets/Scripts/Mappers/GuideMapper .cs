using UnityEngine;

public static class GuideMapper
{
    public enum GuideStep
    {
        Click,
        Buyer,
        Merge,
    }

    private const string MapperName = "Guide";

    public static void FinishGuideStep(GuideStep guideStep)
    {
        PlayerPrefs.SetInt(MapperName + guideStep.ToString(), 1);
    }

    public static bool GetGuideStep(GuideStep guideStep)
    {
        return PlayerPrefs.GetInt(MapperName + guideStep.ToString(), 0) == 1;
    }
}
