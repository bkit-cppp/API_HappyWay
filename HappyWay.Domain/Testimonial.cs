using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWay.Domain
{
    public class Testimonial
    {
        public int TestimonialId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public AppUser User { get; set; }
    }
}
