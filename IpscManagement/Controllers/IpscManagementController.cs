using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;
using CsvHelper;
using IpscManagement.Models;
using IpscManagement.Services;
using Microsoft.AspNet.Identity;

namespace IpscManagement.Controllers
{
    //[EnableCors(origins: "http://localhost:10112", headers: "*", methods: "*")]
    [RoutePrefix("api/IpscManagement")]
    public class IpscManagementController : ApiBaseController
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("setMember")]
        public async Task<IHttpActionResult> SetMember(MemberModel member)
        {
            try
            {
                if (member != null)
                {
                    var result = await Task.Factory.StartNew(() => ManagementService.SetMember(member));
                    if (!result.Successed)
                    {
                        return BadRequest(result.Message);
                    }
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("updateAmount")]
        public async Task<IHttpActionResult> UpdateAmount(AmountChangeModel amountChange)
        {
            try
            {
                ApplicationUser appUser = UserManager.FindById(User.Identity.GetUserId());
                await Task.Factory.StartNew(() => ManagementService.UpdateAmount(amountChange, appUser));
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("getStocks")]
        public async Task<IHttpActionResult> GetStocks()
        {
            try
            {
                var stocks = await Task.Factory.StartNew(() => ManagementService.GetStocks());
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("getMember")]
        public async Task<IHttpActionResult> GetMember([FromBody]int identity)
        {
            try
            {
                var member = await Task.Factory.StartNew(() => ManagementService.GetMemberByIdentity(identity));
                return Ok(member);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("getMemberBulletsStockHistoryChanges")]
        public async Task<IHttpActionResult> GetMemberBulletsStockHistoryChanges([FromBody]int identity)
        {
            try
            {
                var history = await Task.Factory.StartNew(() => ManagementService.GetMemberBulletsStockHistoryChanges(identity));
                return Ok(history);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("updateWarehouseBulletsStock")]
        public async Task<IHttpActionResult> UpdateWarehouseBulletsStock(WarehouseBulletsStockModel warehouseBulletsStockModel)
        {
            try
            {
                ApplicationUser appUser = UserManager.FindById(User.Identity.GetUserId());
                int newAmmoAmmount = await Task.Factory.StartNew(() => ManagementService.UpdateWarehouseBulletsStock(warehouseBulletsStockModel, appUser));
                return Ok(newAmmoAmmount);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("getWarehouseBulletsStockAmmount")]
        public async Task<IHttpActionResult> GetWarehouseBulletsStockAmmount()
        {
            try
            {
                int warehouseAmmoAmmount = await Task.Factory.StartNew(() => ManagementService.GetWarehouseBulletsStockAmmount());
                return Ok(warehouseAmmoAmmount);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("getWarehouseStockHistoryChanges")]
        public async Task<IHttpActionResult>GetWarehouseStockHistoryChanges()
        {
            try
            {
                var warehouseStockHistoryChanges = await Task.Factory.StartNew(() => ManagementService.GetWarehouseStockHistoryChanges());
                return Ok(warehouseStockHistoryChanges);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //   [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getReport")]
        public async Task<HttpResponseMessage> GetReport(string identity, string fromDate, string toDate)
        {

            dynamic report;

            DateTimeFormatInfo ilDtfi = new CultureInfo("he-IL", false).DateTimeFormat;
            DateTime fDate = DateTime.Now.AddYears(-100);
            DateTime tDate = DateTime.Now.AddDays(1);

            if (!string.IsNullOrEmpty(fromDate) && fromDate != "undefined")
            {
                fDate = Convert.ToDateTime(fromDate, ilDtfi);
            }

            if (!string.IsNullOrEmpty(toDate) && fromDate != "undefined")
            {
                tDate = Convert.ToDateTime(toDate, ilDtfi);
            }

            if (identity != 1.ToString())
            {
                report = ManagementService.GetMemberBulletsStockHistoryChangesForCSV(identity, fDate, tDate);
            }
            else
            {
                report = ManagementService.GetWarehouseStockHistoryChangesReport(fDate, tDate);
            }

            var stream = new MemoryStream();
            var encoding = new UTF8Encoding(true);
            byte[] preamble = encoding.GetPreamble();
            if (preamble.Length > 0)
            {
                stream.Write(preamble, 0, preamble.Length);
            }

            using (var writer = new StreamWriter(stream, encoding, 8192, true))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(report);
                }
            }

            stream.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = identity + "_Report.csv"
            };
            return result;
            
        }

        //   [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getWarehouseStocStatusReport")]
        public async Task<HttpResponseMessage> GetWarehouseStocStatusReport(string forDate)
        {

            dynamic report;

            DateTimeFormatInfo ilDtfi = new CultureInfo("he-IL", false).DateTimeFormat;
            DateTime fDate = DateTime.Now.AddYears(-100);
            
            if (!string.IsNullOrEmpty(forDate) && forDate != "undefined")
            {
                fDate = Convert.ToDateTime(forDate, ilDtfi);
            }

            report = ManagementService.GetWarehouseStocStatusReport(fDate);

            var stream = new MemoryStream();
            var encoding = new UTF8Encoding(true);
            byte[] preamble = encoding.GetPreamble();
            if (preamble.Length > 0)
            {
                stream.Write(preamble, 0, preamble.Length);
            }

            using (var writer = new StreamWriter(stream, encoding, 8192, true))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(report);
                }
            }

            stream.Flush();
            stream.Position = 0;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = forDate + "_Report.csv"
            };
            return result;

        }

    }

  
}
