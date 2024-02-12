using ZDDController.Views;
namespace ZDDController;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}


    private void onGenButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new GenTools());
    }

    private void onManButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ManAuth());
    }
}


