# Rosalina
Experimenting code generation with Unity's UXML for generating powerful UI Elements code behind.

## How it works

Rosalina watches your changes related to all `*.uxml` files, parses its content and generates the according C# code based on the element's names.

![alt](https://github.com/Eastrall/Rosalina/blob/main/docs/rosalina-code-gen.png?raw=true)

![alt](https://i.gyazo.com/291ca8702846fcff5c2a53ba4c766536.gif)

## Known limitations

For now, Rosalina only generates the UI Document code based on the UI element names. You still need to create on your own the `GameObject` with a `UIDocument` component and then add the generated script.

It is recommended to create a new `class` with the same name as the document you are creating that will hold the UI logic. If you UI document name is: `MenuDocument`, Rosalina will create a `MenuDocument.g.cs`, and you'll need to create a `MenuDocument.cs` and make it `partial` so it can share the elements with the generated code.
Finally, call the `InitializeDocument()` method in the `Awake()` hook, to initialize the UI properties.

```csharp
public partial class MenuDocument
{
    private void Awake()
    {
        InitializeDocument(); // Very important!
    }
}
```

## Final words

This is still in early development and the process of code generation and file creation will be improved, ajusted and optimized.
