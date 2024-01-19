namespace ZDDController.Views;

public partial class ManTools : ContentPage
{
	public ManTools()
	{
		InitializeComponent();
	}

    private void onDailyReportButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new DailyReport());
    }

    private void onPayrollReportButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PayrollReport());
    }

    private void onEmployeeReviewButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EmployeeReview());
    }

    private void onEditSortButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EditSort());
    }

    private void onSampleSortButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SampleSort());
    }

    private void onAddEmployeeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddEmployee());
    }

    private void onAddInventoryButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NewInventory());
    }

    private void onEditEmployeeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EditEmployee());
    }

    private void onAddPartButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NewPart());
    }

    private void onEditPartButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EditPart());
    }

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }
}
