using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Base;
using Models.DTO;
using Models.Entities;
using Services;

namespace OLaughlinTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET: api/<AppoitmentsController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<BaseResponseModel<IEnumerable<AppointmentDTO>>> GetAllAsync()
        {
            try
            {
                var appointments = await _appointmentService.GetAllAsync();
                if (appointments == null || !appointments.Any())
                {
                    return new BaseResponseModel<IEnumerable<AppointmentDTO>>()
                    {
                        ErrorDetails = new ErrorDetailsModel()
                        {
                            ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                            HttpStatusCode = HttpStatusCodeEnum.NotFound,
                            Message = "No appointments found"
                        },
                        HasError = false,
                        Result = null
                    };
                }
                return new BaseResponseModel<IEnumerable<AppointmentDTO>>()
                {
                    Result = AppointmentDTO.FromEntityList(appointments.ToList()),
                    ErrorDetails = new ErrorDetailsModel() { HttpStatusCode = HttpStatusCodeEnum.OK, Message = "Success" },
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<AppointmentDTO>>()
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

        // GET: api/Appointment/id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<BaseResponseModel<Appointment>> GetAppointment(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                {
                    return new BaseResponseModel<Appointment>()
                    {
                        ErrorDetails = new ErrorDetailsModel()
                        {
                            ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                            HttpStatusCode = HttpStatusCodeEnum.BadRequest,
                            Message = "Invalid appointment id"
                        },
                        HasError = true,
                        Result = null
                    };
                }

                var result = await _appointmentService.GetByIdAsync(id); // ahora recibe Guid
                if (result == null)
                {
                    return new BaseResponseModel<Appointment>()
                    {
                        ErrorDetails = new ErrorDetailsModel()
                        {
                            ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                            HttpStatusCode = HttpStatusCodeEnum.NotFound,
                            Message = "Appointment not found"
                        },
                        HasError = true,
                        Result = null
                    };
                }

                return new BaseResponseModel<Appointment>()
                {
                    Result = result,
                    ErrorDetails = new ErrorDetailsModel() { HttpStatusCode = HttpStatusCodeEnum.OK, Message = "Success" },
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<Appointment>()
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

        // POST: api/Appointment
        [HttpPost]
        public async Task<BaseResponseModel<Appointment>> PostAppointment(NewAppointmentDTO appointment)
        {
            try
            {
                Appointment newAppointment = new Appointment
                {
                    Id = Guid.NewGuid(), // Genera un nuevo GUID para el ID
                    DateTime = appointment.DateTime,
                    Status = appointment.Status,
                    CustomerId = appointment.CustomerId,
                };

                // Add the appointment to the database
                var insertedAppointment = await _appointmentService.AddAsync(newAppointment);


                BaseResponseModel<Appointment> result = new BaseResponseModel<Appointment>()
                {
                    Result = insertedAppointment,
                    ErrorDetails = new ErrorDetailsModel() { HttpStatusCode = HttpStatusCodeEnum.OK, Message = "Success" },
                    HasError = false
                };

                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<Appointment>()
                {
                    ErrorDetails = new ErrorDetailsModel()
                    {
                        ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                        HttpStatusCode = HttpStatusCodeEnum.Internal,
                        Message = $"An error occurred while creating the appointment. Details: {ex.Message}"
                    },
                    HasError = true,
                    Result = null
                };
            }
        }


        // PUT: api/Appointment/id
        [HttpPut("{id}")]
        public async Task<BaseResponseModel<bool?>> PutAppointment(Appointment appointment)
        {
            try
            {
                // update the existing customer
                var updateResult = await _appointmentService.UpdateAsync(appointment);
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
                        Message = $"An error occurred while updating the appointment. Details: {ex.Message}"
                    },
                    HasError = true,
                    Result = null
                };
            }
        }

        /// <summary>
        /// Deletes an Appointment
        /// </summary>
        /// <param name="id">Appointment ID</param>
        /// <returns>Boolean indicating success or failure of the delete operation</returns>
        [HttpDelete("{id}")]
        public async Task<BaseResponseModel<bool?>> DeleteAppointment(string id)
        {
            try
            {
                bool deleteResult;

                // Perform physical delete logic
                deleteResult = await _appointmentService.DeleteAsync(id);
                if (!deleteResult)
                {
                    return new BaseResponseModel<bool?>()
                    {
                        ErrorDetails = new ErrorDetailsModel()
                        {
                            ErrorType = ErrorDetailsModel.ErrorTypeEnum.Internal,
                            HttpStatusCode = HttpStatusCodeEnum.NotFound,
                            Message = $"Appointment with ID {id} was not found for physical deletion."
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
                            ? $"Appointment successfully deleted."
                            : $"Failed to  delete the appointment."
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
                        Message = $"An error occurred while deleting the appointment. Details: {ex.Message}"
                    },
                    HasError = true,
                    Result = null
                };
            }
        }
    }
}
