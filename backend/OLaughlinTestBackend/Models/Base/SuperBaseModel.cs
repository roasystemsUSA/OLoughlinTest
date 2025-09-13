using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Models.Base
{
    public partial class SuperBaseModel
    {

        /// <summary>
        /// Convert the Model into a JSON format string
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert
                    .SerializeObject(this,
                                     Formatting.Indented,
                                     new JsonSerializerSettings
                                     {
                                         ContractResolver = new CamelCasePropertyNamesContractResolver()
                                     });
        }
    }
}
