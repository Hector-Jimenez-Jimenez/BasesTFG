using CommunityToolkit.Maui.Views;
using PrimeraApp.Viewmodels.Popups;

namespace PrimeraApp.Pages.Popups;

public partial class PhotoPopUp : Popup
{
	public PhotoPopUp(PhotoViewModel _viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel;
	}
}