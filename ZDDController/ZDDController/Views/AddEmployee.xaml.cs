namespace ZDDController.Views;

public partial class AddEmployee : ContentPage
{
	public AddEmployee()
	{
		InitializeComponent();
	}

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onSubmitButtonClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(eName.Text))
        {
            DisplayAlert("Error", "Please enter employee name", "OK");
            return;
        }
        else
        {
            int badgeNum = 0;
            Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();

            //check if the eBadgeNum is empty and parse it
            if (!string.IsNullOrEmpty(eBadgeNum.Text))
            {
                if (!int.TryParse(eBadgeNum.Text, out int num))
                {
                    DisplayAlert("Error", "Invalid badge number", "OK");
                    return;
                }
                badgeNum = num;
            }
            Console.WriteLine("casted " + badgeNum);

            if (manInstance.makeEmployee(eName.Text, badgeNum))
            {
                DisplayAlert("Success", "New employee ID: " + manInstance.getLastEID(), "OK");
                eName.Text = string.Empty;
                eBadgeNum.Text = string.Empty;
            }
            else
            {
                DisplayAlert("Failure", "Employee not added", "OK");
            }
        }
    }
}
