using HappyWay.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWay.Infrastructure
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<AppUser> Users { get; }
        IRepository<Product> Products { get; }
        IRepository<Blog> Blogs { get; }
        IRepository<Subscription> Subscriptions { get; set; }
        IRepository<Order> Orders { get; }
        IRepository<Category> Categories { get; }
        IRepository<KnowledgeBase> KnowledgeBases { get; }
        IRepository<OrderDetail> OrderDetails { get; }
        Task<int> SaveAsync();
    }
}
