//using AngleSharp;
//using AngleSharp.Dom;
//using AngleSharp.Html.Dom;
//using ReadHtml.Constant;
//using ReadHtml.Models;
//using System.Text.RegularExpressions;

//namespace ReadHtml.Handles
//{
//    public class HandleTagHtml
//    {
//        private string[] tagHtml = new string[] { "p", "h1", "h2", "h3", "h4", "h5" };

//        private int? count = 1;


//        /// <summary>
//        /// Get tag html { "p", "h1", "h2", "h3", "h4", "h5" }
//        /// </summary>
//        /// <param name="index"></param>
//        /// <param name="data"></param>
//        /// <param name="query"></param>
//        /// <returns></returns>
//        public async Task<List<Root>> GetDataDivTag(int? index, IElement? data, string? query)
//        {
//            List<Root> lstData = new();

//            try
//            {
//                foreach (var tag in tagHtml)
//                {
//                    var queryTag = string.Format(Constants.CONCAT_TWO_TAG, query, tag);

//                    var lstQuery = data?.QuerySelectorAll(queryTag);

//                    if (lstQuery?.Length == 0)
//                    {
//                        continue;
//                    }

//                    foreach (var list in lstQuery)
//                    {

//                        Root rt = new Root();

//                        rt = await GetRoot(index, tag, list);

//                        lstData.Add(rt);

//                        index++;
//                        count = index;
//                    }
//                }
//            }
//            catch (NullReferenceException)
//            {

//            }

//            return await Task.FromResult(lstData);
//        }

//        /// <summary>
//        /// Get image in tag html
//        /// </summary>
//        /// <param name="type"></param>
//        /// <param name="data"></param>
//        /// <returns></returns>
//        public async Task<Image?> GetImage(string? type, IElement? data)
//        {
//            string query = string.Empty;
//            string queryOriginalImg = "data-original";

//            if (type == Constants.TYPE_WRAP_NOTE)
//            {
//                string typeTag = string.Format(Constants.DIV_ATTR_TYPE_PARAM, type);

//                query = string.Format(Constants.CONCAT_TWO_TAG, typeTag, Constants.TAG_DIV_IMG);
//            }
//            else if (type == Constants.TYPE_LAYOUT_ALBUM)
//            {
//                queryOriginalImg = "url";

//                query = Constants.TAG_A_IMG;
//            }
//            else
//            {
//                query = Constants.TAG_DIV_IMG;
//            }

//            var dataImg = data?.QuerySelector(query);

//            if (dataImg == null)
//            {
//                return null;
//            }

//            Image img = new Image();

//            Size size = new Size();
//            img.Size = await GetSize(type, dataImg);

//            img.Src = dataImg?.GetAttribute("src");
//            img.OriginalImg = dataImg?.GetAttribute(queryOriginalImg);

//            return await Task.FromResult(img);
//        }

//        /// <summary>
//        /// Get zise in tag html
//        /// </summary>
//        /// <param name="type"></param>
//        /// <param name="data"></param>
//        /// <returns></returns>
//        public async Task<Size?> GetSize(string? type, IElement? data)
//        {

//            Size size = new Size();

//            int w = 0;
//            int h = 0;

//            if (type == "VideoStream")
//            {
//                h = Convert.ToInt32(data?.GetAttribute("data-height")?.Replace("px", ""));

//                w = Convert.ToInt32(data?.GetAttribute("data-width")?.Replace("px", ""));

//            }

//            if (type == "LayoutAlbum" || type == "wrapnote" || type == "Photo")
//            {
//                h = Convert.ToInt32(data?.GetAttribute("h"));

//                w = Convert.ToInt32(data?.GetAttribute("w"));
//            }

//            if (w == 0 && h == 0)
//            {
//                return null;
//            }

//            size.Height = h;
//            size.Width = w;

//            return await Task.FromResult(size);
//        }

//        /// <summary>
//        /// Get list image
//        /// </summary>
//        /// <param name="type"></param>
//        /// <param name="data"></param>
//        /// <returns></returns>
//        public async Task<List<RowImage>?> ListRowImage(string? type, IElement? data)
//        {
//            int countRow = 1;

//            var lstLayoutAlbumRow = data?.QuerySelectorAll("div.LayoutAlbumContent > div.LayoutAlbumRow");

