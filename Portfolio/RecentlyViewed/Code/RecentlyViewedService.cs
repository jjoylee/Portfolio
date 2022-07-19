namespace RecentlyViewed
{
    public class RecentlyViewedService
    {
        private const int MAX_COUNT = 5;

        public IList<RecentlyViewedItem> GetRecentlyViewedItemsFromCookie(HttpRequestBase request)
        {
            try {
                if (!IsCookieExist(request)) return new List<RecentlyViewedItem>();
                return JsonConvert.DeserializeObject<List<RecentlyViewedItem>>(request.Cookies["RecentlyViewed"].Value);
            }
            catch (Exception e) { 
                // Cookie의 Value가 Json 형식이 아닐 경우
                return new List<RecentlyViewedItem>();
            }
        }

        private bool IsCookieExist(HttpRequestBase request)
        {
            return request.Cookies["COOKIENAME"] != null && !string.IsNullOrEmpty(request.Cookies["COOKIENAME"].Value);
        }

        public void SetRecentlyViewedItemsInCookie(HttpRequestBase request, HttpResponseBase response, ProductItem productItem)
        {
            IList<RecentlyViewedItem> recentlyViewedItems = GetRecentlyViewedItemsFromCookie(request);
            var newItem = new RecentlyViewedItem() { Id = productItem.Id, Url = HttpUtility.UrlEncode(request.Url.PathAndQuery.ToString()), Name = productItem.Name };
            AddRecentlyViewedItem(recentlyViewedItems, newItem);
            SetInCookie(recentlyViewedItems, response, request);
        }

        private void SetInCookie(IList<RecentlyViewedItem> recentlyViewedItems, HttpResponseBase response, HttpRequestBase request)
        {
            request.Cookies.Remove("COOKIENAME");
            HttpCookie RecentlyViewedCookie = new HttpCookie("COOKIENAME");
            RecentlyViewedCookie.Value = JsonConvert.SerializeObject(recentlyViewedItems);
            RecentlyViewedCookie.Expires = DateTime.Now.AddDays(14);
            response.Cookies.Add(RecentlyViewedCookie);
        }

        private void AddRecentlyViewedItem(IList<RecentlyViewedItem> recentlyViewedItems, RecentlyViewedItem newItem)
        {
            recentlyViewedItems.Remove(newItem);
            recentlyViewedItems.Insert(0, newItem);
            if (recentlyViewedItems.Count > MAX_COUNT) recentlyViewedItems.RemoveAt(5);
        }
    }
}
