namespace JYCEngine;

public static class Input
{
    private static Action<ConsoleKeyInfo> _keyPressedCallback;
    private static Dictionary<char, Action> _bindings = new();

    public static void RegisterBinding(char key, Action action)
    {
        _bindings.Add(key, action);
    }

    public static void Run()
    {
        var keys = GetInput();

        foreach (var key in keys)
        {
            _keyPressedCallback?.Invoke(key);
            if (_bindings.ContainsKey(key.KeyChar))
                _bindings[key.KeyChar]?.Invoke();
        }
    }

    public static IEnumerable<ConsoleKeyInfo> GetInput()
    {
        var input = new HashSet<ConsoleKeyInfo>();
        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);
            input.Add(key);
        }
        return input;
    }
}