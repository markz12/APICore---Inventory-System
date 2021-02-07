using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using APICore.Services;
using APICore.Models;
using APICore.Class;
using System.Data;
using Newtonsoft.Json;

namespace APICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDapper _dapper;

        public ProductsController(IDapper dapper)
        {
            _dapper = dapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hey!!! where are you going? Security CHECK!!!!");
        }

        #region Products Action
        [HttpGet(nameof(GetProducts))]
        public async Task<IActionResult> GetProducts(string search, string role, string createdby) // status for admin viewing all, in-stock, out-of-stock,deleted, role - admin,user
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                var param = new DynamicParameters();
                param.Add("status", search, DbType.String);
                param.Add("role", role, DbType.String);
                param.Add("createdby", createdby, DbType.String);
                var response = await Task.FromResult(_dapper.GetAll<dynamic>("[dbo].[GetProducts]", param, commandType: CommandType.StoredProcedure));
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

        [HttpPost(nameof(RegisterProduct))]
        public async Task<IActionResult> RegisterProduct(string data)
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                ProductDetails products = JsonConvert.DeserializeObject<ProductDetails>(data);
                var param = new DynamicParameters();
                param.Add("barcode", products.productinfo.barcode, DbType.String);
                param.Add("category", products.productinfo.category, DbType.String);
                param.Add("brand", products.productinfo.brand, DbType.String);
                param.Add("productname", products.productinfo.productname, DbType.String);
                param.Add("description", products.productinfo.description, DbType.String);
                param.Add("status", products.productinfo.status, DbType.String);
                param.Add("quantity", products.productinfo.quantity, DbType.Int32);
                param.Add("price", products.productinfo.price, DbType.Decimal);
                param.Add("createdby", products.productinfo.createdby, DbType.String);
                param.Add("image", products.productimg.image, DbType.String);
                param.Add("return", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var response = await Task.FromResult(_dapper.GeneralCrud<int>("[dbo].[AddProducts]", param, commandType: CommandType.StoredProcedure));
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
            catch (Exception ex)
            {
                rescode.code = 500;
                rescode.message = ex.Message;
                return StatusCode(500, JsonConvert.SerializeObject(rescode));
            }
        }

        [HttpPost(nameof(UpdateProductDetails))]
        public async Task<IActionResult> UpdateProductDetails(string data)
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                Products products = JsonConvert.DeserializeObject<Products>(data);
                var param = new DynamicParameters();
                param.Add("pid", products.pid, DbType.Int32);
                param.Add("category", products.category, DbType.String);
                param.Add("brand", products.brand, DbType.String);
                param.Add("productname", products.productname, DbType.String);
                param.Add("description", products.description, DbType.String);
                param.Add("status", products.status, DbType.String);
                param.Add("quantity", products.quantity, DbType.Int32);
                param.Add("price", products.price, DbType.Decimal);
                param.Add("updatedby", products.updatedby, DbType.String);
                param.Add("return", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var response = await Task.FromResult(_dapper.GeneralCrud<int>("[dbo].[AddProducts]", param, commandType: CommandType.StoredProcedure));
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
            catch (Exception ex)
            {
                rescode.code = 500;
                rescode.message = ex.Message;
                return StatusCode(500, JsonConvert.SerializeObject(rescode));
            }
        }


        [HttpPost(nameof(UpdateProductQuantity))]
        public async Task<IActionResult> UpdateProductQuantity(int pid, int quantity, string action, string updatedby) //action represent as increment & decrement
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                var param = new DynamicParameters();
                param.Add("pid", pid, DbType.Int32);
                param.Add("quantity", quantity, DbType.Int32);
                param.Add("action", action, DbType.String);
                param.Add("updatedby", updatedby, DbType.String);
                param.Add("return", DbType.Int32, direction: ParameterDirection.Output);
                var response = await Task.FromResult(_dapper.GeneralCrud<int>("[dbo].[UpdateQuantity]", param, commandType: CommandType.StoredProcedure));
                rescode.code = response;
                rescode.message = ResponseMessage.StandardMessage(response);
                if (response == 200)
                {
                    return Ok(JsonConvert.SerializeObject(rescode));
                }
                else if (response == 404)
                {
                    return NotFound(JsonConvert.SerializeObject(rescode));
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
        #endregion

        #region Brand Action

        [HttpGet(nameof(GetBrand))]
        public async Task<IActionResult> GetBrand(string status) //Get active/inactive status
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                var param = new DynamicParameters();
                param.Add("status", status, DbType.String);
                var response = await Task.FromResult(_dapper.GetAll<dynamic>("[dbo].[GetBrands]", param, commandType: CommandType.StoredProcedure));
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

        [HttpPost(nameof(AddBrand))]
        public async Task<IActionResult> AddBrand(string data)
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                brands brand = JsonConvert.DeserializeObject<brands>(data);
                var param = new DynamicParameters();
                param.Add("name", brand.name, DbType.String);
                param.Add("createdby", brand.createdby, DbType.String);
                param.Add("return", DbType.Int32, direction: ParameterDirection.Output);
                var response = await Task.FromResult(_dapper.GeneralCrud<int>("[dbo].[AddBrand]", param, commandType: CommandType.StoredProcedure));
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
            catch (Exception ex)
            {
                rescode.code = 500;
                rescode.message = ex.Message;
                return StatusCode(500, JsonConvert.SerializeObject(rescode));
            }
        }

        [HttpPost(nameof(UpdateBrand))]
        public async Task<IActionResult> UpdateBrand(int brandid, string name, string status, string updatedby)
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                var param = new DynamicParameters();
                param.Add("branid", brandid, DbType.Int32);
                param.Add("name", name, DbType.String);
                param.Add("status", status, DbType.String);
                param.Add("updatedby", updatedby, DbType.String);
                param.Add("return", DbType.Int32, direction: ParameterDirection.Output);
                var response = await Task.FromResult(_dapper.GeneralCrud<int>("[dbo].[UpdateBrand]", param, commandType: CommandType.StoredProcedure));
                rescode.code = response;
                rescode.message = ResponseMessage.StandardMessage(response);
                if (response == 200)
                {
                    return Ok(JsonConvert.SerializeObject(rescode));
                }
                else if (response == 401)
                {
                    return Unauthorized(JsonConvert.SerializeObject(rescode));
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
        #endregion

        #region Categoory

        [HttpGet(nameof(GetCategories))]
        public async Task<IActionResult> GetCategories(string status)
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                var param = new DynamicParameters();
                param.Add("status", status, DbType.String);
                var response = await Task.FromResult(_dapper.GetAll<dynamic>("[dbo].[GetCategories]", param, commandType: CommandType.StoredProcedure));
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

        [HttpPost(nameof(AddCategory))]
        public async Task<IActionResult> AddCategory(string name, string createdby)
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                var param = new DynamicParameters();
                param.Add("name", name, DbType.String);
                param.Add("createdby", createdby, DbType.String);
                param.Add("return", DbType.Int32, direction: ParameterDirection.Output);
                var response = await Task.FromResult(_dapper.GeneralCrud<int>("[dbo].[AddCategory]", param, commandType: CommandType.StoredProcedure));
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
            catch (Exception ex)
            {
                rescode.code = 500;
                rescode.message = ex.Message;
                return StatusCode(500, JsonConvert.SerializeObject(rescode));
            }
        }

        [HttpPost(nameof(UpdateCategory))]
        public async Task<IActionResult> UpdateCategory(int categoryid, string name, string status, string updatedby)
        {
            ResponseCode<dynamic> rescode = new ResponseCode<dynamic>();
            try
            {
                var param = new DynamicParameters();
                param.Add("categoryid", categoryid, DbType.Int32);
                param.Add("name", name, DbType.String);
                param.Add("status", status, DbType.String);
                param.Add("updatedby", updatedby, DbType.String);
                param.Add("return", DbType.Int32, direction: ParameterDirection.Output);
                var response = await Task.FromResult(_dapper.GeneralCrud<int>("[dbo].[UpdateCategory]", param, commandType: CommandType.StoredProcedure));
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
        #endregion
    }
}
