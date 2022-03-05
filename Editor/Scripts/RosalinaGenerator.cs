internal static class RosalinaGenerator
{
    /// <summary>
    /// Generates a C# script containing the bindings of the given UI document.
    /// </summary>
    /// <param name="document">UI Document.</param>
    /// <param name="outputFileName">C# script output file.</param>
    /// <returns>Rosalina generation result.</returns>
    public static RosalinaGenerationResult GenerateBindings(UIDocumentAsset document, string outputFileName)
    {
        return new RosalinaBindingsGenerator().Generate(document, outputFileName);
    }

    /// <summary>
    /// Geneartes a C# script for the UI logic.
    /// </summary>
    /// <param name="document">UI Document asset information.</param>
    /// <param name="outputFileName">Output file name.</param>
    /// <returns>Rosalina generation result.</returns>
    public static RosalinaGenerationResult GenerateScript(UIDocumentAsset document, string outputFileName)
    {
        return new RosalinaScriptGenerator().Generate(document, outputFileName);
    }
}
