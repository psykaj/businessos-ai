namespace backend.Modules.Workflow.Interfaces;

public interface IVariableResolver
{
    string Resolve(string template, Dictionary<string, string> context);
    Dictionary<string, string> ResolveDictionary(Dictionary<string, string> templateDict, Dictionary<string, string> context);
}
