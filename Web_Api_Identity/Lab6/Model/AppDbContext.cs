using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Lab6.Model
{
    public class AppDbContext :IdentityDbContext
    {
        public AppDbContext() : base("OrderCs")
        {

        }
        public DbSet<Order> Orders { get; set; }

        public System.Data.Entity.DbSet<Lab6.Model.OrderDTO> OrderDTOes { get; set; }
    }
}