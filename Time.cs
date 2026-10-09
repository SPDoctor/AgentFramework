using System.ComponentModel;
using System;

namespace Plugins;

public class TimePlugin
{
  [Description("Get the current date and time")]
  public static DateTime Time()
  {
    return DateTime.UtcNow;
  }

  [Description("get the current day of the week")]
  public static System.DayOfWeek DayOfWeek()
  {
    return DateTime.UtcNow.DayOfWeek;
  }
}