using bs.Auth.Api.Models;
using bs.Data;
using bs.Data.Interfaces;
using System.Linq;

namespace bs.Auth.Api.Repositories
{
    public class CustomersRepository : Repository
    {
        public CustomersRepository(IUnitOfWork unitOfwork) : base(unitOfwork)
        {
        }
        public CustomerModel GetCustomer(Guid customerId)
        {
            return GetById<CustomerModel>(customerId);
        }

        public IQueryable<CustomerModel> GetCustomers()
        {
            return Query<CustomerModel>();
        }

        public Guid CreateCustomer(CustomerModel customer)
        {
            Create(customer);
            return customer.Id;
        }

        public Guid UpdateCustomer(CustomerModel customer)
        {
            Update(customer);
            return customer.Id;
        }

        public void DeleteCustomer(Guid customerId)
        {
            Delete(GetById<CustomerModel>(customerId));
        }

    }
}
