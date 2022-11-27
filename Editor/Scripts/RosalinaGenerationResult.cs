#if UNITY_EDITOR
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
    /// Creates a new <see cref="RosalinaGenerationResult"/> instance.
    /// </summary>
    /// <param name="code">Generated code.</param>
    public RosalinaGenerationResult(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Saves the result to the output path.
    /// </summary>
    /// <param name="outputFilePath">Output file path.</param>
    public void Save(string outputFilePath)
    {
        if (!string.IsNullOrEmpty(outputFilePath))
        {
            File.WriteAllText(outputFilePath, Code);
        }
    }
}
#endif