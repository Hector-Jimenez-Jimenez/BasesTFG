using PrimeraApp.Pages;

namespace PrimeraApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute(nameof(CocheDetail), typeof(CocheDetail));
            InitializeComponent();
        }
    }
}
