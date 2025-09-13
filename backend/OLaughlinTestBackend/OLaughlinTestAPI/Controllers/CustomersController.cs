using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Base;
using Models.Entities;
using Services;
namespace OLaughlinTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/<CustomersController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<BaseResponseModel<IEnumerable<Customer>>> GetAllAsync()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                if (customers == null || !customers.Any())
                {
                    return new BaseResponseModel<IEnumerable<Customer>>()
                    {
                        ErrorDetails = new ErrorDetailsModel()
                        {
                            ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                            HttpStatusCode = HttpStatusCodeEnum.NotFound,
                            Message = "No customers found"
                        },
                        HasError = false,
                        Result = null
                    };
                }
                return new BaseResponseModel<IEnumerable<Customer>>()
                {
                    Result = customers,
                    ErrorDetails = new ErrorDetailsModel() { HttpStatusCode = HttpStatusCodeEnum.OK, Message = "Success" },
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<Customer>>()
                {
                    ErrorDetails = new ErrorDetailsModel()
                    {
                        ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                        HttpStatusCode = HttpStatusCodeEnum.Internal,
                        Message = ex.Message
                    },
                    HasError = true,
                    Result = null
                };
            }
        }

        // GET: api/Customer/id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<BaseResponseModel<Customer>> GetCustomer(string id)
        {
            try
            {
                var result = await _customerService.GetByIdAsync(id);
                if (result == null)
                {
                    return new BaseResponseModel<Customer>()
                    {
                        ErrorDetails = new ErrorDetailsModel()
                        {
                            ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                            HttpStatusCode = HttpStatusCodeEnum.NotFound,
                            Message = "Customer not found"
                        },
                        HasError = true,
                        Result = null
                    };
                }
                // Create a response model
                BaseResponseModel<Customer> response = new BaseResponseModel<Customer>()
                {
                    Result = result,
                    ErrorDetails = new ErrorDetailsModel() { HttpStatusCode = HttpStatusCodeEnum.OK, Message = "Success" },
                    HasError = false
                };
                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<Customer>()
                {
                    ErrorDetails = new ErrorDetailsModel()
                    {
                        ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                        HttpStatusCode = HttpStatusCodeEnum.Internal,
                        Message = ex.Message
                    },
                    HasError = true,
                    Result = null
                };
            }
        }


        // POST: api/Customer
        [HttpPost]
        public async Task<BaseResponseModel<Customer>> PostCustomer(Customer customer)
        {
            try
            {

                // Add the customer to the database
                var insertedCustomer = await _customerService.AddAsync(customer);


                BaseResponseModel<Customer> result = new BaseResponseModel<Customer>()
                {
                    Result = insertedCustomer,
                    ErrorDetails = new ErrorDetailsModel() { HttpStatusCode = HttpStatusCodeEnum.OK, Message = "Success" },
                    HasError = false
                };

                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<Customer>()
                {
                    ErrorDetails = new ErrorDetailsModel()
                    {
                        ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                        HttpStatusCode = HttpStatusCodeEnum.Internal,
                        Message = $"An error occurred while creating the customer. Details: {ex.Message}"
                    },
                    HasError = true,
                    Result = null
                };
            }
        }


        // PUT: api/Customer/id
        [HttpPut("{id}")]
        public async Task<BaseResponseModel<bool?>> PutCustomer(Customer customer)
        {
            try
            {
                // update the existing customer
                var updateResult = await _customerService.UpdateAsync(customer);
                BaseResponseModel<bool?> result = new BaseResponseModel<bool?>()
                {
                    Result = updateResult,
                    ErrorDetails = new ErrorDetailsModel() { HttpStatusCode = HttpStatusCodeEnum.OK, Message = "Success" },
                    HasError = !updateResult
                };
                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<bool?>()
                {
                    ErrorDetails = new ErrorDetailsModel()
                    {
                        ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                        HttpStatusCode = HttpStatusCodeEnum.Internal,
                        Message = $"An error occurred while updating the customer. Details: {ex.Message}"
                    },
                    HasError = true,
                    Result = null
                };
            }


        }

        /// <summary>
        /// Deletes an Customer
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Boolean indicating success or failure of the delete operation</returns>
        [HttpDelete("{id}")]
        public async Task<BaseResponseModel<bool?>> DeleteCustomer(string id)
        {
            try
            {
                bool deleteResult;

                // Perform physical delete logic
                deleteResult = await _customerService.DeleteAsync(id);
                if (!deleteResult)
                {
                    return new BaseResponseModel<bool?>()
                    {
                        ErrorDetails = new ErrorDetailsModel()
                        {
                            ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                            HttpStatusCode = HttpStatusCodeEnum.NotFound,
                            Message = $"Customer with ID {id} was not found for physical deletion."
                        },
                        HasError = true,
                        Result = false
                    };
                }
                return new BaseResponseModel<bool?>()
                {
                    Result = deleteResult,
                    ErrorDetails = new ErrorDetailsModel()
                    {
                        HttpStatusCode = HttpStatusCodeEnum.OK,
                        Message = deleteResult
                            ? $"Customer successfully deleted."
                            : $"Failed to  delete the customer."
                    },
                    HasError = !deleteResult
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<bool?>()
                {
                    ErrorDetails = new ErrorDetailsModel()
                    {
                        ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                        HttpStatusCode = HttpStatusCodeEnum.Internal,
                        Message = $"An error occurred while deleting the customer. Details: {ex.Message}"
                    },
                    HasError = true,
                    Result = null
                };
            }
        }
    }
}
