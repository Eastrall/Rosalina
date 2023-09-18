#if UNITY_EDITOR

internal interface IRosalinaCodeGeneartor
{
    RosalinaGenerationResult Generate(UIDocumentAsset document);
}

#endif