namespace ZDDController.Views;

public partial class EditEmployee : ContentPage
{
	public EditEmployee()
	{
		InitializeComponent();
	}

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onSubmitButtonClicked(object sender, EventArgs e)
    {
        if (eName.Text == null || eID.Text == null || eBadgeNum.Text == null)
        {
            DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }
        Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();
        string name = eName.Text.ToString();
        if (!int.TryParse(eID.Text, out int eid))
        {
            DisplayAlert("Error", "Invalid employee ID", "OK");
            return;
        }
        if (!int.TryParse(eBadgeNum.Text, out int bNum))
        {
            DisplayAlert("Error", "Invalid badge number", "OK");
            return;
        }

        if (manInstance.editEmployee(name, eid, bNum))
        {
            DisplayAlert("Success", "Employee updated", "OK");
        }
        else
        {
            DisplayAlert("Failure", "Employee not updated", "OK");
        }
        eName.Text = string.Empty;
        eID.Text = string.Empty;
        eBadgeNum.Text = string.Empty;
    }
}
