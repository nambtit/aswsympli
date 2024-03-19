using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Factories
{
    public interface ISearchHttpClientFactory
    {
        HttpClient CreateSearchHttpClient();
    }
}