//            if (lstLayoutAlbumRow?.Length == 0)
//            {
//                return null;
//            }

//            List<RowImage> listRowImage = new List<RowImage>();

//            foreach (var lstLayout in lstLayoutAlbumRow)
//            {

//                var lstItem = lstLayout.QuerySelectorAll("figure");

//                RowImage rowImg = new RowImage();
//                List<Image> lstImg = new List<Image>();

//                rowImg.Row = countRow;

//                foreach (var item in lstItem)
//                {
//                    Image? img = new Image();

//                    img = await GetImage(type, item);

//                    lstImg.Add(img);
//                }

//                rowImg.listImage = lstImg;

//                listRowImage.Add(rowImg);

//                countRow++;

//            }

//            return await Task.FromResult(listRowImage);
//        }

//        /// <summary>
//        /// Get 1 object in tag html
//        /// </summary>
//        /// <param name="index"></param>
//        /// <param name="type"></param>
//        /// <param name="data"></param>
//        /// <returns></returns>
//        public async Task<Root> GetRoot(int? index, string? type, IElement? data)
//        {

//            var lstClass = data?.LastElementChild?.ClassList;

//            string valueClass = "h6";

//            if (lstClass != null && lstClass?.Length != 0)
//            {

//                foreach (var value in lstClass)
//                {

//                    if (value.Contains(Constants.CAPTION) && value != Constants.START_NAME_CAPTION)
//                    {
//                        valueClass = value;
//                        break;
//                    }

//                }
//            }

//            string queryType = string.Format(Constants.DIV_ATTR_TYPE_PARAM, type);

//            string queryQuote = string.Format(Constants.CONCAT_TWO_TAG, queryType, Constants.TAG_DIV_P_QUOTE);

//            string queryCap = string.Format(Constants.TAG_DIV_CAP, valueClass);

//            Root root = new Root();

//            //var rx = new Regex(Constants.PATTERN_REGEX, RegexOptions.Compiled);


//            //var match = rx.Match(data.InnerHtml);

//            //if (match.Success)
//            //{
//            //    

//            //}

//            root.Type = type;
//            root.Caption = data?.QuerySelector(queryCap)?.TextContent;
//            root.FileName = data?.GetAttribute(Constants.TAG_ATTR_FILE_NAME);
//            root.Avatar = data?.GetAttribute(Constants.TAG_ATTR_AVATAR);
//            root.Quote = data?.QuerySelector(queryQuote)?.TextContent;
//            root.StarNameCaption = data?.QuerySelector(Constants.TAG_ATTR_START_NAME)?.TextContent;
//            root.Size = await GetSize(type, data);

//            root.ListRowImage = await ListRowImage(type, data);

//            int i = 1;

//            if (type == Constants.TYPE_WRAP_NOTE)
//            {
//                root.ListValue = await GetListRoot(i, data, queryType);
//            }

//            if (type == Constants.TYPE_PHOTO)
//            {
//                //index++;
//                root.Image = await GetImage(type, data);

//            }

//            foreach (var tag in tagHtml)
//            {
//                if (type == tag)
//                {
//                    root.Value = data?.TextContent;
//                }
//                else
//                {
//                    continue;
//                }
//            }

//            root.Index = index;

//            return root;
//        }

//        /// <summary>
//        /// Get list object in tag html
//        /// </summary>
//        /// <param name="index"></param>
//        /// <param name="data"></param>
//        /// <param name="query"></param>
//        /// <returns></returns>
//        public async Task<List<Root>> GetListRoot(int? index, IElement data, string query)
//        {
//            List<Root> list = new List<Root>();

//            List<Root> listRoot = new();

//            //var allDivType = doc.All.Where(m => m.LocalName == "div" && m.HasAttribute("type"));

//            var queryConcat = string.Format(Constants.CONCAT_TWO_TAG, query, Constants.DIV_ATTR_TYPE);

//            var allDivType = data.QuerySelectorAll(queryConcat);

//            listRoot = await GetDataDivTag(index, data, query);

//            int? i = count;

//            index = i;

//            foreach (var div in allDivType)
//            {
//                Root root = new Root();
//                var type = div.GetAttribute("type");

//                root = await GetRoot(index, type, div);

//                listRoot.Add(root);

//                index++;
//            }

//            return listRoot;
//        }
//    }
//}
