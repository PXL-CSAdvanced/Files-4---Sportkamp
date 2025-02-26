using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;

namespace Files_4___Sportkamp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SportkampLeden : Window
    {
        private List<Lid> _leden;
        
        public SportkampLeden()
        {
            InitializeComponent();

            try
            {
                _leden = Lid.LeesLeden(@"..\..\..\Bestanden\LedenSportkampen.txt");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Kan bestand LedenSportkampen.txt niet vinden!", 
                    "Fout", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return;
            }

            detailsTextBox.Text = Lid.ToonLeden(_leden);
            weekOverviewTextBox.Text = Lid.ToonWeekOverzicht();
            sportOverviewTextBox.Text = Lid.ToonSporttakOverzicht();
        }
    }
}
