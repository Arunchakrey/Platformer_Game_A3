using UnityEngine;

public static class Progression
{
    static string Key(string k) => $"PROFILE_{Profile.Current}_{k}";

    public static int Total
    {
        get => PlayerPrefs.GetInt(Key("TOTAL_POINTS"), 0);
        set { PlayerPrefs.SetInt(Key("TOTAL_POINTS"), value); PlayerPrefs.Save(); }
    }

    // Unlock checks
    public static bool L2 => Total >= 50;
    public static bool L3 => Total >= 120;
    public static bool L4 => Total >= 220;
}
