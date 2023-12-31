namespace CodeGen;

public class MainTemplateArgs
{
    public required string NamespaceName { get; set; }

    public required string ClassName { get; set; }

    public required string MethodName { get; set; }

    public string MethodLevel { get; set; } = "";
}
