namespace ZDDController.Views;

public partial class PayrollReport : ContentPage
{
	public PayrollReport()
	{
		InitializeComponent();
	}

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onPayRepButtonClicked(object sender, EventArgs e)
    {
        Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();
        DateTime day = end.Date;
        TimeSpan time = new TimeSpan(23, 59, 59);
        DateTime combined = day + time;
        if (manInstance.generatePayrollReport(start.Date, combined))
        {
            DisplayAlert("Success", "Report created", "OK");
        }
        else
        {
            DisplayAlert("Error", "Something went wrong", "OK");
        }
    }
}
