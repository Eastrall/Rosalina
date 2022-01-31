# Rosalina

Rosalina is a code generation tool for Unity's UI documents. It allows developers to generate C# UI bindings and code-behind scripts based on a UXML template.

## How it works

Rosalina watches your changes related to all `*.uxml` files, parses its content and generates the according C# UI binding code based on the element's names.

Take for instance the following UXML template:

**`SampleDocument.uxml`**
```xml
<ui:UXML xmlns:ui="UnityEngine.UIElements" xmlns:uie="UnityEditor.UIElements" xsi="http://www.w3.org/2001/XMLSchema-instance" 
         engine="UnityEngine.UIElements" editor="UnityEditor.UIElements" 
         noNamespaceSchemaLocation="../../UIElementsSchema/UIElements.xsd" editor-extension-mode="False">
    <ui:VisualElement>
        <ui:Label text="Label" name="TitleLabel" />
        <ui:Button text="Button" name="Button" />
    </ui:VisualElement>
</ui:UXML>
```

Rosalina's `AssetProcessor` will automatically genearte the following C# UI bindings script:

**`SampleDocument.g.cs`**
```csharp
// <autogenearted />
using UnityEngine;
using UnityEngine.UIElements;

public partial class SampleDocument
{
    [SerializeField]
    private UIDocument _document;
    public Label TitleLabel { get; private set; }

    public Button Button { get; private set; }

    public VisualElement Root
    {
        get
        {
            return _document?.rootVisualElement;
        }
    }

    public void InitializeDocument()
    {
        TitleLabel = (Label)Root?.Q("TitleLabel");
        Button = (Button)Root?.Q("Button");
    }
}
```

> ⚠️ This script behing an auto-genearted code based on the UXML template, **you should not** write code inside this file. It will be overwritten everytime you update your UXML template file.

Rosalina provides a context-menu option to generate a C# UI script where you can place your UI related code without the risk of behing overwritten by Rosalina's asset processor.
Just right click on the UXML and access `Rosalina` menu-item, then select `Genearte UI script`.

![image](https://user-images.githubusercontent.com/4021025/151774578-84a648a3-5907-4f54-ba7e-c49ab5808a3c.png)

This option will generate the following code:

**`SampleDocument.cs`**
```csharp
using UnityEngine;

public partial class SampleDocument : MonoBehaviour
{
    private void OnEnable()
    {
        InitializeDocument();
    }
}
```

## Notes

As pointed out by [JuliaP_Unity](https://forum.unity.com/members/juliap_unity.4707193/) on [Unity Forums](https://forum.unity.com/threads/share-your-ui-toolkit-projects.980061/#post-7799040) the document initialization process (element queries) **should** be done on the `OnEnable()` hook, since the `UIDocument` visual tree asset is instancied at this moment.
*Thank you for the tip!*

## Known limitations

For now, Rosalina only generates the UI Document bindings and code behding scripts based on the UI element names. You still need to create on your own the `GameObject` with a `UIDocument` component and then add the UI script (not the UI binding scripts).

> ℹ️ In next versions, we could think of an extension that automatically creates a GameObject with the UI script attached to it. 😄 

## Final words

If you like the project, don't hesitate to contribute! All contributions are welcome!
