public static class GameState
{
    public static StarSystemDefinition CurrentSystem { get; private set; }

    public static void SetCurrentSystem(StarSystemDefinition system)
    {
        CurrentSystem = system;
    }
}