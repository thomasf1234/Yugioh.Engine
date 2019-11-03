using System;

namespace Yugioh.Engine.Models
{
  public class World
  {
    public static bool IsMorning()
    {
      return DateTime.Now.Hour < 12;
    }

    public static bool IsLunch()
    {
      return DateTime.Now.Hour == 12;
    }

    public static bool IsAfternoon()
    {
      return DateTime.Now.Hour > 12 && DateTime.Now.Hour < 18;
    }

    public static bool IsEvening()
    {
      return DateTime.Now.Hour >= 18;
    }

    public static bool IsNight()
    {
      return DateTime.Now.Hour >= 22 || DateTime.Now.Hour < 4 ;
    }
  }
}
