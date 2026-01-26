using System.Globalization;
using LunarCalendar.Core.Models;

namespace LunarCalendar.MobileApp.Helpers;

/// <summary>
/// Helper class for formatting sexagenary cycle (stem-branch) information
/// Provides consistent formatting across all pages (Calendar, Holiday Detail, Year Holidays)
/// </summary>
public static class SexagenaryFormatterHelper
{
    /// <summary>
    /// Format year stem-branch based on current language
    /// Vietnamese: "Ất Tỵ" (just stem-branch)
    /// English: "Yi Si (Snake)" (stem-branch with animal name)
    /// Chinese: "乙巳" (Chinese characters)
    /// </summary>
    public static string FormatYearStemBranch(HeavenlyStem stem, EarthlyBranch branch)
    {
        var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        
        if (currentCulture == "vi")
        {
            // Vietnamese: Just stem + branch
            var stemName = GetVietnameseStemName(stem);
            var branchName = GetVietnameseBranchName(branch);
            return $"{stemName} {branchName}";
        }
        else if (currentCulture == "zh")
        {
            // Chinese: Characters for stem + branch
            return GetChineseStemName(stem) + GetChineseBranchName(branch);
        }
        else
        {
            // English: Stem + Branch (Animal name)
            var animalName = GetAnimalNameFromBranch(branch);
            return $"{stem} {branch} ({animalName})";
        }
    }

    /// <summary>
    /// Get Vietnamese name for heavenly stem
    /// </summary>
    private static string GetVietnameseStemName(HeavenlyStem stem)
    {
        return stem switch
        {
            HeavenlyStem.Jia => "Giáp",
            HeavenlyStem.Yi => "Ất",
            HeavenlyStem.Bing => "Bính",
            HeavenlyStem.Ding => "Đinh",
            HeavenlyStem.Wu => "Mậu",
            HeavenlyStem.Ji => "Kỷ",
            HeavenlyStem.Geng => "Canh",
            HeavenlyStem.Xin => "Tân",
            HeavenlyStem.Ren => "Nhâm",
            HeavenlyStem.Gui => "Quý",
            _ => stem.ToString()
        };
    }

    /// <summary>
    /// Get Vietnamese name for earthly branch
    /// </summary>
    private static string GetVietnameseBranchName(EarthlyBranch branch)
    {
        return branch switch
        {
            EarthlyBranch.Zi => "Tý",
            EarthlyBranch.Chou => "Sửu",
            EarthlyBranch.Yin => "Dần",
            EarthlyBranch.Mao => "Mão",
            EarthlyBranch.Chen => "Thìn",
            EarthlyBranch.Si => "Tỵ",
            EarthlyBranch.Wu => "Ngọ",
            EarthlyBranch.Wei => "Mùi",
            EarthlyBranch.Shen => "Thân",
            EarthlyBranch.You => "Dậu",
            EarthlyBranch.Xu => "Tuất",
            EarthlyBranch.Hai => "Hợi",
            _ => branch.ToString()
        };
    }

    /// <summary>
    /// Get Chinese character for heavenly stem
    /// </summary>
    private static string GetChineseStemName(HeavenlyStem stem)
    {
        return stem switch
        {
            HeavenlyStem.Jia => "甲",
            HeavenlyStem.Yi => "乙",
            HeavenlyStem.Bing => "丙",
            HeavenlyStem.Ding => "丁",
            HeavenlyStem.Wu => "戊",
            HeavenlyStem.Ji => "己",
            HeavenlyStem.Geng => "庚",
            HeavenlyStem.Xin => "辛",
            HeavenlyStem.Ren => "壬",
            HeavenlyStem.Gui => "癸",
            _ => stem.ToString()
        };
    }

    /// <summary>
    /// Get Chinese character for earthly branch
    /// </summary>
    private static string GetChineseBranchName(EarthlyBranch branch)
    {
        return branch switch
        {
            EarthlyBranch.Zi => "子",
            EarthlyBranch.Chou => "丑",
            EarthlyBranch.Yin => "寅",
            EarthlyBranch.Mao => "卯",
            EarthlyBranch.Chen => "辰",
            EarthlyBranch.Si => "巳",
            EarthlyBranch.Wu => "午",
            EarthlyBranch.Wei => "未",
            EarthlyBranch.Shen => "申",
            EarthlyBranch.You => "酉",
            EarthlyBranch.Xu => "戌",
            EarthlyBranch.Hai => "亥",
            _ => branch.ToString()
        };
    }

    /// <summary>
    /// Get animal name from earthly branch (English name)
    /// </summary>
    private static string GetAnimalNameFromBranch(EarthlyBranch branch)
    {
        return branch switch
        {
            EarthlyBranch.Zi => "Rat",
            EarthlyBranch.Chou => "Ox",
            EarthlyBranch.Yin => "Tiger",
            EarthlyBranch.Mao => "Rabbit",
            EarthlyBranch.Chen => "Dragon",
            EarthlyBranch.Si => "Snake",
            EarthlyBranch.Wu => "Horse",
            EarthlyBranch.Wei => "Goat",
            EarthlyBranch.Shen => "Monkey",
            EarthlyBranch.You => "Rooster",
            EarthlyBranch.Xu => "Dog",
            EarthlyBranch.Hai => "Pig",
            _ => "Snake" // Default fallback
        };
    }

    /// <summary>
    /// Get animal name from string (e.g., "Snake" -> localized name via LocalizationHelper)
    /// This is for backward compatibility with existing AnimalSign string properties
    /// </summary>
    public static string GetAnimalNameFromString(string animalSign)
    {
        // Return as-is, the caller should use LocalizationHelper.GetLocalizedAnimalSign
        return animalSign;
    }
}
