using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pedalSHIELD_Uploader
{
    /// <summary>
    /// Interaktionslogik für InvisibleButton.xaml
    /// </summary>
    public partial class InvisibleButton : UserControl
    {
        public InvisibleButton()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler CustomClick;

        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            CustomClick(this, new RoutedEventArgs());
        }
    }
}
