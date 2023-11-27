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
            Notebook nb;
            IElement value;
            foreach ( var a in aList )
            {
                Console.WriteLine(a);
                doc = await context.OpenAsync(a);
                nb = new Notebook();
                value = doc.QuerySelector("div.product__area-title > h1.product__title");
                nb.Name = value.TextContent;
                using (var db = new MigrationsContext())
                {
                    if (db.Notebook.Any(x => x.Name == nb.Name))
                    {
                        break;
                    }
                }
                nb.Id = Guid.NewGuid();
                foreach (var prop in doc.QuerySelectorAll("div#tab-characteristics div.property")
                    .Select(prop => new KeyValuePair<string, string>(prop.QuerySelector("div.property__name").TextContent, prop.QuerySelector("div.property__content").TextContent)))
                {
                    switch (prop.Key)
                    {
                        case "Частота обновления экрана":
                            nb.Frequency = ClearValue(prop.Value);
                            break;
                        case "Разрешение экрана":
                            nb.Resolution = ClearValue(prop.Value);
                            break;
                        case "Вес":
                            nb.Weight = ClearValue(prop.Value);
                            break;
                        case "Бренд":
                            using (var db = new MigrationsContext())
                            {
                                string brand = ClearValue(prop.Value).ToString();
                                if (db.Brand.Count() == 0)
                                {
                                    Brand bb = new Brand();
                                    bb.Name = brand;
                                    db.Brand.Add(bb);
                                    db.SaveChanges();
                                }
                                if (db.Brand.Any(b => b.Name == brand))
                                {
                                    foreach (var b in db.Brand)
                                    {
                                        if (b.Name == brand)
                                        {
                                            nb.BrandID = b.BrandID;
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                using (var db = new MigrationsContext())
                {
                    db.Notebook.Add(nb);
                    db.SaveChanges();
                }
            }
        }
        private static string ClearValue(string value)
        {
            return value.Trim(' ', '\n');
        }
    }
}
