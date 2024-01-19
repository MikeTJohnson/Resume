namespace ZDDController.Views;

public partial class AddInventory : ContentPage
{
	public AddInventory()
	{
		InitializeComponent();
        Controllers.UniversalControllers uniInstance = new Controllers.UniversalControllers();
        picker.ItemsSource = uniInstance.getParts();
    }

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onSubmitButtonClicked(object sender, EventArgs e)
    {
        if (picker.SelectedIndex == -1 || batchNum.Text == null || fo.Text == null || qty == null)
        {
            DisplayAlert("error", "Please fill in all fields", "OK");
            return;
        }
        //grab the data
        if (!int.TryParse(fo.Text, out int FO))
        {
            DisplayAlert("error", "Invalid FO", "OK");
            return;
        }
        string pNum = picker.SelectedItem.ToString();
        if (!int.TryParse(qty.Text, out int QTY))
        {
            DisplayAlert("error", "Invalid Qantity value", "OK");
            return;
        }
        string bNum = batchNum.Text.ToString();

        //call the function to add the data to the database
        Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();
        if (manInstance.addNormalInventroy(FO, pNum, QTY, recDate.Date, bNum))
        {
            DisplayAlert("Success", "Sort added", "OK");
            fo.Text = string.Empty;
            qty.Text = string.Empty;
            picker.SelectedIndex = -1;
            batchNum.Text = string.Empty;
        }
        else
        {
            DisplayAlert("Failure", "Sort not added", "OK");
        }
    }
}
