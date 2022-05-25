namespace PopStudio.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("MiSans-Normal.ttf", "MiSansNormal");
                });

            return builder.Build();
        }
    }
}