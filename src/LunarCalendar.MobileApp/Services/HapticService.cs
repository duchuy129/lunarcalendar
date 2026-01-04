using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Services;

public class HapticService : IHapticService
{
    private readonly ILogService _logService;

    public HapticService(ILogService logService)
    {
        _logService = logService;
    }

    public void PerformClick()
    {
        if (SettingsViewModel.GetEnableHapticFeedback())
        {
            try
            {
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            }
            catch (Exception ex)
            {
                _logService.LogWarning("Haptic click feedback failed - non-critical", "HapticService.PerformClick");
            }
        }
    }

    public void PerformSelection()
    {
        if (SettingsViewModel.GetEnableHapticFeedback())
        {
            try
            {
                HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            }
            catch (Exception ex)
            {
                _logService.LogWarning("Haptic selection feedback failed - non-critical", "HapticService.PerformSelection");
            }
        }
    }
}
