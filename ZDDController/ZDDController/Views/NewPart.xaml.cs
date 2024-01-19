namespace ZDDController.Views;

public partial class NewPart : ContentPage
{
	public NewPart()
	{
		InitializeComponent();
	}

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onSubmitButtonClicked(object sender, EventArgs e)
    {
        bool BWI = false;
        bool Oring = false;
        bool Special = false;
        if (bwi.IsChecked)
        {
            BWI = true;
        }
        else if (oring.IsChecked)
        {
            Oring = true;
        }
        else if (special.IsChecked)
        {
            Special = true;
        }
        else
        {
            DisplayAlert("Error", "Please choose a type for the part", "OK");
            return;
        }
        if (rate.Text == null || ppmNum.Text == null || partNum.Text == null || ttfRate.Text == null)
        {
            DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }
        else
        {
            Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();
            if (!float.TryParse(rate.Text, out float rateFloat))
            {
                DisplayAlert("Error", "Invalid rate entry", "OK");
                return;
            }
            if (!float.TryParse(ppmNum.Text, out float ppmFloat))
            {
                DisplayAlert("Error", "Invalid parts per minute value", "OK");
                return;
            }
            if (!float.TryParse(ttfRate.Text, out float ttfRateFloat))
            {
                DisplayAlert("Error", "Invalid TTF rate entry", "OK");
                return;
            }
            Console.WriteLine("casted " + ppmFloat + " " + rateFloat);

            if (manInstance.makePart(partNum.Text, rateFloat, ppmFloat, ttfRateFloat, BWI, Oring, Special))
            {
                DisplayAlert("Success", "Part added", "OK");
                rate.Text = string.Empty;
                ppmNum.Text = string.Empty;
                partNum.Text = string.Empty;
                ttfRate.Text = ".165";
            }
            else
            {
                DisplayAlert("Failure", "Part not added", "OK");
            }
        }
    }
}
