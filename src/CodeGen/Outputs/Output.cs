namespace TargetNamespace;

public class TargetClass
{
    public void MethodName()
    {
    }

    public static bool TryParse(string s, out bool x) => bool.TryParse(s, out x);
    public static bool TryParse(string s, out byte x) => byte.TryParse(s, out x);
    public static bool TryParse(string s, out int x) => int.TryParse(s, out x);
    public static bool TryParse(string s, out double x) => double.TryParse(s, out x);

}
