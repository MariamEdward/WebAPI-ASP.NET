using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lab6.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int  Number { get; set; }
        public string CustomerId { get; set; }



        [JsonIgnore]
        [ForeignKey("CustomerId")]
        public virtual IdentityUser Customer { get; set; }
    }
}