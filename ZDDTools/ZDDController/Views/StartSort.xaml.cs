namespace ZDDController.Views;

public partial class StartSort : ContentPage
{
	public StartSort()
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
        if (picker.SelectedIndex == -1 || fo.Text == null || eID.Text == null)
        {
            DisplayAlert("Error", "Please fill in all fields", "OK");
        }
        else
        {
            //grab the data
            if (!int.TryParse(fo.Text, out int FO))
            {
                DisplayAlert("Error", "invalid FO", "OK");
                return;
            }
            string pNum = picker.SelectedItem.ToString();
            if (!int.TryParse(eID.Text, out int EID))
            {
                DisplayAlert("Error", "invalid employee ID", "OK");
                return;
            }
            DateTime combined = date.Date.Date + startTime.Time;

            //call the function to add the data to the database
            Controllers.GeneralControllers genInstance = new Controllers.GeneralControllers();
            if (genInstance.startSort(EID, combined, FO, pNum))
            {
                DisplayAlert("Success", "Work started", "OK");
                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Failure", "Error starting work. Check fields", "OK");
            }
        }
    }
}
