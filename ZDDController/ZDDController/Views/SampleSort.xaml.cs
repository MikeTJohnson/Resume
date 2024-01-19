
namespace ZDDController.Views;

public partial class SampleSort : ContentPage
{
	public SampleSort()
	{
		InitializeComponent();
        Controllers.UniversalControllers uniInstance = new Controllers.UniversalControllers();
        picker.ItemsSource = uniInstance.manCheckParts();
    }

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onSubmitButtonClicked(object sender, EventArgs e)
    {
        if (eid.Text == null || picker.SelectedIndex == -1)
        {
            DisplayAlert("Error", "Please fill in all fields", "OK");
        }
        else {
            string fo = picker.SelectedItem.ToString();
            if (!int.TryParse(fo, out int FO))
            {
                DisplayAlert("Error", "Invalid FO", "OK");
                return;
            }
            if (!int.TryParse(eid.Text, out int eID))
            {
                DisplayAlert("Error", "Invalid employee ID", "OK");
                return;
            }
            Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();

            if (pButton.IsChecked)
            {
                //pass
                if (manInstance.passSort(eID, FO))
                {
                    DisplayAlert("Success", "Sort passed", "OK");
                }
                else
                {
                    DisplayAlert("Error", "Something went wrong", "OK");
                }
            }
            else if (fButton.IsChecked)
            {
                //fail
                if (manInstance.failSort(FO))
                {
                    DisplayAlert("Success", "Sort failed", "OK");
                }
                else
                {
                    DisplayAlert("Error", "Something went wrong", "OK");
                }
            }
            else
            {
                DisplayAlert("Error", "Missing pass or fail", "OK");
            }

            //reset the picker
            Controllers.UniversalControllers uniInstance = new Controllers.UniversalControllers();
            picker.ItemsSource = uniInstance.manCheckParts();
        }
    }
}
