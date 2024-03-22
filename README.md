# aswsympli

# About

This solution is for querying the search engines for SEO rank of a URL based on certain input keyword. It is required that no search APIs be used, and extracting the HTML respose from separate search engine to do the task instead. Currently Google and Bing are supported as search engines.

There is a dashboard to show SEO rank data per keyword for ech supported search engine, and the time when the rank is recorded. It automatically reload to fetch new data (if any) every 30 seconds.

# Solution structure

The solution structure is inspired by Clean Architecture style for loose coupling benefit. The APIs are consumed by the a Web UI built with AspNet Razor page to show SEO rank in a dashboard. Also within the Web application there is a background process to trigger the API for refreshing SEO rank data (by checking with the search engines).

# Libraries and framework

The solution is built with .NET 8 and as a requirement, no 3rd party libraries should be used.

- SQLite for Persistence
- .NET Memory Cache for data caching
- XUnit for unit testing
- FluentAssertion for unit test assertion
- Moq for unit test mocking

# Notes

There are a number of configurations for the application. Among them there are few consideration when setting values for the config.

- `SEORankDataRefreshFrequencyMinutes`: The interval for checking with the search engines for updated SEO rank data. This is by default set to `60` minutes. Too frequent interval could cause the search engines to reject the requests and respond `429: Too Many Requests` error.
- `MaxResultPerPage`: The relative maximum number of results for the search engine to return for every search. This is RELATIVE because search engine could respond fewer than requested (as noted in the `count` param [here](https://learn.microsoft.com/en-us/bing/search-apis/bing-web-search/reference/query-parameters), same for Google search per our experiment)

# Improvements

- Increase test coverage.
- As soon as new data is updated, trigger an event for the cache to reset itself.
- Optimize the rank extractor implementations so that the filtering of character stream is smarter.
- Containerization the apps.

# References
- [Google Search URL Parameters](https://aicontentfy.com/en/blog/demystifying-google-search-url-parameters-and-how-to-use-them)
- [Bing Search URL Parameters](https://learn.microsoft.com/en-us/bing/search-apis/bing-web-search/reference/query-parameters)