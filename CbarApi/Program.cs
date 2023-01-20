using System.Net;
using System.Xml.Serialization;
using CbarApi.Models;

class Program
{
    static async Task Main()
    {
        string url = $"https://www.cbar.az/currencies/{DateTime.Now.Date.ToString("dd.MM.yyyy")}.xml";
        List<string> valuteCodes = new List<string>() { "USD", "EUR", "RUB", "TRY" };
        ValCurs valCurs;

        // string xmlXesult = await GetAsync(url);
        // // Console.WriteLine(xmlXesult);
        // XmlSerializer serializer = new XmlSerializer(typeof(ValCurs));
        // using (StringReader reader = new StringReader(xmlXesult))
        // {
        //     valCurs = (ValCurs)serializer.Deserialize(reader);
        // }

        // foreach (var valueType in valCurs.ValType)
        // {
        //     Console.WriteLine("#########################################");
        //     Console.WriteLine(valueType.Type);
        //     Console.WriteLine("#########################################\n");
        //
        //     foreach (var BankMaterial in valueType.Valute)
        //     {
        //         Console.WriteLine("Nominal: " + BankMaterial.Nominal);
        //         Console.WriteLine("Name: " + BankMaterial.Name);
        //         Console.WriteLine("Value: " + BankMaterial.Value);
        //
        //         Console.WriteLine("*****************************************************");
        //     }
        // }
        string xmlXesult = await GetAsync(url);
        valCurs = Deseralize(xmlXesult);

        List<Valute> valutes = SelectData(valCurs, valuteCodes);
        foreach (var valute in valutes)
        {
            Console.WriteLine("Nominal: " + valute.Nominal);
            Console.WriteLine("Name: " + valute.Name);
            Console.WriteLine("Value: " + valute.Value);
        }
    }

    public static List<Valute> SelectData(ValCurs valCurs, List<string> valuteCodes) => valCurs.ValType
        .Where(p => p.Type == "Xarici valyutalar")
        .FirstOrDefault()
        .Valute.Where(p => valuteCodes.Contains(p.Code))
        .ToList();


    public static ValCurs Deseralize(string xml)
    {
        // Console.WriteLine(xmlXesult);
        XmlSerializer serializer = new XmlSerializer(typeof(ValCurs));
        using (StringReader reader = new StringReader(xml))
        {
            return (ValCurs)serializer.Deserialize(reader);
        }
    }

    public static async Task<string> GetAsync(string uri)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            return await reader.ReadToEndAsync();
        }
    }
}