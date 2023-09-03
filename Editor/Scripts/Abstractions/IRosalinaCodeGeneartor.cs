#if UNITY_EDITOR

internal interface IRosalinaCodeGeneartor
{
    RosalinaGenerationResult Generate(UIDocumentAsset documentAsset);
}

#endif