#if UNITY_EDITOR

internal interface IRosalinaGeneartor
{
    RosalinaGenerationResult Generate(UIDocumentAsset documentAsset);
}

#endif