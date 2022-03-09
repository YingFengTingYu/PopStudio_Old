using PopStudio.MAUI.Languages;

namespace PopStudio.MAUI;

public partial class Page_AD : ContentPage
{
	public Page_AD()
	{
		InitializeComponent();
		ad.Text = MAUIStr.Obj.AD_Title;
		image.Source = ImageSource.FromStream(() => new MemoryStream(ResourceAD.ImageAD1));
	}

    private async void image_Clicked(object sender, EventArgs e)
    {
		await Browser.OpenAsync(ResourceAD.AD1, BrowserLaunchMode.SystemPreferred);
	}
}