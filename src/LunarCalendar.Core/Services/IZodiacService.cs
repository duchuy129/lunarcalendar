using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

public interface IZodiacService
{
    ZodiacAnimal GetAnimalForDate(DateTime gregorianDate);
    ZodiacAnimal GetAnimalForLunarYear(int lunarYear);
    ElementalAnimal GetElementalAnimalForLunarYear(int lunarYear);
    ElementalAnimal GetElementalAnimalForDate(DateTime gregorianDate);
}
