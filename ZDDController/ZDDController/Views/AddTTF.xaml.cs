namespace ZDDController.Views;

public partial class AddTTF : ContentPage
{
	public AddTTF()
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
        //grab the data
        if (picker.SelectedIndex == -1 || batchNum.Text == null || fo.Text == null || qty == null)
        {
            DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }
        if (!int.TryParse(fo.Text, out int FO))
        {
            DisplayAlert("Error", "Invalid FO", "OK");
            return;
        }
        string pNum = picker.SelectedItem.ToString();
        if (!int.TryParse(qty.Text, out int QTY))
        {
            DisplayAlert("Error", "Invalid Qantity value", "OK");
            return;
        }
        string bNum = batchNum.Text.ToString();

        //call the function to add the data to the database
        Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();
        if (manInstance.addTTFInventory(FO, pNum, QTY, originalDate.Date, bNum, recDate.Date))
        {
            DisplayAlert("Success", "TTF added", "OK");
            fo.Text = string.Empty;
            qty.Text = string.Empty;
            picker.SelectedIndex = -1;
            batchNum.Text = string.Empty;
            originalDate.Date = recDate.Date;
        }
        else
        {
            DisplayAlert("Failure", "TTF not added", "OK");
        }
    }
}
