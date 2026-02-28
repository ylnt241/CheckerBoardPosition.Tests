
using System.Globalization;

namespace CheckerBoardPosition.Tests;

public class CheckerBoardPositionsTests
{
    public static readonly TheoryData<CultureInfo> Cultures = [
        CultureInfo.InvariantCulture,
        CultureInfo.CurrentCulture,
        new CultureInfo("ru-RU"),
        new CultureInfo("es-ES")
    ];
    public static IEnumerable<object[]> AllowedNumbers()
    {
        for (byte i = 1; i <= 8; i++)
        for (byte j = 1; j <= 8; j++)
            yield return [i, j];
    }

    public static IEnumerable<object[]> ErrorNumbers()
    {
        for (byte i = 9; i < 255; i++)
        for (byte j = 9; j < 255; j++)
            yield return [i, j];
    }

    [Theory]
    [MemberData(nameof(AllowedNumbers))]
    public void SetCoordinates_ValidNumbers_Pass(byte x, byte y)
    {
        //Act
        var fake = new ChessExample.CheckerBoardPosition(x,y);
        //Assert
        Assert.InRange(fake.X, 1, 8);
        Assert.InRange(fake.Y, 1, 8);
        Assert.Equal(x, fake.X);
        Assert.Equal(y, fake.Y);
    }

    [Theory]
    [MemberData(nameof(ErrorNumbers))]
    public void SetCoordinates_InvalidNumbers_Fail(byte x, byte y)
    {
        //Assert
        Assert.ThrowsAny<Exception>(() => new ChessExample.CheckerBoardPosition(x, y));
    }

    [Theory]
    [MemberData(nameof(AllowedNumbers))]
    public void ConvertToChessNotation_ValidNumbers_Pass(byte x, byte y)
    {
        //Arrange
        var fake =  new ChessExample.CheckerBoardPosition(x, y);
        //Assert
        Assert.Equal((char)('@' + fake.X), fake.XLetter);
        Assert.Equal($"{fake.XLetter}{fake.Y}", fake.ToString());
    }

    [Fact]
    public void TryParse_Null_ReturnNull()
    {
        //Act
        ChessExample.CheckerBoardPosition.TryParse(null, CultureInfo.InvariantCulture, out ChessExample.CheckerBoardPosition? pos);
        Assert.Null(pos);
    }

    [Theory]
    [MemberData(nameof(Cultures))]
    public void Parse_VariousCultures_Pass(CultureInfo culture)
    {
        //Arrange
        string? s = "A1";
        
        //Act
        var result = ChessExample.CheckerBoardPosition.Parse(s, culture);
        
        //Assert
        Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(Cultures))]
    public void TryParse_VariousCultures_Pass(CultureInfo culture)
    {
        //Arrange
        var fake = new ChessExample.CheckerBoardPosition(1,1);
        string? s = fake.ToString();
        
        //Act
        bool result =  ChessExample.CheckerBoardPosition.TryParse(s, culture, out var pos);
        Assert.True(result);
        Assert.NotNull(pos);
    }
}