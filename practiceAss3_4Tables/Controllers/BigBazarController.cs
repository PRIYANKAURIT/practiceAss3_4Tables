using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practiceAss3_4Tables.Model;
using practiceAss3_4Tables.Repository.Interface;
using static practiceAss3_4Tables.Model.BaseModel;

namespace practiceAss3_4Tables.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BigBazarController : ControllerBase
    {
        private readonly ILogger logger;
        public IBigBazarRepository BigBazar;

        public BigBazarController(IConfiguration configuartion, ILoggerFactory loggerFactory, IBigBazarRepository BigBazarRepo)
        {
            this.logger = loggerFactory.CreateLogger<BigBazarController>();
            this.BigBazar = BigBazarRepo;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult> GetAllProducts()
        {
            BaseResponseStatus baseResponseStatus = new BaseResponseStatus();
            logger.LogDebug(string.Format($"BigBazarController-GetAllProducts:Calling GetAllProducts."));
            var segment = await BigBazar.GetAllProducts();
            if (segment.Count == 0)
            {
                baseResponseStatus.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponseStatus.StatusMessage = "Data not found";
            }
            else
            {
                baseResponseStatus.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponseStatus.StatusMessage = "All Data feached Successfully";
                baseResponseStatus.ResponseData = segment;
            }
            return Ok(baseResponseStatus);
        }


        [HttpGet("GetAllDiscount")]
        public async Task<ActionResult> GetAllDiscount()
        {
            BaseResponseStatus baseResponseStatus = new BaseResponseStatus();
            logger.LogDebug(string.Format($"BigBazarController-GetAllDiscount:Calling GetAllDiscount."));
            var segment = await BigBazar.GetAllDiscount();
            if (segment.Count == 0)
            {
                baseResponseStatus.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponseStatus.StatusMessage = "Data not found";
            }
            else
            {
                baseResponseStatus.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponseStatus.StatusMessage = "All Data feached Successfully";
                baseResponseStatus.ResponseData = segment;
            }
            return Ok(baseResponseStatus);
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult> GetAllOrders()
        {
            BaseResponseStatus baseResponseStatus = new BaseResponseStatus();
            logger.LogDebug(string.Format($"BigBazarController-GetAllOrders:Calling GetAllOrders."));
            var segment = await BigBazar.GetAllOrders();
            if (segment.Count == 0)
            {
                baseResponseStatus.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponseStatus.StatusMessage = "Data not found";
            }
            else
            {
                baseResponseStatus.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponseStatus.StatusMessage = "All Data feached Successfully";
                baseResponseStatus.ResponseData = segment;
            }
            return Ok(baseResponseStatus);
        }

        [HttpGet("GetorderbyId")]

        public async Task<IActionResult> GetorderbyId(int Id)
        {
            BaseResponseStatus responseDetails = new BaseResponseStatus();
            try
            {
                logger.LogDebug(string.Format("MasterDropdownController-GetAllTaluka : Calling GetAllTaluka"));
                var dist = await BigBazar.GetorderbyId(Id);

                if (dist.Count == 0)
                {
                    var returnMsg = string.Format("There are not any records for GetAllTaluka.");
                    logger.LogInformation(returnMsg);
                    responseDetails.StatusCode = StatusCodes.Status404NotFound.ToString();
                    responseDetails.StatusMessage = returnMsg;
                    return Ok(responseDetails);
                }
                var rtrMsg = string.Format("All  records are fetched successfully.");
                logger.LogDebug("MasterDropdownController-GetAllTaluka : Completed Get action all GetAllTaluka records.");
                responseDetails.StatusCode = StatusCodes.Status200OK.ToString();
                responseDetails.StatusMessage = rtrMsg;
                responseDetails.ResponseData = dist;
            }
            catch (Exception ex)
            {
                //log error
                logger.LogError(ex.Message);
                var returnMsg = string.Format(ex.Message);
                logger.LogInformation(returnMsg);
                responseDetails.StatusCode = StatusCodes.Status409Conflict.ToString();
                responseDetails.StatusMessage = returnMsg;
                return Ok(responseDetails);
            }
            return Ok(responseDetails);
        }
        /*
                [HttpGet("GetById")]
                public async Task<IActionResult> GetById(int id)
                {
                    BaseResponseStatus baseResponseStatus = new BaseResponseStatus();
                    logger.LogDebug(string.Format($"SegmentMasterController-GetById:Calling GetById."));
                    var segment = await segmentMasterRepository.GetById(id);
                    if (segment == null)
                    {
                        baseResponseStatus.StatusCode = StatusCodes.Status404NotFound.ToString();
                        baseResponseStatus.StatusMessage = "Data not found";
                    }
                    else
                    {
                        baseResponseStatus.StatusCode = StatusCodes.Status200OK.ToString();
                        baseResponseStatus.StatusMessage = "All Data feached Successfully";
                        baseResponseStatus.ResponseData = segment;
                    }
                    return Ok(baseResponseStatus);
                }
        */
        [HttpPost("Add")]
                public async Task<IActionResult> Add(OrderModel orderModel)
                {
                    BaseResponseStatus baseResponseStatus = new BaseResponseStatus();
                    logger.LogDebug(String.Format($"BigBazarController-Add:Calling By Add action."));
                    if (BigBazar != null)
                    {
                        var Execution = await BigBazar.Add(orderModel);
                        if (Execution == -1)
                        {
                            var returnmsg = string.Format($"Record Is Already saved With ID ");
                            logger.LogDebug(returnmsg);
                            baseResponseStatus.StatusCode = StatusCodes.Status409Conflict.ToString();
                            baseResponseStatus.StatusMessage = returnmsg;
                            return Ok(baseResponseStatus);
                        }
                        else if (Execution >= 1)
                        {
                            var rtnmsg = string.Format("Record added successfully..");
                            logger.LogInformation(rtnmsg);
                            logger.LogDebug(string.Format("BigBazarController-Add:Calling By Add action."));
                            baseResponseStatus.StatusCode = StatusCodes.Status200OK.ToString();
                            baseResponseStatus.StatusMessage = rtnmsg;
                            baseResponseStatus.ResponseData = Execution;
                            return Ok(baseResponseStatus);
                        }
                        else
                        {
                            var rtnmsg1 = string.Format("Error while Adding");
                            logger.LogError(rtnmsg1);
                            baseResponseStatus.StatusCode = StatusCodes.Status409Conflict.ToString();
                            baseResponseStatus.StatusMessage = rtnmsg1;
                            return Ok(baseResponseStatus);
                        }

                    }
                    else
                    {
                        var returnmsg = string.Format("Record added successfully..");
                        logger.LogDebug(returnmsg);
                        baseResponseStatus.StatusCode = StatusCodes.Status200OK.ToString();
                        baseResponseStatus.StatusMessage = returnmsg;
                        return Ok(baseResponseStatus);
                    }
                }


                [HttpPut("Update")]
                public async Task<IActionResult> Update(OrderModel orderModel)
                {
                    BaseResponseStatus baseResponseStatus = new BaseResponseStatus();
                    logger.LogDebug(String.Format($"BigBazarController-Update:Calling By Update action."));
                    if (BigBazar != null)
                    {
                        var Execution = await BigBazar.Update(orderModel);
                        if (Execution == -1)
                        {
                            var returnmsg = string.Format($"Record Is Already saved With ID {orderModel.Id}");
                            logger.LogDebug(returnmsg);
                            baseResponseStatus.StatusCode = StatusCodes.Status409Conflict.ToString();
                            baseResponseStatus.StatusMessage = returnmsg;
                            return Ok(baseResponseStatus);
                        }
                        else if (Execution >= 1)
                        {
                            var rtnmsg = string.Format("Record update successfully..");
                            logger.LogInformation(rtnmsg);
                            logger.LogDebug(string.Format("BigBazarController-Update:Calling By Update action."));
                            baseResponseStatus.StatusCode = StatusCodes.Status200OK.ToString();
                            baseResponseStatus.StatusMessage = rtnmsg;
                            baseResponseStatus.ResponseData = Execution;
                            return Ok(baseResponseStatus);
                        }
                        else
                        {
                            var rtnmsg1 = string.Format("Error while Adding");
                            logger.LogError(rtnmsg1);
                            baseResponseStatus.StatusCode = StatusCodes.Status409Conflict.ToString();
                            baseResponseStatus.StatusMessage = rtnmsg1;
                            return Ok(baseResponseStatus);
                        }

                    }
                    else
                    {
                        var returnmsg = string.Format("Record added successfully..");
                        logger.LogDebug(returnmsg);
                        baseResponseStatus.StatusCode = StatusCodes.Status200OK.ToString();
                        baseResponseStatus.StatusMessage = returnmsg;
                        return Ok(baseResponseStatus);
                    }
                }
                [HttpDelete]
                public async Task<ActionResult> Delete(DeleteObj deleteObj)
                {
                    BaseResponseStatus baseResponse = new BaseResponseStatus();
                    logger.LogDebug(string.Format("SegmentMasterController-Delete:Calling By Delete action"));
                    if (deleteObj != null)
                    {
                        var Execution = await BigBazar.Delete(deleteObj);

                        if (Execution >= 1)
                        {
                            var rtnmsg = string.Format("Record Deleted successfully..");
                            logger.LogDebug(rtnmsg);
                            baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                            baseResponse.StatusMessage = rtnmsg;
                            baseResponse.ResponseData = Execution;
                            return Ok(baseResponse);
                        }
                        else
                        {
                            var rtnmsg = string.Format("Record is not deleted because it is exist in Ass3_TrnOrdeDetails table");
                            logger.LogDebug(rtnmsg);
                            baseResponse.StatusCode = StatusCodes.Status409Conflict.ToString();
                            baseResponse.StatusMessage = rtnmsg;
                            return Ok(baseResponse);
                        }
                    }
                    else
                    {
                        var rtnmsg = string.Format("Record Deleted successfully..");
                        logger.LogDebug(rtnmsg); 
                        baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                        baseResponse.StatusMessage = rtnmsg;
                        return Ok(baseResponse);
                    }
                }
    }
}

