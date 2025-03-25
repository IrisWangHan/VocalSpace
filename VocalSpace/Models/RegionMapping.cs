namespace VocalSpace.Models
{
    public static class RegionMapping
    {
        // 台灣各縣市的區域分類，按照按鈕的 data-region 屬性分類
        public static Dictionary<string, string> RegionDict= new Dictionary<string, string>
        {
            // 北部 (Northern)
            { "台北市", "north" },
            { "新北市", "north" },
            { "基隆市", "north" },
            { "桃園市", "north" },
            { "新竹市", "north" },
            { "新竹縣", "north" },
            { "宜蘭縣", "north" },

            // 中部 (Central)
            { "台中市", "central" },
            { "彰化縣", "central" },
            { "南投縣", "central" },
            { "雲林縣", "central" },

            // 南部 (Southern)
            { "台南市", "south" },
            { "高雄市", "south" },
            { "屏東縣", "south" },

            // 東部 (Eastern)
            { "台東縣", "east" },
            { "花蓮縣", "east" },

            // 外島 (Outlying Islands)
            { "金門縣", "outlying" },
            { "馬祖", "outlying" },
            { "澎湖縣", "outlying" },
            { "連江縣", "outlying" },

            // 其他 (Other)
            { "其他", "other" }
        };
    }
}
