using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Lab6.Model;

namespace Lab6.Controller
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        [AllowAnonymous]
        //you must write the full route i think because you didnor put route prefix
        [Route("api/orders/hello")]
        public string gethello() 
        {
            return "hello from server";
        }


        // GET: api/Orders

        //[AllowAnonymous]
        //you must write the full route i think because you didnor put route prefix
        [Route("api/orders/AllOrders")]
        //http://localhost:54653//api/orders/
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        
        
        // GET: api/Orders/5
        [Route("api/orders/Order/{id}")]
        [ResponseType(typeof(Order))]

        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }



        // PUT: api/Orders/5
        [Route("api/orders/UpdateOrder/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok("updated");
        }

        // POST: api/Orders
        [Route("api/orders/AddOrder")]
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
            return Ok("Addedd");
        }

        // DELETE: api/Orders/5
        [Route("api/orders/DeleteOrder/{id}")]
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok("deleted");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}