namespace PopStudio.MAUI
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("确定要退出吗", "确定要退出吗？", "确定", "取消")) Environment.Exit(0);
            });
            return true;
        }
    }
}