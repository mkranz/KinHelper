using System;
using System.Data;
using System.Linq;
using HtmlAgilityPack;
using KinHelper.Model.Db;
using KinHelper.Model.Entities;

namespace KinHelper.Model.Parsers
{
    public class KinshipParser
    {
        private readonly KinHelperContext _context;

        public KinshipParser(KinHelperContext context)
        {
            _context = context;
        }

        public void Update(Kinship kinship)
        {
            var doc = UrlDocument.Get(kinship);

            kinship.Name = doc.GetElementbyId("pprofile_name").InnerText;
            var fields = doc.DocumentNode.SelectNodes("//td[@id='pprofile_fields']/table/tr");
            foreach (var field in fields)
            {
                switch(field.FirstChild.InnerText)
                {
                    case "Server:":
                        kinship.Server = field.LastChild.InnerText;
                        break;
                    case "Size:":
                        kinship.Size = Int32.Parse(field.LastChild.InnerText);
                        break;
                    case "Recruiting:":
                        kinship.IsRecruiting = field.LastChild.InnerText == "Yes";
                        break;
                    case "Founded:":
                        kinship.FoundedDateTime = DateTime.Parse(field.LastChild.InnerText,
                                                                 System.Globalization.CultureInfo.CreateSpecificCulture(
                                                                     "en-US"));
                        break;
                    case "Leader":
                        // skip until character parsing complete
                        break;
                    case "Play Style:":
                        // skip
                        break;
                    default:
                        Console.WriteLine(field.FirstChild.InnerText + " : "+field.LastChild.InnerText);
                        break;
                }
            }
        }
 
        public void UpdateRoster(Kinship kinship)
        {
            var doc = UrlDocument.Get(kinship,"characters");
            LoadRosterPage(kinship, doc);
            var next = GetNextRosterPageUrl(doc);
            while (next != null)
            {
                doc = UrlDocument.Get(kinship, next);
                LoadRosterPage(kinship, doc);
                next = GetNextRosterPageUrl(doc);
            }
        }

        private static string GetNextRosterPageUrl(HtmlDocument doc)
        {
            var next =
                doc.DocumentNode.SelectNodes(@"//div[@id='page_guildroster']/table[@class='paginate_control']/tr/td[@class='paginate_next']/a");
            if (next != null && next.Count != 0)
            {
                return next[0].GetAttributeValue("href", null);
            }
            return null;
        }

        private void LoadRosterPage(Kinship kinship, HtmlDocument doc)
        {
            var rows = doc.DocumentNode.SelectNodes("//div[@id='page_guildroster']/table[@id='groster_header']/tr[@class='clist_row']");
            foreach (var row in rows)
            {
                KinshipMember member = null;
                foreach (var column in row.Elements("td"))
                {
                    switch(column.GetFirstClass())
                    {
                        case "name":
                            var pnote = column.Element("div").Element("div").GetAttributeValue("id", null);
                            var lotroId = pnote.Replace("star_", string.Empty);
                            var link = column.Element("div").Element("a");
                            var name = link.InnerText;
                            var url = link.GetAttributeValue("href", null);

                            var existingCharacter = kinship.Roster.FirstOrDefault(x => x.Character.LotroId == lotroId);
                            if (existingCharacter != null)
                            {
                                Console.WriteLine("Updating " + name);
                                member = existingCharacter;
                                _context.Entry(existingCharacter).State = EntityState.Modified;
                                _context.Entry(existingCharacter.Character).State = EntityState.Modified;
                            }
                            else
                            {
                                Console.WriteLine("Adding "+name);
                                member = new KinshipMember()
                                                 {
                                                     Character = new Character(),
                                                     Kinship = kinship
                                                 };
                                kinship.Roster.Add(member);
                                _context.Members.Add(member);
                                _context.Characters.Add(member.Character);
                            }

                            member.Character.ScrapedUrl = url;
                            member.Character.Name = name;
                            member.Character.LotroId = lotroId;
                            break;
                        case "level":
                            member.Character.Level = Int32.Parse(column.InnerText.Trim());
                            break;
                        case "race":
                            member.Character.Race = _context.GetOrCreateRace(column.InnerText.Trim());
                            break;
                        case "class":
                            member.Character.Class = _context.GetOrCreateClass(column.Element("img").GetAttributeValue("title",null));
                            break;
                        case "rank":
                            member.Rank = _context.GetOrCreateRank(column.InnerText.Trim());
                            break;
                    }
                }
            }
        }

        public string GetKinshipId(string server, string name)
        {
            // http://my.lotro.com/kinship-crickhollow-asylum
            var url = "http://my.lotro.com/kinship-" + server.ToLower() + "-" + name.ToLower().Replace(' ','_');
            var redirect = UrlDocument.GetRedirectUrl(url);  // /guild-870320629166151452
            return redirect.Replace("/guild-", String.Empty);
        }
    }
}   