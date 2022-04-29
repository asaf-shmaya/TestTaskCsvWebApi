using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using TT.Models;
using TT.WebAPI.App_Code;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TT.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        [HttpGet]
        [Route("AllPaged")]
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
            IEnumerable<Invoice>? invoice = GetPagedInvoices(1,int.MaxValue).Where(x => x.Number == id);

            return Ok(invoice);
        }

        [HttpGet]
        [Route("filter")]
        public ActionResult<Invoice[]> GetFiltered([FromQuery] Filter filter, int page, int pageSize)
        {
            IEnumerable<Invoice>? invoice = GetPagedInvoices(page, pageSize);

            var filtered = invoice.Where(x => x.Created >= filter.MinCreated &&
                                              x.Created <= filter.MaxCreated &&
                                              x.Changed >= filter.MinChanged &&
                                              x.Changed <= filter.MaxChanged &&
                                              x.ProcessingStatus == filter.ProcessingStatus &&
                                              x.Amount >= filter.MinAmount &&
                                              x.Amount <= filter.MaxAmount &&
                                              x.PaymentMethod == filter.PaymentMethod);

            return Ok(filtered);
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
