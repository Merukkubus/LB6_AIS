using LB6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB6
{
    internal class Program
    {
        static async Task Main()
        {
            await Parser.Parse(@"https://2droida.ru/collection/noutbuki-xiaomi");
            var dbcon = new SiteParseEntities();
            foreach (var n in dbcon.NoteBook)
                Console.WriteLine("{0}\n\t{1}\n\t{2}\n\t{3}", n.Name, n.Brand, n.Frequency, n.Resolution, n.Weight);
            Console.ReadLine();
        }
    }
}
