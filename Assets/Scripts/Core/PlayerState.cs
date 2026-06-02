public static class PlayerState
{
    public static bool IsDocked { get; private set; }

    public static void SetDocked(bool isDocked)
    {
        IsDocked = isDocked;
    }
}