namespace ZDDController.Views;
using System;

public partial class ManAuth : ContentPage
{
	public ManAuth()
	{
		InitializeComponent();
	}

	private void onSubmitButtonClicked(object sender, EventArgs e)
	{
		//hash the input PIN to a hexadecimal string
		Helpers.Functions help = new Helpers.Functions();
        string pinHash = help.computeHash(pin.Text);

        //retrieve the stored hash
        if (!int.TryParse(id.Text, out int eID))
        {
            DisplayAlert("Error", "invalid employee ID", "OK");
            return;
        }
        Controllers.ManagerControllers manInstance = new Controllers.ManagerControllers();
        string retrivedHash = manInstance.getPinHash(eID);

        //compare the stored hash and the entered PIN
        Console.WriteLine(pinHash);
        Console.WriteLine(retrivedHash);
        if (pinHash == retrivedHash)
        {
            Navigation.PushAsync(new ManTools());
        }
        else
        {
            DisplayAlert("Error", "Incorrect PIN or manager ID", "OK");
        }
    }

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }
}
