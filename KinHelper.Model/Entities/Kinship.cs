using System;
using System.Collections.Generic;

namespace KinHelper.Model.Entities
{
    public class Kinship : IScrapedEntity
    {
        public int Id { get; set; }

        public virtual string LotroId { get; set; }
        public virtual string Url { get { return "http://my.lotro.com/guild-" + LotroId + "/"; } }

        public virtual string Name { get; set; }
        public virtual string Server { get; set; }
        public virtual int Size { get; set; }
        public virtual bool IsRecruiting { get; set; }
        public virtual DateTime FoundedDateTime { get; set; }
        
        public virtual ICollection<KinshipMember> Roster { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}