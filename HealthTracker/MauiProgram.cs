using HealthTracker.Services;
using HealthTracker.ViewModels;
using HealthTracker.Views;

namespace HealthTracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // --- Dependency Injection Registration ---

        // Register Services (as singletons so there's only one instance)
        builder.Services.AddSingleton<DatabaseService>();

        // Register ViewModels
        builder.Services.AddTransient<MentalHealthViewModel>();
        builder.Services.AddTransient<PhysicalHealthViewModel>();
        builder.Services.AddTransient<EmotionalHealthViewModel>();
        builder.Services.AddTransient<JournalViewModel>();
        builder.Services.AddTransient<PastJournalsViewModel>();
        builder.Services.AddTransient<TodoViewModel>();
        builder.Services.AddTransient<TodoItemDetailViewModel>();

        // Register Views
        builder.Services.AddTransient<MentalHealthPage>();
        builder.Services.AddTransient<PhysicalHealthPage>();
        builder.Services.AddTransient<EmotionalHealthPage>();
        builder.Services.AddTransient<JournalPage>();
        builder.Services.AddTransient<PastJournalsPage>();
        builder.Services.AddTransient<TodoPage>();
        builder.Services.AddTransient<TodoItemDetailPage>();


        return builder.Build();
    }
}