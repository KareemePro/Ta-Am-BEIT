using System.Net;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoodDelivery.Services.Common
{
    public interface IResult<T>
    {
        bool IsSuccess { get; }
        T Data { get; }
        List<string> Errors { get; }
        HttpStatusCode? HttpStatusCode { get; }
    }

    public class SingleResult<T> : IResult<T>
    {
        public bool IsSuccess { get; }
        public T Data { get; }
        public List<string> Errors { get; }
        public HttpStatusCode? HttpStatusCode { get; }

        private SingleResult(bool success, T data, List<string> errors, HttpStatusCode? httpStatusCode)
        {
            IsSuccess = success;
            Data = data;
            Errors = errors;
            HttpStatusCode = httpStatusCode;
        }

        public static SingleResult<T> Success(T data, HttpStatusCode? httpStatusCode = null) => new(true, data, null, httpStatusCode);
        public static SingleResult<T> Failure(List<string> errors, HttpStatusCode? httpStatusCode = null) => new(false, default(T), errors, httpStatusCode);
    }

    public class ListResult<T> : IResult<IEnumerable<T>>
    {
        public bool IsSuccess { get; }
        public IEnumerable<T> Data { get; }
        public List<string> Errors { get; }
        public HttpStatusCode? HttpStatusCode { get; }

        private ListResult(bool success, IEnumerable<T> data, List<string> errors, HttpStatusCode? httpStatusCode)
        {
            IsSuccess = success;
            Data = data;
            Errors = errors;
            HttpStatusCode = httpStatusCode;
        }

        public static ListResult<T> Success(IEnumerable<T> data, HttpStatusCode? httpStatusCode = null) => new(true, data, null, httpStatusCode);
        public static ListResult<T> Failure(List<string> errors, HttpStatusCode? httpStatusCode = null) => new(false, null, errors, httpStatusCode);
    }

    public class CartResult<T> : IResult<T>
    {
        [JsonIgnore]
        public bool IsSuccess { get; }
        public T Data { get; }
        public List<string> Errors { get; }
        [JsonIgnore]
        public HttpStatusCode? HttpStatusCode { get; }

        private CartResult(bool success, T data, List<string> errors, HttpStatusCode? httpStatusCode)
        {
            IsSuccess = success;
            Data = data;
            Errors = errors;
            HttpStatusCode = httpStatusCode;
        }

        public static CartResult<T> Correct(T data, HttpStatusCode? httpStatusCode = null) => new(true, data, null, httpStatusCode);
        public static CartResult<T> Modified(List<string> errors, T data, HttpStatusCode? httpStatusCode = null) => new(false, data, errors, httpStatusCode);
        public static CartResult<T> Failure(List<string> errors, HttpStatusCode? httpStatusCode = null) => new(false, default(T), errors, httpStatusCode);


    }
}