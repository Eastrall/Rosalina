using System;

internal interface IRosalinaGenerator
{
    RosalinaGenerationResult Generate(UIDocumentAsset document, string outputFileName);
}
