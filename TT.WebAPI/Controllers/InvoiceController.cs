﻿using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Reflection;
using System.Text;
using TT.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TT.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {

        private static IEnumerable<Invoice> GetAllInvoices()
        {
            IEnumerable<Invoice> invoices = new List<Invoice>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                NewLine = Environment.NewLine,
                Delimiter = ";",
            };
            using (var reader = new StreamReader(Constants.INVOICE_FILE_PATH))
            using (var csv = new CsvReader(reader, config))
            {
                invoices = csv.GetRecords<Invoice>().ToList();
            }

            return invoices;
        }

        private static IEnumerable<Invoice> GetPagedInvoices(int page, int pageSize)
        {
            IEnumerable<Invoice> invoices = GetAllInvoices();

            invoices = invoices.Skip((page - 1) * pageSize).Take(pageSize);

            return invoices;
        }

        private static IEnumerable<Invoice> FilterBy(InvoiceParameters invoiceParameters, IEnumerable<Invoice> invoice)
        {
            return invoice.Where(x => x.Created >= invoiceParameters.MinCreated &&
                                      x.Created <= invoiceParameters.MaxCreated &&
                                      x.Changed >= invoiceParameters.MinChanged &&
                                      x.Changed <= invoiceParameters.MaxChanged &&
                                      x.ProcessingStatus == invoiceParameters.ProcessingStatus &&
                                      x.Amount >= invoiceParameters.MinAmount &&
                                      x.Amount <= invoiceParameters.MaxAmount &&
                                      x.PaymentMethod == invoiceParameters.PaymentMethod);
        }
        private static IEnumerable<Invoice> OrderBy(InvoiceParameters invoiceParameters, IEnumerable<Invoice> invoicesByParameters)
        {
            var orderParams = invoiceParameters.OrderBy.Trim().Split(',');
            var propertyInfos = typeof(Invoice).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            invoicesByParameters = invoicesByParameters.AsQueryable().OrderBy(orderQuery);
            return invoicesByParameters;
        }

        [HttpGet]
        [Route("getByParameters")]
        public ActionResult<Invoice[]> GetByParameters([FromQuery] InvoiceParameters invoiceParameters)
        {
            IEnumerable<Invoice> invoices = GetPagedInvoices(invoiceParameters.PageNumber, invoiceParameters.PageSize);

            IEnumerable<Invoice> invoicesByParameters = FilterBy(invoiceParameters, invoices);

            if (!string.IsNullOrWhiteSpace(invoiceParameters.OrderBy))
                invoicesByParameters = OrderBy(invoiceParameters, invoicesByParameters);

            return Ok(invoicesByParameters);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Invoice), (int)HttpStatusCode.OK)]
        public IActionResult Get(int id)
        {
            IEnumerable<Invoice> invoice = GetAllInvoices().Where(x => x.Number == id);

            return Ok(invoice);
        }

        /* CANNED VISUAL STUDIO EXAMPLES */

        // POST api/<InvoiceController>
        [HttpPost]
        [Route("add")]
        public void Post([FromBody] BaseInvoice baseInvoice)
        {
            var nextInvoiceNumber = GetAllInvoices().Max(x => x.Number) + 1;
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            string fmt = "0000";


            var newInvoice = new 
            {
                Created = now,
                Changed = now,
                Number = nextInvoiceNumber.ToString(fmt),
                ProcessingStatus = (int)Enums.Processing.Statuses.New,
                Amount = baseInvoice.Amount,
                PaymentMethod = (int)baseInvoice.PaymentMethod,                
            };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                NewLine = Environment.NewLine,
                Delimiter = ";",
            };

            using (var writer = new StreamWriter(Constants.INVOICE_FILE_PATH,true))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(new[] { newInvoice });
            }

        }

        //// PUT api/<InvoiceController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<InvoiceController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
