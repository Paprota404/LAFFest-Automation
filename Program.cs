using System;
using System.IO;
using Xceed.Words.NET;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
        string filePath = @"filmy\Animus\Animus_Jedno_życie\Animus_Jedno_życie_tekst.docx";

        string tytulPattern = @"TYTUŁ POLSKI LUB : (.*?)TYTUŁ ORYGINALNY";
        string rezyseriaPattern = @"REŻYSER/KA: (.*?)PRODUKCJA";
        string produkcjaPattern = @"ROK, CZAS TRWANIA: (.*?), (\d{4})";
        string czasPattern = @", (\d{1,})’";
        string opisPattern = @"OPIS FILMU:(.*?)(?=NAGRODY|$)";
        string nagrodyPattern = @"NAGRODY I FESTIWALE:(.*)";

        RegexOptions options = RegexOptions.Singleline | RegexOptions.IgnoreCase;

        using (DocX document = DocX.Load(filePath))
        {
            string text = document.Text;
            Console.WriteLine(text);
            Match tytulMatch = Regex.Match(text, tytulPattern, options);
            Match rezyseriaMatch = Regex.Match(text, rezyseriaPattern, options);
            Match produkcjaMatch = Regex.Match(text, produkcjaPattern, options);
            Match czasMatch = Regex.Match(text, czasPattern, options);
            Match opisMatch = Regex.Match(text, opisPattern, options);
            Match nagrodyMatch = Regex.Match(text, nagrodyPattern, options);
            string tytul = tytulMatch.Groups[1].Value.Trim();
            Console.WriteLine(tytul);


            string rezyseria = rezyseriaMatch.Groups[1].Value.Trim();
            Console.WriteLine(rezyseria);

            string produkcja = $"{produkcjaMatch.Groups[1].Value.Trim()}, {produkcjaMatch.Groups[2].Value.Trim()}";
            Console.WriteLine(produkcja);

            string czas = czasMatch.Groups[1].Value.Trim();
            int totalMinutes = int.Parse(czas); 

            int hours = totalMinutes / 60; 
            int minutes = totalMinutes % 60; 

         
            string formattedTime = $"{hours} H {minutes} Min";

            Console.WriteLine(formattedTime);
            
            string opis = opisMatch.Groups[1].Value.Trim();
            Console.WriteLine(opis);

            string nagrody = nagrodyMatch.Groups[1].Value.Trim();
            Console.WriteLine(nagrody);

            await CreateWordpressPost();
        }
    }

    static async Task CreateWordpressPost(){
        string url = "https://laffest.pl/wp-json/";

        using(HttpClient client = new HttpClient()){
            try{
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();


                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject(responseBody);
                Console.Write(jsonResponse);
            }
            catch(HttpRequestException e){
                Console.WriteLine("Request error:" + e.Message);
            }
        }
    }
}
