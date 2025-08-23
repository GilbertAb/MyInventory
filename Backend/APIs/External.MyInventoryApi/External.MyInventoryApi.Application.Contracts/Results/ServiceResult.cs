using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.MyInventoryApi.Application.Contracts.Results
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static ServiceResult<T> Success(T data) =>
            new ServiceResult<T> { Data = data, ErrorCode = 0, ErrorMessage = "OK" };

        public static ServiceResult<T> Fail(int errorCode, string errorMessage) =>
            new ServiceResult<T> { Data = default, ErrorCode = errorCode, ErrorMessage = errorMessage };
    }
}
