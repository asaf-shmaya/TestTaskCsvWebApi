using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.ExtensionMethods
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<object> GetPage(IEnumerable<object> input, int page, int pageSize)
        {
            return input.Skip(page * pageSize).Take(pageSize);
        }
    }
}
