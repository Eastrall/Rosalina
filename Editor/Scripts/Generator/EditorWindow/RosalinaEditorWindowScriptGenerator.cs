#if UNITY_EDITOR

using System;

internal class RosalinaEditorWindowScriptGenerator : IRosalinaGeneartor
{
    public RosalinaGenerationResult Generate(UIDocumentAsset documentAsset)
    {
        if (documentAsset is null)
        {
            throw new ArgumentNullException(nameof(documentAsset), "Cannot generate binding with a null document asset.");
        }

        throw new NotImplementedException();
    }
}

#endif