namespace ZDDController.Views;

public partial class NewInventory : ContentPage
{
	public NewInventory()
	{
		InitializeComponent();
	}

    private void onInventoryButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddInventory());
    }

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onTTFButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddTTF());
    }
}
