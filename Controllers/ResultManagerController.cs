﻿using Bank.NET___backend.Data;
using Bank.NET___backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.NET___backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultManagerController : ControllerBase
    {
        private readonly SqlContext _sqlContext;
        public ResultManagerController(SqlContext sqlContext )
        {
            _sqlContext = sqlContext;
        }

        [HttpPost]
        [Route("api/ResultManagement/SelectedResult/{RequestId}")]
        public ActionResult SelectedResult(int RequestId, [FromBody]Offer of)
        {
            try
            {
                var req = _sqlContext.Requests.Where(r => r.RequestID == RequestId).First();
                var res = _sqlContext.Responses.Where(r=>r.Request.RequestID == RequestId && r.MonthlyInstallment == of.MonthlyInstallment).First();
                if (of.validate(req, res.MonthlyInstallment))
                {
                    req.Status = RequestStatus.OfferSelected.ToString();
                    req.Response = res;
                    res.State = ResponseStatus.PendingConfirmation.ToString();
                    //send add documents and send to review here
                    return Redirect($"api/ResultManagement/getConfirmation/{req.RequestID}/{req.ResponseID}");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/ResultManagement/getConfirmation/{rqid}/{rsid}")]
        public ActionResult GetConfirmation(int rqid, int rsid)
        {
            if (_sqlContext.Responses.Where(r => r.ResponseID == rsid).Count() == 1 && _sqlContext.Requests.Where(r => r.RequestID== rqid).Count() == 1)
            {
                var res = _sqlContext.Responses.Where(r => r.ResponseID == rsid).First();
                res.State = ResponseStatus.Approved.ToString();
                return Redirect("/");
            }
            else
            {
                return NotFound();
            }
        }
    }

    
}