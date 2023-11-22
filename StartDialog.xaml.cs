using System.Windows;
using System.Windows.Controls.Primitives;

namespace N_Gewinnt
{
    /// <summary>
    /// Interaktionslogik für StartDialog.xaml
    /// </summary>
    public partial class StartDialog : Window
    {
        public StartDialog()
        {
            InitializeComponent();
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txt_spalten.Text, out int spalten) &&
                int.TryParse(txt_reihen.Text, out int reihen) &&
                int.TryParse(txt_n.Text, out int n))
            {
                if (spalten > 0 && reihen <=4 && reihen > 0 && n > 0)
                {
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Bitte geben Sie gültige Werte ein!", "Falsche Werte", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Ungültige Eingabe. Bitte überprüfen Sie Ihre Eingaben.", "Ungültige Eingabe!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void txt_spalten_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_spalten.Text = null;
        }
        private void txt_n_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_n.Text = null;
        }
        private void txt_reihen_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_reihen.Text = null;
        }


        public int GetColumns()
        {
            return int.Parse(txt_spalten.Text);
        }
        public int GetRows()
        {
            return int.Parse(txt_reihen.Text);
        }
        public int GetN()
        {
            return int.Parse(txt_n.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btn_start.Focus();
        }
    }
}
