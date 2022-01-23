internal class RosalinaGenerationResult
{
    public string Code { get; }

    public string OutputFilePath { get; }

    public RosalinaGenerationResult(string code, string outputFilePath)
    {
        Code = code;
        OutputFilePath = outputFilePath;
    }
}
