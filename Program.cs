using System;
using System.IO;
using Xceed.Words.NET;

class Program{
    static void Main(string[] args)
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
        string filePath = @"Animus\Animus_Jedno_życie\Animus_Jedno_życie_tekst.docx";

        string pattern = @"Tytuł Polski LUB : (.*?) Tytuł Oryginalny \(jeśli jest\):";


        using(DocX document  = DocX.Load(filePath))
        {
            string text = document.Text;
            Console.WriteLine(text);   
        }
    }
}
