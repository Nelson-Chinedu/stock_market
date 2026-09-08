// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace dotnet_api_learning.Helpers
// {
//     public class PagedResponse<T>
//     {
//         public int PageNumber { get; set; }
//         public int PageSize { get; set; }
//         public int TotalRecords { get; set; }
//         public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
//         public List<T> Data { get; set; }
        
//         public PagedResponse(List<T> data, int totalRecords, int pageNumber, int pageSize)
//         {
//             Data = data;
//             TotalRecords = totalRecords;
//             PageNumber = pageNumber;
//             PageSize = pageSize;
//         }
//     }
// }

namespace dotnet_api_learning.Helpers;

public class PagedResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    public List<T> Data { get; set; }

    public PagedResponse(List<T> data, int totalRecords, int pageNumber, int pageSize)
    {
        Data = data;
        TotalRecords = totalRecords;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    // Helper method to map PagedResponse<T> to PagedResponse<TNext>
    public PagedResponse<TNext> Map<TNext>(Func<T, TNext> mapFunc)
    {
        var mappedData = Data.Select(mapFunc).ToList();
        return new PagedResponse<TNext>(mappedData, TotalRecords, PageNumber, PageSize);
    }
}