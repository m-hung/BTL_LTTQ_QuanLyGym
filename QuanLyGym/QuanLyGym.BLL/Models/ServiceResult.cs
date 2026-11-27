using System;
using System.Collections.Generic;

namespace QuanLyGym.BLL.Models
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string> FieldErrors { get; set; } = new Dictionary<string, string>();

        public static ServiceResult Ok(string message = "") => new ServiceResult
        {
            Success = true,
            Message = message
        };

        public static ServiceResult Fail(string message, Dictionary<string, string>? fieldErrors = null) => new ServiceResult
        {
            Success = false,
            Message = message,
            FieldErrors = fieldErrors ?? new Dictionary<string, string>()
        };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Ok(T data, string message = "") => new ServiceResult<T>
        {
            Success = true,
            Message = message,
            Data = data
        };

        public new static ServiceResult<T> Fail(string message, Dictionary<string, string>? fieldErrors = null) => new ServiceResult<T>
        {
            Success = false,
            Message = message,
            FieldErrors = fieldErrors ?? new Dictionary<string, string>()
        };
    }
}
