namespace ZDDController.Views;

public partial class EditPart : ContentPage
{


	public EditPart()
	{
		InitializeComponent();
        Controllers.UniversalControllers uniInstance = new Controllers.UniversalControllers();
        picker.ItemsSource = uniInstance.getParts();
	}

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        if (picker.SelectedIndex != -1)
        {
            Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();
            string pn = (string)picker.SelectedItem;
            DataClasses.Part part = manInstance.populateEditPart(pn);
            ppmNum.Text = part.ppm.ToString();
            rate.Text = part.rate.ToString();
            ttfRate.Text = part.ttfRate.ToString();
            if (part.bwi)
            {
                bwi.IsChecked = true;
            }
            else if (part.oring)
            {
                oring.IsChecked = true;
            }
            else if (part.special)
            {
                special.IsChecked = true;
            }
        }
    }

    private void onSubmitButtonClicked(object sender, EventArgs e)
    {
        float ppm = 0;
        float price = 0;
        float ttfPrice = 0;
        //TODO handle null fields differently
        //TODO how to change entry elements to hold the values of the picker selection query
        if (picker.SelectedIndex == -1)
        {
            DisplayAlert("Error", "Please choose a part number", "OK");
            return;
        }
        else
        {
            Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();
            if (!string.IsNullOrEmpty(ppmNum.Text))
            {
                if (!float.TryParse(ppmNum.Text, out float num))
                {
                    DisplayAlert("Error", "Invalid parts per minutes value", "OK");
                    return;
                }
                ppm = num;
            }
            else
            {
                DisplayAlert("Error", "Please enter a parts per minute value", "OK");
            }
            if (!string.IsNullOrEmpty(rate.Text))
            {
                if (!float.TryParse(rate.Text, out float num))
                {
                    DisplayAlert("Error", "Invalid rate", "OK");
                    return;
                }
                price = num;
            }
            else
            {
                DisplayAlert("Error", "Please enter a rate value", "OK");
            }
            if (!string.IsNullOrEmpty(ttfRate.Text))
            {
                if (!float.TryParse(ttfRate.Text, out float num))
                {
                    DisplayAlert("Error", "Invalid TTF rate", "OK");
                    return;
                }
                ttfPrice = num;
            }
            else
            {
                DisplayAlert("Error", "Please enter a TFF rate value", "OK");
            }

            if (manInstance.editPart(picker.SelectedItem.ToString(), ppm, price, ttfPrice))
            {
                DisplayAlert("Success", "Part updated", "OK");
                ppmNum.Text = string.Empty;
                rate.Text = string.Empty;
                picker.SelectedIndex = -1;
                ttfRate.Text = ".165";
            }
            else
            {
                DisplayAlert("Failure", "Part not updated", "OK");
            }
        }
    }
}
