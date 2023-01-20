using System.Net;
using System.Xml.Serialization;
using CbarApi.Models;

class Program
{
    static async Task Main()
    {
        string url = "https://www.cbar.az/currencies/19.01.2023.xml";
        ValCurs valCurs;
        
        
        string xmlXesult = await GetAsync(url);
        // Console.WriteLine(xmlXesult);
        XmlSerializer serializer = new XmlSerializer(typeof(ValCurs));
        using (StringReader reader = new StringReader(xmlXesult))
        {
            valCurs = (ValCurs)serializer.Deserialize(reader);
        }

        foreach (var valueType in valCurs.ValType )
        {
            Console.WriteLine("#########################################");
            Console.WriteLine(valueType.Type);
            Console.WriteLine("#########################################\n");

            foreach (var BankMaterial in  valueType.Valute)
            {
                Console.WriteLine("Nominal: "+BankMaterial.Nominal);
                Console.WriteLine("Name: "+BankMaterial.Name);
                Console.WriteLine("Value: "+BankMaterial.Value);

                Console.WriteLine("*****************************************************");
            }
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