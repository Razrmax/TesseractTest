// See https://aka.ms/new-console-template for more information
using System.Drawing;
using Tesseract;

Console.WriteLine("Hello, World!");
RotateClockwise();
//RotateClockwise();
string content = "1 page";
string filePath = @"C:\Users\Maxim_Soolkovskiy\Downloads\image.jpg";

Console.WriteLine("Text (GetText): \r\n{0}", GetText(filePath));
RotateClockwise();

var startAngle = CalcCurrentAngle(content, filePath);
RestoreSourceAngle(startAngle);
StraightenImage(content, filePath);
Console.WriteLine("Text (GetText): \r\n{0}", GetText(filePath));

/// <summary>
/// Gets the text from the specified image file
/// </summary>
static string GetText(string filePath)
{
    string text;
    using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
    {
        using (var img = Pix.LoadFromFile(filePath))
        {
            using (var page = engine.Process(img))
            {
                text = page.GetText();
            }
        }
    }

    return text;
}

/// <summary>
/// Rotates the image file clockwise
/// </summary>
static void RotateClockwise()
{
    using (var bitmap = (Bitmap)Image.FromFile(@"C:\Users\Maxim_Soolkovskiy\Downloads\image.jpg"))
    {
        if (bitmap != null)
        {
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            bitmap.Save(@"C:\Users\Maxim_Soolkovskiy\Downloads\image.jpg");
        } 
    }
}

/// <summary>
/// Gets the current Angle of a text image
/// </summary>
static Angles CalcCurrentAngle(string content, string filePath)
{
    if (GetText(filePath).Contains(content)) { return Angles.Zero; }
    
    var currentAngle = Angles.TwoSeventy;

    for (Angles deltaAngle = Angles.Zero; deltaAngle <= Angles.TwoSeventy; deltaAngle++)
    {
        if (GetText(filePath).Contains(content))
        {
            currentAngle -= deltaAngle;
            break;
        }

        RotateClockwise();
    }

    return currentAngle;
}

/// <summary>
/// Rotates the image till it is at 0 Angle
/// </summary>
static void StraightenImage(string content, string filePath)
{
    while(!GetText(filePath).Contains(content))
    {
        RotateClockwise();
    }
}

/// <summary>
/// Rotates the image till its source angle gets to required value
/// </summary>
static void RestoreSourceAngle(Angles actualAngle)
{
    for (Angles angle = Angles.Zero; angle < actualAngle; angle++)
    {
        RotateClockwise();
    }
}

enum Angles
{
    Zero = 0,
    Ninety = 1,
    OneEighty = 2,
    TwoSeventy = 3
}

