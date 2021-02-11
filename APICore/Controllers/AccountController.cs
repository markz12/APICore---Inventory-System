using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICore.Services;
using Newtonsoft.Json;
using APICore.Models;
using APICore.Class;
using Dapper;
using System.Data;

namespace APICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IDapper _dapper;
        public AccountController(IDapper dapper)
        {
            _dapper = dapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hey you are on the wrong side! Keep going!");
        }

        [HttpPost(nameof(ValidateUser))]
        public async Task<IActionResult> ValidateUser([FromBody] Users user)
        {
            ResponseAPI<Users> rescode = new ResponseAPI<Users>();
            try
            {
                var param = new DynamicParameters();
                param.Add("username", user.username, DbType.String);
                param.Add("password", user.password, DbType.String);
                Users response = await Task.FromResult(_dapper.Get<Users>("[dbo].[ValidateUserAccess]", param, commandType: CommandType.StoredProcedure));
                rescode.code = response == null ? 404 : 200;
                rescode.message = ResponseMessage.StandardMessage(response == null ? 404 : 200);
                rescode.data = response;
                return Ok(JsonConvert.SerializeObject(rescode));
            }
            catch (Exception ex)
            {
                rescode.code = 500;
                rescode.message = ex.Message;
                rescode.data = null;
                return StatusCode(500, JsonConvert.SerializeObject(rescode));
            }
        }

        [HttpGet(nameof(GetSpecificUser))]
        public async Task<IActionResult> GetSpecificUser(string username)
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                var param = new DynamicParameters();
                param.Add("username", username, DbType.String);
                var response = await Task.FromResult(_dapper.Get<dynamic>("[dbo].[GetSpecificUser]", param, commandType: CommandType.StoredProcedure));
                rescode.code = response == null ? 404 : 200;
                rescode.message = ResponseMessage.StandardMessage(response == null ? 404 : 200);
                rescode.data = response;
                return Ok(JsonConvert.SerializeObject(rescode));
            }
            catch (Exception ex)
            {
                rescode.code = 500;
                rescode.message = ex.Message;
                return StatusCode(500, JsonConvert.SerializeObject(rescode));
            }
        }

        [HttpGet(nameof(GetRegisteredUsers))]
        public async Task<IActionResult> GetRegisteredUsers(string status) //status data should be: active, inactive and deleted. deleted should be the status of the data after the user request to delete. no delete query executed in this api.
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                var param = new DynamicParameters();
                param.Add("status", status, DbType.String);
                var response = await Task.FromResult(_dapper.GetAll<dynamic>("[dbo].[GetRegisterdUser]", param, commandType: CommandType.StoredProcedure));
                rescode.code = response == null ? 404 : 200;
                rescode.message = ResponseMessage.StandardMessage(response == null ? 404 : 200);
                rescode.data = response;
                return Ok(JsonConvert.SerializeObject(rescode));
            }
            catch (Exception ex)
            {
                rescode.code = 500;
                rescode.message = ex.Message;
                return StatusCode(500, JsonConvert.SerializeObject(rescode));
            }
        }

        [HttpPost(nameof(createuser))]
        public async Task<IActionResult> createuser(string data)
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                Users user = JsonConvert.DeserializeObject<Users>(data);
                if (user == null)
                {
                    return BadRequest(ResponseMessage.StandardMessage(300));
                }
                else
                {
                    var param = new DynamicParameters();
                    param.Add("fullname", user.fullname, DbType.String);
                    param.Add("contact", user.contact, DbType.String);
                    param.Add("email", user.email, DbType.String);
                    param.Add("username", user.username, DbType.String);
                    param.Add("password", user.password, DbType.String);
                    param.Add("roles", user.roles, DbType.String);
                    param.Add("image", user.image, DbType.String);
                    param.Add("return", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var response = await Task.FromResult(_dapper.GeneralCrud<int>("[dbo].[addUserData]", param, commandType: CommandType.StoredProcedure));
                    rescode.code = response;
                    rescode.message = ResponseMessage.StandardMessage(response);

                    if (response == 200)
                    {
                        return Ok(JsonConvert.SerializeObject(rescode));
                    }
                    else if (response == 409)
                    {
                        return Conflict(JsonConvert.SerializeObject(rescode));
                    }
                    else
                    {
                        return BadRequest(JsonConvert.SerializeObject(rescode));
                    }
                }
            }
            catch (Exception ex)
            {
                rescode.code = 500;
                rescode.message = ex.Message;
                return StatusCode(500, JsonConvert.SerializeObject(rescode));
            }
        }

        [HttpPut(nameof(UpdateUser))]
        public async Task<IActionResult> UpdateUser(string data)
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                Users user = JsonConvert.DeserializeObject<Users>(data);
                var param = new DynamicParameters();
                param.Add("uid", user.uid, DbType.Int32);
                param.Add("fullname", user.fullname, DbType.String);
                param.Add("contact", user.contact, DbType.String);
                param.Add("email", user.email, DbType.String);
                param.Add("username", user.username, DbType.String);
                param.Add("password", user.password, DbType.String);
                param.Add("status", user.status, DbType.String);
                param.Add("image", user.image, DbType.String);
                param.Add("return", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var response = await Task.FromResult(_dapper.GeneralCrud<int>("[dbo].[UpdateUserDetails]", param, commandType: CommandType.StoredProcedure));
                rescode.code = response;
                rescode.message = ResponseMessage.StandardMessage(response);
                if (response == 200)
                {
                    return Ok(JsonConvert.SerializeObject(rescode));
                }
                else
                {
                    return BadRequest(JsonConvert.SerializeObject(rescode));
                }
            }
            catch (Exception ex)
            {
                rescode.code = 500;
                rescode.message = ex.Message;
                return StatusCode(500, JsonConvert.SerializeObject(rescode));
            }
        }

    }
}
