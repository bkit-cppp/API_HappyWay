using HappyWay.DataAccess;
using HappyWay.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWay.Infrastructure
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new Repository<AppUser>(_context);
            Blogs = new Repository<Blog>(_context);
            Products = new Repository<Product>(_context);
            Orders = new Repository<Order>(_context);
            Subscriptions = new Repository<Subscription>(_context);
            Testimonials = new Repository<Testimonial>(context);
            OrderDetails = new Repository<OrderDetail>(_context);
        }

        public IRepository<AppUser> Users { get; private set; }
        public IRepository<Blog> Blogs { get; set; }
        public IRepository<Product> Products { get; private set; }
        public IRepository<Subscription> Subscriptions { get;  set; }
        public IRepository<Order> Orders { get; private set; }
        public IRepository<OrderDetail> OrderDetails { get; private set; }
        public IRepository<Testimonial> Testimonials { get; set; }
        public IRepository<Category> Categories => throw new NotImplementedException();

        public IRepository<KnowledgeBase> KnowledgeBases => throw new NotImplementedException();

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
