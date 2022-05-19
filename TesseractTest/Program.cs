// See https://aka.ms/new-console-template for more information
using System.Drawing;
using Tesseract;

Console.WriteLine("Hello, World!");
string content = "1 page\n";
RotateClockwise();



Console.WriteLine("Text (GetText): \r\n{0}", GetText());


static string GetText()
{
    string text;
    using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
    {
        using (var img = Pix.LoadFromFile(@"C:\Users\Maxim_Soolkovskiy\Downloads\image.jpg"))
        {
            using (var page = engine.Process(img))
            {
                text = page.GetText();
                
            }
        }
    }

    return text;
}

static void RotateClockwise()
{
    Bitmap bitmap;
    bitmap = (Bitmap)Bitmap.FromFile(@"C:\Users\Maxim_Soolkovskiy\Downloads\image.jpg");
    if (bitmap != null)
    {
        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        bitmap.Save(@"C:\Users\Maxim_Soolkovskiy\Downloads\image.jpg");
    }
}

/// <summary>
/// Gets the current Angle of a text image
/// </summary>
static Angles CalcCurrentAngle(string content)
{
    if (GetText().Contains(content)) { return Angles.Zero; }

    var actualAngle = Angles.Zero;

    while (!GetText().Contains(content))
    {
        Console.WriteLine(GetText());
        actualAngle--;
        RotateClockwise();
    }

    return actualAngle;
}

/// <summary>
/// Rotates the image till it is at 0 Angle
/// </summary>
static void StraightenImage(string content)
{
    while(!GetText().Contains(content))
    {
        RotateClockwise();
    }
}

/// <summary>
/// Rotates the image till its source angle gets to required value
/// </summary>
static void RestoreSourceAngle(Angles actualAngle)
{
    for (int i = 0; i < (int)actualAngle; i++)
    {
        RotateClockwise();
    }
}

enum Angles
{
    Ninety,
    OneEighty,
    TwoSeventy,
    Zero
}

