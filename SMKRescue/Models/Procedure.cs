namespace SMKRescue.Models;

public class Procedure
{
    public Procedure(string fileName, string selector)
    {
        FileName = fileName;
        Selector = selector;
    }

    public string FileName { get; set; }
    public string Selector { get; set; }
}