using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    ///     
    /// 03/28/2022
    /// assignment CarSales2
    public sealed partial class CarSales2 : Page
    {
        public CarSales2()
        {
            this.InitializeComponent();
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            const decimal GST_RATE = 0.1m;
            decimal subAmount;
            decimal gstAmount;
            decimal finalAmount;
            subAmount = decimal.Parse(vehiclePriceTextBox.Text) - decimal.Parse(tradeInTextBox.Text);
            subAmountTextBox.Text = subAmount.ToString("C");
            gstAmount = subAmount * GST_RATE;
            gstAmountTextBox.Text = gstAmount.ToString("C");
            finalAmount = subAmount + gstAmount;
            finalAmountTextBox.Text = finalAmount.ToString("C");
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            phoneTextBox.Text = "";
            customerNameTextBox.Text = "";
            vehiclePriceTextBox.Text = "";
            tradeInTextBox.Text = "";
            subAmountTextBox.Text = "";
            gstAmountTextBox.Text = "";
            finalAmountTextBox.Text = "";
            phoneTextBox.IsEnabled = true;
            customerNameTextBox.IsEnabled = true;
            customerNameTextBox.Focus(FocusState.Programmatic);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            phoneTextBox.IsEnabled = false;
            customerNameTextBox.IsEnabled = false;
            vehiclePriceTextBox.Focus(FocusState.Programmatic);
        }
    }
}
