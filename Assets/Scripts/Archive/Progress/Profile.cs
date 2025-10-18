public static class Profile
{
    public static int Current = 0; // always profile 0 unless you add save slots

    static string Key(string k) => $"PROFILE_{Current}_{k}";

    public static int GetInt(string k, int d = 0)
        => UnityEngine.PlayerPrefs.GetInt(Key(k), d);

    public static void SetInt(string k, int v)
    {
        UnityEngine.PlayerPrefs.SetInt(Key(k), v);
        UnityEngine.PlayerPrefs.Save();
    }
}
