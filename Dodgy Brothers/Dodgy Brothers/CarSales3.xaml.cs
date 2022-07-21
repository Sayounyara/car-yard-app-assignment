using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Dodgy_Brothers
{
    /// 26/05/2022
    /// assignment CarSalesV3
    public sealed partial class CarSales3 : Page
    {
        string[] names = new string[10];
        string[] phoneNumbers = new string[10];
        string[] vehicleMakes = new string[8];

        const double GST_RATE = 0.1;
        const double TWO_YEAR_WARRANTY = 0.05;
        const double THREE_YEAR_WARRANTY = 0.1;
        const double FIVE_YEAR_WARRANTY = 0.2;

        const double UNDER_TWENTYFIVE_INSURANCE = 0.2;
        const double OVER_TWENTYFIVE_INSURANCE = 0.1;

        const double TINTED_WINDOWS = 150.0;
        const double DUCO_PROTECTION = 180.0;
        const double FLOOR_MATS = 320.0;
        const double SOUND_SYSTEM = 350.0;

        public CarSales3()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            names[0] = "Jeff Winger";
            phoneNumbers[0] = "720 555 0264";

            names[1] = "Britta Perry";
            phoneNumbers[1] = "909 555 7538";

            names[2] = "Troy Bolton";
            phoneNumbers[2] = "505 555 3566";

            names[3] = "Luz Noceda";
            phoneNumbers[3] = "475 555 2648";

            names[4] = "Samuel Axe";
            phoneNumbers[4] = "786 555 0613";

            names[5] = "Maurice Moss";
            phoneNumbers[5] = "0118 999 881 999 911 9725 3";

            names[6] = "Richard Vernon";
            phoneNumbers[6] = "312 407 1982";

            names[7] = "Emmet Brown";
            phoneNumbers[7] = "619 850 2015";

            names[8] = "Ben Chang";
            phoneNumbers[8] = "909 555 1969";

            names[9] = "Gouji Rokkaku";
            phoneNumbers[9] = "03 5735-2002";

            vehicleMakes[0] = "Toyota";
            vehicleMakes[1] = "Holden";
            vehicleMakes[2] = "Mitsubishi";
            vehicleMakes[3] = "Ford";
            vehicleMakes[4] = "BMW";
            vehicleMakes[5] = "Mazda";
            vehicleMakes[6] = "Volkswagen";
            vehicleMakes[7] = "Mini";
        }

        private async void summaryButton_Click(object sender, RoutedEventArgs e)
        {
            double subAmount;
            double gstAmount;
            double finalAmount;

            double purchaseVehicleCost = 0.0;
            double tradeInValue = 0.0;
            Boolean purchaseVehicleCostValid = !(string.IsNullOrEmpty(vehiclePriceTextBox.Text));
            Boolean tradeInValueValid = !(string.IsNullOrEmpty(tradeInTextBox.Text));
            Boolean bothValid = (purchaseVehicleCostValid && tradeInValueValid);

            if (bothValid)
            {
                // This try/catch runs the code in the try loop. If it happens upon an error, it doesn't crash the program and instead moves to the catch block
                try
                {
                    purchaseVehicleCost = double.Parse(vehiclePriceTextBox.Text);
                    if (purchaseVehicleCost <= 0.0)
                    {
                        var purchaseCostError = new MessageDialog("Purchase cost must be greater than 0.");
                        await purchaseCostError.ShowAsync();
                        vehiclePriceTextBox.Focus(FocusState.Programmatic);
                        vehiclePriceTextBox.SelectAll();
                        return;
                    }

                } catch (Exception exception)
                {
                    // If it was not a double number, the catch block 'catches' the error and instead of crashing,
                    // the try block stops and it goes to the catch block to print an error explaining what happened
                    var purchaseCostError = new MessageDialog("You need to enter a number for the vehicle price.");
                    await purchaseCostError.ShowAsync();
                    vehiclePriceTextBox.Focus(FocusState.Programmatic);
                    vehiclePriceTextBox.SelectAll();
                    return;
                }

                // The try block of this try/catch block runs the code until the program produces an error
                try
                {
                    tradeInValue = double.Parse(tradeInTextBox.Text);

                    if (tradeInValue < 0.0)
                    {
                        var tradeInError = new MessageDialog("Trade-in value must be greater than or equal to 0.");
                        await tradeInError.ShowAsync();
                        tradeInTextBox.Focus(FocusState.Programmatic);
                        tradeInTextBox.SelectAll();
                        return;
                    }

                } catch (Exception exception)
                {
                    // If it was not a double number, the try block throws an error and the catch block 'catches'
                    // it and instead of crashing, it goes to the catch block to print an error explaining what happened
                    var tradeInError = new MessageDialog("You need to enter a number for the trade-in value");
                    await tradeInError.ShowAsync();
                    tradeInTextBox.Focus(FocusState.Programmatic);
                    tradeInTextBox.SelectAll();
                    return;
                }

            } else
            {
                if (!(purchaseVehicleCostValid))
                {
                    var emptyBoxError = new MessageDialog("Enter a number in the purchase cost box.");
                    await emptyBoxError.ShowAsync();
                    return;
                } else
                {
                    var emptyBoxError = new MessageDialog("Enter a number in the trade-in value box.");
                    await emptyBoxError.ShowAsync();
                    tradeInTextBox.Focus(FocusState.Programmatic);
                    tradeInTextBox.SelectAll();
                    return;
                }
                

            }

            if (!(purchaseVehicleCost > tradeInValue))
            {
                var priceError = new MessageDialog("The purchase cost must be greater than the trade-in-value");
                await priceError.ShowAsync();
                vehiclePriceTextBox.Focus(FocusState.Programmatic);
                vehiclePriceTextBox.SelectAll();
                return;
            }

            var vehicleWarranty = calcVehicleWarranty(purchaseVehicleCost);
            var extrasCost = calcOptionalExtras();
            var accidentInsurance = calcAccidentInsurance(purchaseVehicleCost, extrasCost);

            subAmount = (purchaseVehicleCost + vehicleWarranty + extrasCost + accidentInsurance) - tradeInValue;
            subAmountTextBox.Text = subAmount.ToString("C");

            gstAmount = subAmount * GST_RATE;
            gstAmountTextBox.Text = gstAmount.ToString("C");

            finalAmount = subAmount + gstAmount;
            finalAmountTextBox.Text = finalAmount.ToString("C");

            summaryTextBlock.Text = "Customer name: " + customerNameTextBox.Text +
                                    "\nPhone number: " + phoneTextBox.Text +
                                    "\nVehicle cost: " + purchaseVehicleCost.ToString("C") +
                                    "\nTrade-in amount: " + tradeInValue.ToString("C") +
                                    "\nWarranty cost: " + vehicleWarranty.ToString("C") +
                                    "\nOptional extras cost: " + extrasCost.ToString("C") +
                                    "\nInsurance cost: " + accidentInsurance.ToString("C") +
                                    "\nFinal amount: " + finalAmountTextBox.Text;
        }

        private double calcVehicleWarranty(double vehiclePrice)
        {
            String[] warrantyComboBoxValues = warrantyComboBox.SelectedValue.ToString().Split(' ');
            // defaulted to 1 year warranty (no extra cost)
            double warrantyPrice = 0.0;
            switch (warrantyComboBoxValues[0])
            {
                case "2":
                    // 5% of vehicle price
                    warrantyPrice = vehiclePrice * TWO_YEAR_WARRANTY;
                    break;
                case "3":
                    // 10% of vehicle price
                    warrantyPrice = vehiclePrice * THREE_YEAR_WARRANTY;
                    break;
                case "5":
                    // 20% of vehicle price
                    warrantyPrice = vehiclePrice * FIVE_YEAR_WARRANTY;
                    break;
            }

            return warrantyPrice;
        }

        private double calcOptionalExtras()
        {
            double extrasCost = 0.0;

            if (tintedWindowsCheckBox.IsChecked == true)
            {
                // If the tinted windows box is ticked, it will add its cost to the base double number
                extrasCost += TINTED_WINDOWS;
            }
            if (ducoProtectionCheckBox.IsChecked == true)
            {
                // If the duco protection box is ticked, it will add its cost to the extrasCost variable
                extrasCost += DUCO_PROTECTION;
            }
            if (floorMatsCheckBox.IsChecked == true)
            {
                // The same happens if the floor mats box is ticked
                extrasCost += FLOOR_MATS;
            }
            if (soundSystemCheckBox.IsChecked == true)
            {
                // And if the deluxe sound system box is ticked
                extrasCost += SOUND_SYSTEM;
            }

            return extrasCost;
        }

        private double calcAccidentInsurance(double vehiclePrice, double optionalExtras)
        {
            double accidentInsurance = 0.0;

            if (!(insuranceToggleSwitch.IsOn))
            {
                // If the toggle switch is off, it returns 0 and doesn't run the rest of the code
                return accidentInsurance;
            }

            if (youngerRadioButton.IsChecked == true)
            {
                // Because only one radio button can be checked, it checks to see if the under 25 button is checked
                // If it is, it multiplies the vehicle price and the optional extras price by 20%
                accidentInsurance = (vehiclePrice + optionalExtras) * UNDER_TWENTYFIVE_INSURANCE;
            } else
            {
                // If the under 25 button isn't checked, than it multiplies it by 10%
                accidentInsurance = (vehiclePrice + optionalExtras) * OVER_TWENTYFIVE_INSURANCE;
            }

            return accidentInsurance;
        }


        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            customerNameTextBox.Text = "";
            phoneTextBox.Text = "";
            vehiclePriceTextBox.Text = "";
            tradeInTextBox.Text = "";
            subAmountTextBox.Text = "$0.0";
            gstAmountTextBox.Text = "$0.0";
            finalAmountTextBox.Text = "$0.0";

            summaryTextBlock.Text = "";

            customerNameTextBox.IsEnabled = true;
            phoneTextBox.IsEnabled = true;
            customerNameTextBox.Focus(FocusState.Programmatic);

            warrantyComboBox.SelectedIndex = 0;
            tintedWindowsCheckBox.IsChecked = false;
            ducoProtectionCheckBox.IsChecked = false;
            floorMatsCheckBox.IsChecked = false;
            soundSystemCheckBox.IsChecked = false;
            insuranceToggleSwitch.IsOn = false;
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Boolean customerNameTextBoxValid = !(string.IsNullOrEmpty(customerNameTextBox.Text));
            Boolean phoneTextBoxValid = !(string.IsNullOrEmpty(phoneTextBox.Text));
            Boolean bothValid = (customerNameTextBoxValid && phoneTextBoxValid);

            if (bothValid)
            {
                customerNameTextBox.IsEnabled = false;
                phoneTextBox.IsEnabled = false;
                vehiclePriceTextBox.Focus(FocusState.Programmatic);
            } else
            {
                var customerNameErrorMessage = new MessageDialog("You need to enter the customer name and the phone number text box.");
                await customerNameErrorMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
            }
        }

        public void insuranceToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (insuranceToggleSwitch.IsOn)
            {
                youngerRadioButton.IsEnabled = true;
                youngerRadioButton.IsChecked = true;
                olderRadioButton.IsEnabled = true;
            } else
            {
                youngerRadioButton.IsEnabled = false;
                youngerRadioButton.IsChecked= false;
                olderRadioButton.IsEnabled = false;
                olderRadioButton.IsChecked = false;
            }
        }

        private void displayCustomersButton_Click(object sender, RoutedEventArgs e)
        {
            summaryTextBlock.Text = "";
            for (int i = 0; i < names.Length; i++)
            {
                summaryTextBlock.Text = summaryTextBlock.Text + "Name: " + names[i] + ", Ph: " + phoneNumbers[i] + "\n";
            }
        }

        private async void searchNameButton_Click(object sender, RoutedEventArgs e)
        {
            // to track position (index) of array
            int i = 0;
            // found will be true or false depending on if the name was found
            bool found = false;
            // search criteria entered by user to check against array
            // string has been capitalized to make searching case insensitive
            string criteria = customerNameTextBox.Text.ToUpper();

            // checks if search box is empty
            if (customerNameTextBox.Text == "")
            {
                var searchNameBlankError = new MessageDialog("Enter a name in the customer name box.");
                await searchNameBlankError.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
                return;
            }

            displayCustomersButton_Click(sender, e);

            // do sequential search of string array until match found or end of array reached
            // while not found and not end of array
            while (!found && i < names.Length)
            {
                // check if name is found
                if (criteria == names[i].ToUpper())
                {
                    found = true;
                } else
                {
                    // if no match found, move to next element in array
                    i++;
                }
            }
            // end of while loop

            // if name was found
            if (i < names.Length)
            {
                customerNameTextBox.Text = names[i];
                phoneTextBox.Text = phoneNumbers[i];
            } else
            {
                // if not found
                var searchNameError = new MessageDialog("Name does not exist.");
                await searchNameError.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
            }
        }

        private async void deleteNameButton_Click(object sender, RoutedEventArgs e)
        {
            // creates index to search through array with, boolean to check if criteria is found, and criteria to search against
            int i = 0;
            bool found = false;
            string criteria = customerNameTextBox.Text.ToUpper();
            // creates empty strings to store deleted variables to confirm with user
            string deletedName = "";
            string deletedNumber = "";

            // checks to see if customerNameTextBox is empty
            if (customerNameTextBox.Text == "")
            {
                var searchNameBlankError = new MessageDialog("Enter a name in the customer name box.");
                await searchNameBlankError.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
                return;
            }

            // displays customers in the summaryTextBlock
            displayCustomersButton_Click(sender, e);

            while (!found && i < names.Length)
            {
                // checks if criteria matches current array positions value (case insensitive)
                if (criteria == names[i].ToUpper())
                {
                    deletedName = names[i];
                    deletedNumber = phoneNumbers[i];
                    found = true;
                } else
                {
                    // if false, add 1 to counter
                    i++;
                }
            }
            // end while loop

            // if found
            if (i < names.Length)
            {
                // deletes only the value at the index that equals i
                names = names.Where((source, j) => j != i).ToArray();
                phoneNumbers = phoneNumbers.Where((source, j) => j != i).ToArray();

                // displays updated customers
                displayCustomersButton_Click(sender, e);

                // alerts the user with confirmation on deleted name
                var deletionSuccessful = new MessageDialog("Deletion successful.\nName deleted: " + deletedName + 
                    "\nPhone number deleted: " + deletedNumber + "\nCurrent array size: " + names.Length);
                await deletionSuccessful.ShowAsync();
            } else
            {
                //if not found
                var deletionUnsuccessful = new MessageDialog("Deletion unsuccessful. Name not found");
                await deletionUnsuccessful.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
            }            
        }

        private void displayMakesButton_Click(object sender, RoutedEventArgs e)
        {
            // sorts the vehicle makes array alphabetically
            Array.Sort(vehicleMakes);
            summaryTextBlock.Text = "";
            // adds each make to the summary text block
            for (int i = 0; i < vehicleMakes.Length; i++)
            {
                summaryTextBlock.Text = summaryTextBlock.Text + "Make: " + vehicleMakes[i] + "\n";
            }
        }

        private async void searchMakeButton_Click(object sender, RoutedEventArgs e)
        {
            string criteria = vehicleMakeTextBox.Text.ToUpper();
            // sorts and displays the vehicle makes
            displayMakesButton_Click(sender, e);

            if (vehicleMakeTextBox.Text == "")
            {
                var vehicleMakeBlankError = new MessageDialog("Enter a make into the vehicle makes text box.");
                await vehicleMakeBlankError.ShowAsync();
                vehicleMakeTextBox.Focus(FocusState.Programmatic);
                vehicleMakeTextBox.SelectAll();
                return;
            }

            // call the stringArrayBinarySearch method to find the criteria
            int foundPos = stringArrayBinarySearch(vehicleMakes, criteria);

            if (foundPos == -1)
            {
                var makeNotFoundError = new MessageDialog(criteria + " not found.");
                await makeNotFoundError.ShowAsync();
                vehicleMakeTextBox.Focus(FocusState.Programmatic);
                vehicleMakeTextBox.SelectAll();
            } else
            {
                var makeFound = new MessageDialog("Found " + criteria + " at index " + foundPos);
                await makeFound.ShowAsync();
                displayMakesButton_Click(sender, e);
            }
        }

        private int stringArrayBinarySearch(string[] data, string item)
        {
            int min = 0;
            int max = data.Length - 1;
            int mid;
            item = item.ToUpper();

            do
            {
                mid = (min + max) / 2;
                // if the item is found return the index mid
                if (data[mid].ToUpper() == item)
                {
                    return mid;
                }

                // check if the item wanted is in the top half of the search
                if (item.CompareTo(data[mid].ToUpper()) > 0)
                {
                    // set the min part of the search to the mid + 1
                    min = mid + 1;
                } else
                {
                    // otherwise the item must be in the lower half of the search, set max to the mid-1
                    max = mid - 1;
                }

            } while (min <= max);

            // if not found, return -1
            return -1;
        }

        private async void insertMakeButton_Click(Object sender, RoutedEventArgs e)
        {
            if (vehicleMakeTextBox.Text == "")
            {
                var vehicleMakeBlankError = new MessageDialog("Enter a make into the vehicle makes text box.");
                await vehicleMakeBlankError.ShowAsync();
                vehicleMakeTextBox.Focus(FocusState.Programmatic);
                vehicleMakeTextBox.SelectAll();
                return;
            }

            if (vehicleMakes.Contains(vehicleMakeTextBox.Text.ToUpper()))
            {
                var containsMakeError = new MessageDialog(vehicleMakeTextBox.Text + " already exists in the vehicle makes list.");
                await containsMakeError.ShowAsync();
                vehicleMakeTextBox.Focus(FocusState.Programmatic);
                vehicleMakeTextBox.SelectAll();
                return;
            }

            string[] newArray = new string[vehicleMakes.Length + 1];
            for (int i = 0; i < vehicleMakes.Length; i++)
            {
                newArray[i] = vehicleMakes[i];
            }
            newArray[vehicleMakes.Length] = vehicleMakeTextBox.Text;
            vehicleMakes = newArray;

            displayMakesButton_Click(sender, e);
        }
    }
}
