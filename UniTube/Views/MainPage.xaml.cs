using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniTube.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            TitleBar.Height         = coreTitleBar.Height;
            PaneTitleBar.Height     = coreTitleBar.Height;
            ContentTitleBar.Height  = coreTitleBar.Height;
            Window.Current.SetTitleBar(TitleBar);

            coreTitleBar.LayoutMetricsChanged   += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged       += CoreTitleBar_IsVisibleChanged;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Visibility         = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            PaneTitleBar.Visibility     = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            ContentTitleBar.Visibility  = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Height         = sender.Height;
            PaneTitleBar.Height     = sender.Height;
            ContentTitleBar.Height  = sender.Height;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsSwipeablePaneOpen = !splitView.IsSwipeablePaneOpen;
        }
    }
}
