namespace KinHelper.Model.Entities
{
    public class Character : IScrapedEntity
    {

        public virtual int Id { get; set; }
        public virtual string LotroId { get; set; }
        //public virtual string Url { get { return "http://my.lotro.com/home/character/" + LotroId + "/"; } }
        public virtual string Url { get { return ScrapedUrl; } }

        public virtual string Name { get; set; }
        public virtual string ScrapedUrl { get; set; }

        // character sheet informations?
        public virtual Race Race { get; set; }
        public virtual CharacterClass Class { get; set; }
        public virtual int Level { get; set; }
        public virtual string ImageUrl { get; set; }

        public virtual User User { get; set; }

        public virtual bool IsAnonymous { get; set; }
        public virtual bool HasNoPlayerPage { get; set; }

        public virtual string LastActivityDateString { get; set; }

        public override string ToString()
        {
            return string.Format("LotroId: {0}, Name: {1}, Race: {2}, Class: {3}, Level: {4}", LotroId, Name, Race, Class, Level);
        }
    }
}