namespace VocalSpace.Services
{
  
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // 建構函式，注入 IHttpContextAccessor
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // 獲取當前用戶的 UserId
        public long? UserId
        {
            get
            {
                // 從 Session 中取得 UserId                
                return _httpContextAccessor.HttpContext?.Items["UserId"] as long?;

            }
        }
    }
}
