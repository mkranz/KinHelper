namespace KinHelper.Model.Entities
{
    public class User : IScrapedEntity
    {
        public virtual int Id { get; set; }
        public virtual string LotroId { get; set; }
        public virtual string Name { get; set; }
        public virtual string HiddenId { get; set; }

        public string Url
        {
            get { return "http://my.lotro.com/user-" + LotroId + "/"; }
        }
    }
}