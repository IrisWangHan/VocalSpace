namespace VocalSpace.Services
{
    //未來如果要換成其他分頁套件，只需要修改這個Service
    public interface IPaginationService
    {
        PaginationResult<T> GetPaginationToLinq<T>(List<T> dataList, int currentPage, int pageSize);
    }

    /// <summary>
    /// 分頁服務 傳入參數(List物件,當前的頁數,每頁顯示幾筆)
    /// </summary>
    public class PaginationService : IPaginationService
    {
        public PaginationResult<T> GetPaginationToLinq<T>(List<T> dataList, int currentPage, int pageSize)
        {

            var DataList = dataList
            .Skip((currentPage - 1) * pageSize) // 跳過前 (2-1)*10 筆
            .Take(pageSize) // 取 10 筆
            .ToList();      
            
            return new PaginationResult<T>
            {
                TotalPage = dataList.Count(),
                CurrentPage = currentPage,
                PageSize = pageSize,
                PaginationCount= (int)Math.Ceiling((double)dataList.Count() / pageSize),//分頁數量:有餘數要加1
                StartPageNumber = (currentPage - 1) * pageSize+1,
                EndPageNumber = currentPage * pageSize,
                DataList= DataList
            };
        }
    }

    public class PaginationResult<T>
    {
        public int TotalPage { get; set; }          // 總筆數
        public int CurrentPage { get; set; }        // 目前頁數
        public int PageSize { get; set; }           // 每頁筆數
        public int PaginationCount { get; set; }    // 總分頁數
        public int StartPageNumber { get; set; }    // 第 [起始頁數]
        public int EndPageNumber { get; set; }      // 第 [結束頁數]
        public List<T> DataList { get; set; } = new List<T>(); //回傳的資料
    }


}
