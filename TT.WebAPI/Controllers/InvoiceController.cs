using CsvHelper;
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
        [HttpGet]
        [Route("allPaged")]
        [ProducesResponseType(typeof(IEnumerable<Invoice>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll(int page = 1, int pageSize = 50)
        {
            IEnumerable<Invoice> invoices = GetPagedInvoices(page, pageSize);

            return Ok(invoices);
        }

        private static IEnumerable<Invoice> GetPagedInvoices(int page, int pageSize)
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
                invoices = csv.GetRecords<Invoice>().ToList().Skip((page - 1) * pageSize).Take(pageSize);
            }

            return invoices;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Invoice), (int)HttpStatusCode.OK)]
        public IActionResult Get(int id)
        {
            IEnumerable<Invoice>? invoice = GetPagedInvoices(1, int.MaxValue).Where(x => x.Number == id);

            return Ok(invoice);
        }

        [HttpGet]
        [Route("getByParameters")]
        public ActionResult<Invoice[]> GetByParameters([FromQuery] InvoiceParameters invoiceParameters)
        {
            var invoice = GetPagedInvoices(invoiceParameters.PageNumber, invoiceParameters.PageSize);

            IEnumerable<Invoice> invoicesByParameters = Filter(invoiceParameters, invoice);

            if (!string.IsNullOrWhiteSpace(invoiceParameters.OrderBy))
                invoicesByParameters = OrderBy(invoiceParameters, invoicesByParameters);

            return Ok(invoicesByParameters);
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

        private static IEnumerable<Invoice> Filter(InvoiceParameters invoiceParameters, IEnumerable<Invoice> invoice)
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

        /* CANNED VISUAL STUDIO EXAMPLES */

        //// POST api/<InvoiceController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

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
