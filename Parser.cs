using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using LB6.Models;

namespace LB6
{
    class Parser
    {
        public static async Task Parse(string url)
        {
            var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            var doc = await context.OpenAsync(url);
            var aList = doc.QuerySelectorAll("div.product-preview__title a").Select(elem => doc.Origin + elem.GetAttribute("href"));
            NoteBook nb;
            IElement value;
            foreach ( var a in aList )
            {
                Console.WriteLine(a);
                doc = await context.OpenAsync(a);
                nb = new NoteBook();
                nb.Id = Guid.NewGuid();
                value = doc.QuerySelector("div.product__area-title > h1.product__title");
                nb.Name = value.TextContent;
                foreach (var prop in doc.QuerySelectorAll("div#tab-characteristics div.property")
                    .Select(prop => new KeyValuePair<string, string>(prop.QuerySelector("div.property__name").TextContent, prop.QuerySelector("div.property__content").TextContent)))
                {
                    if (prop.Key == "Бренд")
                        nb.Brand = ClearValue(prop.Value);
                    else if (prop.Key.Contains("Частота обновления экрана"))
                        nb.Frequency = ClearValue(prop.Value);
                    else if (prop.Key.Contains("Разрешение экрана"))
                        nb.Resolution = ClearValue(prop.Value);
                    else if (prop.Key.Contains("Вес"))
                        nb.Weight = ClearValue(prop.Value);
                }
                SiteParseEntities db = new SiteParseEntities();
                db.NoteBook.Add(nb);
                db.SaveChanges();
            }
        }
        private static string ClearValue(string value)
        {
            return value.Trim(' ', '\n');
        }
    }
}
