namespace WebApp.ViewModels;

public class HomeIndexViewModel
{
    public string PageTitle { get; set; } = "Home";
    public TestFormModel Form { get; set; } = new TestFormModel();
}