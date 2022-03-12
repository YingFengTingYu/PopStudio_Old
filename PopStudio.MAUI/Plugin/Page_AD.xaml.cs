using PopStudio.MAUI.Languages;

namespace PopStudio.MAUI;

public partial class Page_AD : ContentPage
{
	string url;
	byte[] img;

	public Page_AD()
	{
		InitializeComponent();
		ad.Text = MAUIStr.Obj.AD_Title;
		int t = new Random().Next(1, 4);
		switch (t)
        {
			case 1:
				url = ResourceAD.AD1;
				img = ResourceAD.ImageAD1;
				break;
			case 2:
				url = ResourceAD.AD2;
				img = ResourceAD.ImageAD2;
				break;
			case 3:
				url = ResourceAD.AD3;
				img = ResourceAD.ImageAD3;
				break;
			default:
				return;
		}
		image.Source = ImageSource.FromStream(() => new MemoryStream(img));
	}

    private async void image_Clicked(object sender, EventArgs e)
    {
		await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
	}
}