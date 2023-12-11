namespace ZDDController.Views;

public partial class EndSort : ContentPage
{
	public EndSort()
	{
		InitializeComponent();
        Controllers.UniversalControllers uniInstance = new Controllers.UniversalControllers();
        picker.ItemsSource = uniInstance.partsToEnd();
    }

    private void addDefectEntry(object sender, EventArgs e)
    {
        //create a row
        StackLayout row = new StackLayout
        {
            Orientation = StackOrientation.Horizontal
        };

        //create a new Entry
        Entry dName = new Entry()
        {
            Placeholder = "Defect name",
            MaximumWidthRequest = 300,
            StyleId = "dName"
        };

        Entry qty = new Entry()
        {
            Placeholder = "Qantity",
            MaximumWidthRequest = 300,
            StyleId = "qty"
        };

        //add the Entry to the row
        row.Children.Add(dName);
        row.Children.Add(qty);

        //add the row to layout
        defectArea.Children.Add(row);
    }

    private List<String> getDNames(object sender, EventArgs e)
    {
        List<String> dNames = new List<string>();
        foreach (var row in defectArea.Children)
        {
            Console.WriteLine("checking row type");
            if (row is StackLayout elements)
            {
                Console.WriteLine("row is correct type");
                foreach(var element in elements.Children)
                {
                    Console.WriteLine("checking entry type");
                    if (element is Entry entry && entry.StyleId == "dName")
                    {
                        Console.WriteLine("entry is correct type");
                        string dName = entry.Text;
                        dNames.Add(dName);
                    }
                }
            }
        }
        return dNames;
    }

    private List<int> getDQty(object sender, EventArgs e)
    {
        List<int> dQtys = new List<int>();
        foreach (var row in defectArea.Children)
        {
            Console.WriteLine("checking row type");
            if (row is StackLayout elements)
            {
                Console.WriteLine("row is correct type");
                foreach (var element in elements.Children)
                {
                    Console.WriteLine("checking entry type");
                    if (element is Entry entry && entry.StyleId == "qty")
                    {
                        Console.WriteLine("entry is correct type");
                        if (!int.TryParse(entry.Text, out int dQty))
                        {
                            return null;
                        }
                        dQtys.Add(dQty);
                    }
                }
            }
        }
        return dQtys;
    }

    private void onHomeButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void onSubmitButtonClicked(object sender, EventArgs e)
    {
        Console.WriteLine("getting dNames and dQtys");
        List<String> dNames = getDNames(sender, e);
        List<int> dQtys = getDQty(sender, e);
        if (dQtys is null && dNames is not null)
        {
            DisplayAlert("Error", "Invalid Qantity value", "OK");
        }

        Console.WriteLine("getting fo");
        string fo = picker.SelectedItem.ToString();
        if (!int.TryParse(fo, out int FO))
        {
            DisplayAlert("Error", "Invalid FO", "OK");
            return;
        }

        Console.WriteLine("getting sid");
        Controllers.GeneralControllers genInstance = new Controllers.GeneralControllers();
        int sid = genInstance.getSid(FO);
        if (sid < 0)
        {
            DisplayAlert("Error", "FO does not exist. please ensure it is entered correctly", "OK");
            return;
        }

        Console.WriteLine("getting finished boolean");
        bool finished;
        if (finish.IsChecked)
        {
            finished = true;
        }
        else if (notFinish.IsChecked)
        {
            finished = false;
        }
        else
        {
            DisplayAlert("Error", "Please select if you are finishing the sort or not", "OK");
            return;
        }

        Console.WriteLine("getting dateTime");
        DateTime combined = endDate.Date.Date + endTime.Time;
        Console.WriteLine("getting comments");
        string comments = comment.Text;
        Console.WriteLine("getting eID");
        if (!int.TryParse(eID.Text, out int eid))
        {
            DisplayAlert("Error", "Invalid employee ID", "OK");
            return;
        }

        if (finished)
        {
            //theres a problem here
            if (genInstance.completeSort(dNames, dQtys, sid, comments, combined, eid))
            {
                DisplayAlert("Success", "Sort Recorded", "OK");
                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Failure", "Sort not Recorded", "OK");
            }
        }
        else if (!finished)
        {
            if (genInstance.stopWorkingOnSort(sid, combined, eid))
            {
                DisplayAlert("Success", "Time recorded", "OK");
                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Failure", "Time not recorded", "OK");
            }
        }
        else
        {
            DisplayAlert("Error", "Something went wrong", "OK");
        }
    }
}
