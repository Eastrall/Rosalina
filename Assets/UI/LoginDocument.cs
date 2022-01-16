public partial class LoginDocument 
{
    private void OnEnable()
    {
        InitializeDocument();
        Button1.clicked += OnButtonClick;
    }

    private void OnButtonClick()
    {
        TitleLabel.text = "Clicked!";
    }
}
