using System.Text;

namespace CC.HandbookService.Infrastructure.Services;

public static class CsvCellHelper
{
    public static string GetCellName(int rowIndex, int columnIndex)
    {
        const int enLettersCount = 26;

        var stack = new Stack<char>();
        var sb = new StringBuilder();

        while (columnIndex > 0)
        {
            columnIndex--;
            
            var remainder = columnIndex % enLettersCount;
            var letter = (char)('A' + remainder);
            stack.Push(letter);

            columnIndex /= enLettersCount;
        }
        
        while (stack.Count > 0)
            sb.Append(stack.Pop());
        
        sb.Append(rowIndex);
        
        return sb.ToString();
    }
}