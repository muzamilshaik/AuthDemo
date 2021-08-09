using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemo.Dto
{
    public class ResultDto<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }
}
