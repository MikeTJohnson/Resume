namespace ZDDController.Views;

public partial class GenTools : ContentPage
{
	public GenTools()
	{
		InitializeComponent();
	}

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onStartButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StartSort());
    }

    private void onSpecialButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StartSpecialSort());
    }

    private void onEndButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EndSort());
    }

    private void onSpecialEndButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EndSpecialSort());
    }
}
