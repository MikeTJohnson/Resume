namespace ZDDController.Views;

public partial class DailyReport : ContentPage
{
	public DailyReport()
	{
		InitializeComponent();
	}

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onDayRepButtonClicked(object sender, EventArgs e)
	{
		Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();
		DateTime today = datePicker1.Date.Date;
		if (manInstance.generateDailyReports(today))
        {
            DisplayAlert("Success", "Report created", "OK");
        }
        else
        {
            DisplayAlert("Error", "Something went wrong", "OK");
        }
    }
}
