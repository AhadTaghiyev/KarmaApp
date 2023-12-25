using System;
namespace Karma.Service.Responses
{
	public class CommonResponse
	{
		public int StatusCode { get; set; }
		public string Message { get; set; } = null!;
    }
}

