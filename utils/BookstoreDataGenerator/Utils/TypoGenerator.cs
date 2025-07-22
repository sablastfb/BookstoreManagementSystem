using System.Text;

namespace BookstoreDataGenerator.Utils;

public static class TypoGenerator
{
  private static readonly Random _random = new();
    
  public static string AddTypo(string str, float strength = 0.1f)
  {
    if (string.IsNullOrEmpty(str) || strength <= 0f)
      return str;

    int typoCount = Math.Max(1, (int)(str.Length * strength));
        
    var result = new StringBuilder(str);
        
    for (int i = 0; i < typoCount; i++)
    {
      if (result.Length == 0) break;
            
      int operation = _random.Next(0, 4);
      int position = _random.Next(0, result.Length);

      switch (operation)
      {
        case 0: // Insert random character
          char insertChar = (char)_random.Next('a', 'z' + 1);
          result.Insert(position, insertChar);
          break;
                    
        case 1 when result.Length > 1: // Delete character (only if length > 1)
          result.Remove(position, 1);
          break;
                    
        case 2 when position < result.Length - 1: // Swap adjacent characters
          char current = result[position];
          char next = result[position + 1];
          result[position] = next;
          result[position + 1] = current;
          break;
                    
        case 3: // Replace character
          char replaceChar = (char)_random.Next('a', 'z' + 1);
          result[position] = replaceChar;
          break;
      }
    }

    return result.ToString();
  }
}
