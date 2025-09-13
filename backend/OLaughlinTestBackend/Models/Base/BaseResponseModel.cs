using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Base
{
    /// <summary>
    /// Base response model for all request of this API
    /// </summary>
    /// <typeparam name="T">The type od the property Result. </typeparam>
    public partial class BaseResponseModel<T> : SuperBaseModel
    {
        public BaseResponseModel()
        {
            HasError = false;
        }
        /// <summary>
        /// Contains the result you want to return to the client
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Indicate if there is an error in the request or not
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Error details if HasError = true
        /// </summary>
        public ErrorDetailsModel ErrorDetails { get; set; }


    }
}
