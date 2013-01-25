using System;
using KinHelper.Model.Db;
using KinHelper.Model.Entities;

namespace KinHelper.Model.Parsers
{
    public class CharacterParser
    {
        private readonly KinHelperContext _context;

        public CharacterParser(KinHelperContext context)
        {
            _context = context;
        }

        public void Update(Character character)
        {
            var doc = UrlDocument.Get(character);
            if (doc == null)
            {
                character.HasNoPlayerPage = true;

                Console.WriteLine(character.Name + " ---> NOPAGE");
                return;
            }
            var avatar = doc.DocumentNode.SelectSingleNode("//div[@class='charoverview']/div[@class='avatar']").Element("div");
            character.ImageUrl = avatar.Element("img").GetAttributeValue("src", null);

            string hiddenUserId = string.Empty;
            var charlinks = doc.DocumentNode.SelectSingleNode("//div[@class='charoverview']/div[@class='charlinks']").Element("div");
            var charlink = charlinks.Element("a");
            if (charlink != null && charlink.InnerText.Contains("Character Details"))
            {
                var link = charlink.GetAttributeValue("href", null);
                link = link.Replace("http://my.lotro.com/home/character/", string.Empty);
                hiddenUserId = link.Substring(0,link.IndexOf("/")).Trim('/');
            }

            var userLink = avatar.Element("a");
            if (userLink == null || avatar.InnerText.Contains("Anonymous"))
            {
                character.IsAnonymous = true;
                character.User = _context.GetOrCreateUserFromHidden(hiddenUserId);

                if (String.IsNullOrEmpty(character.User.Name))
                {
                    Console.WriteLine(character.Name + " ---> Anonymous (" + character.User.HiddenId + ")");
                }
                else
                {
                    Console.WriteLine(character.Name + " ---> Anonymous ("+character.User.Name+")");
                }
            }
            else
            {
                var id =
                    userLink.GetAttributeValue("href", null).Replace("http://my.lotro.com/user-", string.Empty).Replace(
                        "/", "");
                var name = userLink.InnerText.Trim();

                character.User = _context.GetOrCreateUser(id, hiddenUserId, name);


                Console.WriteLine(character.Name + " ---> " + name);
            }
        }
    }
}