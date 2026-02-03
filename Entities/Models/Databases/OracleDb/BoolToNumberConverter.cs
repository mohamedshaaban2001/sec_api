using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class BoolToNumberConverter : ValueConverter<bool, int>
{
    public BoolToNumberConverter() : base(v => v ? 1 : 0, v => v == 1)
    { }
}

