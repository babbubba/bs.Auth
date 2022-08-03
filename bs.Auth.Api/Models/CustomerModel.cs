using bs.Data.Interfaces.BaseEntities;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace bs.Auth.Api.Models
{
    public class CustomerModel : IPersistentEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Vat { get; set; }
        public virtual string BusinessName { get; set; }

        public class Map : ClassMapping<CustomerModel>
        {
            public Map()
            {
                Table("Customers");
                Id(x => x.Id, x =>
                {
                    x.Generator(Generators.GuidComb);
                    x.Type(NHibernateUtil.Guid);
                    x.Column("Id");
                    x.UnsavedValue(Guid.Empty);
                });

                Property(x => x.Name);
                Property(x => x.Vat);
                Property(x => x.BusinessName);
            }
        }
    }
}
