using System.IO;

/// <summary>
/// Describes a generation result.
/// </summary>
internal class RosalinaGenerationResult
{
    /// <summary>
    /// Gets the generated code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the output file path.
    /// </summary>
    public string OutputFilePath { get; }

    /// <summary>
    /// Creates a new <see cref="RosalinaGenerationResult"/> instance.
    /// </summary>
    /// <param name="code">Generated code.</param>
    /// <param name="outputFilePath">Output file path.</param>
    public RosalinaGenerationResult(string code, string outputFilePath)
    {
        Code = code;
        OutputFilePath = outputFilePath;
    }

    /// <summary>
    /// Saves the result to the output path.
    /// </summary>
    public void Save()
    {
        File.WriteAllText(OutputFilePath, Code);
    }
}
