using AngleSharp.Dom;
using ReadHtml.Constant;
using ReadHtml.Models;
using System.Text.RegularExpressions;

namespace ReadHtml.Handles
{
    public class HandleHtml
    {

        /// <summary>
        /// Get object Size of width and height in tag img
        /// </summary>
        /// <param name="type">Attribute type</param>
        /// <param name="data">Element data</param>
        /// <returns>Object Size</returns>
        private async Task<Size?> GetSize(string? type, IElement data)
        {
            Size size = new ();

            try
            {
                int width = 0;
                int height = 0;

                if (type == Constants.TYPE_VIDEO_STREAM)
                {
                    height = Convert.ToInt32(data.GetAttribute(Constants.DATA_HEIGHT)?.Replace("px", ""));

                    width = Convert.ToInt32(data.GetAttribute(Constants.DATA_WIDTH)?.Replace("px", ""));
                }

                if (type == Constants.TYPE_LAYOUT_ALBUM || 
                    type == Constants.TYPE_WRAP_NOTE || 
                    type == Constants.TYPE_PHOTO)
                {
                    height = Convert.ToInt32(data.GetAttribute(Constants.HEIGHT));

                    width = Convert.ToInt32(data.GetAttribute(Constants.WIDTH));
                }

                if (width == 0 && height == 0)
                {
                    return null;
                }

                size.Height = height;
                size.Width = width;

            }
            catch(NullReferenceException)
            {
                return null;
            }

            return await Task.FromResult(size);
        }

        /// <summary>
        /// Get object Image in tag img
        /// </summary>
        /// <param name="type">Attribute type</param>
        /// <param name="data">Element data</param>
        /// <returns>Object Image</returns>
        private async Task<Image?> GetImage(string? type, IElement data)
        {
            Image img = new();

            try
            {
                string querySelector = string.Empty;

                string queryOriginalImg = Constants.DATA_ORIGINAL;

                if (type == Constants.TYPE_WRAP_NOTE)
                {
                    string typeTag = string.Format(Constants.DIV_ATTR_TYPE_PARAM, type);

                    querySelector = string.Format(Constants.CONCAT_TWO_TAG_PARAM, typeTag, Constants.TAG_DIV_IMG);
                }
                else if (type == Constants.TYPE_LAYOUT_ALBUM)
                {
                    queryOriginalImg = Constants.URL;

                    querySelector = Constants.TAG_A_IMG;
                }
                else
                {
                    querySelector = Constants.TAG_DIV_IMG;
                }

                var dataImg = data.QuerySelector(querySelector);

                if (dataImg == null)
                {
                    return null;
                }

                img.Size = await GetSize(type, dataImg);

                img.Src = dataImg.GetAttribute(Constants.SRC);
                img.OriginalImg = dataImg.GetAttribute(queryOriginalImg);

            }
            catch(NullReferenceException)
            {
                return null;
            }

            return await Task.FromResult(img);
        }

        /// <summary>
        /// List row image
        /// </summary>
        /// <param name="type">Attribute type</param>
        /// <param name="data">Element data</param>
        /// <returns>List object RowImage</returns>
        private async Task<List<RowImage>?> ListRowImage(string? type, IElement data)
        {
            List<RowImage> listRowImage = new ();

            try
            {
                int countRow = 1;

                var lstLayoutAlbumRow = data.QuerySelectorAll(Constants.DIV_CHILD_LAYOUT_ALBUM);

                if (lstLayoutAlbumRow.Length == 0)
                {
                    return null;
                }

                foreach (var lstLayout in lstLayoutAlbumRow)
                {

                    var lstItem = lstLayout.QuerySelectorAll(Constants.FIGURE);

                    RowImage rowImg = new();
                    List<Image> lstImg = new();

                    rowImg.Row = countRow;

                    foreach (var item in lstItem)
                    {
                        Image? img = new ();

                        img = await GetImage(type, item);

                        if(img != null)
                        {
                            lstImg.Add(img);
                        }
                    }

                    rowImg.listImage = lstImg;

                    listRowImage.Add(rowImg);

                    countRow++;

                }
            }
            catch (NullReferenceException)
            {
                return null;
            }
            
            return await Task.FromResult(listRowImage);
        }

        /// <summary>
        /// Get object Root
        /// </summary>
        /// <param name="type">Attribute type</param>
        /// <param name="data">Element data</param>
        /// <returns>Object Root</returns>
        private async Task<Root> GetRoot(int index, IElement data)
        {
            Root root = new();

            try
            {
                // 
                if (data.GetType().Name == Constants.HTML_DIV_ELEMENT)
                {
                    var type = data.GetAttribute(Constants.TYPE);
                    string? valueCap = ValueRegex(data);

                    root.Type = type;
                    root.Index = index;
                    root.Value = null;
                    root.FileName = data.GetAttribute(Constants.TAG_ATTR_FILE_NAME);
                    root.Avatar = data.GetAttribute(Constants.TAG_ATTR_AVATAR);
                    root.Quote = data.QuerySelector(Constants.TAG_P_QUOTE)?.TextContent;
                    root.Size = await GetSize(type, data);
                    root.ListRowImage = await ListRowImage(type, data);
                    root.Caption = ValueCaption(1, valueCap, data);
                    root.StarNameCaption = ValueCaption(2, valueCap, data);

                    if (type == Constants.TYPE_WRAP_NOTE)
                    {
                        root.ListValue = await GetListRoot(data);
                        root.Caption = null;
                    }

                    if (type == Constants.TYPE_PHOTO)
                    {
                        root.Image = await GetImage(type, data);
                    }
                }
                else
                {
                    root.Type = data.NodeName;
                    root.Index = index;
                    root.Value = data.InnerHtml;
                }

                index++;
            }
            catch (Exception)
            {
                Console.WriteLine("Error get object Root");
            }

            return root;
        }

        /// <summary>
        /// Get list object Root
        /// </summary>
        /// <param name="data">Element data</param>
        /// <returns>List object Root</returns>
        public async Task<List<Root>?> GetListRoot(IElement data)
        {
            List<Root> listRoot = new();

            int count = Constants.NUMBER_ONE;

            try
            {
                foreach (var items in data.Children)
                {
                    listRoot.Add(await GetRoot(count, items));
                    count++;
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Error get list root");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return await Task.FromResult(listRoot);
        }

        /// <summary>
        /// Get value match with regex
        /// </summary>
        /// <param name="data">Element data</param>
        /// <returns>String text match with regex</returns>
        private string? ValueRegex(IElement data)
        {
            var rx = new Regex(Constants.PATTERN_REGEX, RegexOptions.Compiled);

            var match = rx.Match(data.OuterHtml);

            if(match.Success)
            {
                return match.Value;
            }

            return null;
        }

        /// <summary>
        /// Get value Caption or StarNameCaption
        /// </summary>
        /// <param name="number">Choose number = 1 return value Caption or number = 2 return value StarNameCaption</param>
        /// <param name="valueRegex">Value regex</param>
        /// <param name="data">Element data</param>
        /// <returns>String text of Caption or StarNameCaption</returns>
        private string? ValueCaption(int number, string? valueRegex, IElement data)
        {
            if(data != null && valueRegex != null)
            {
                switch(number)
                {
                    // Get caption
                    case 1:
                        if (valueRegex.Contains(Constants.CAPTION) && valueRegex != Constants.START_NAME_CAPTION)
                        {
                            string? selector = string.Format(Constants.TAG_DIV_CAP_PARAM, valueRegex);

                            var text = data?.QuerySelector(selector)?.TextContent;

                            if (string.IsNullOrEmpty(text))
                            {
                                selector = string.Format(Constants.TAG_P_CAP_PARAM, valueRegex);

                                return data?.QuerySelector(selector)?.TextContent;
                            }

                            return text;
                        }
                        break;

                    //Get StarNameCaption
                    case 2:
                        if (valueRegex.Contains(Constants.CAPTION) && valueRegex == Constants.START_NAME_CAPTION)
                        {
                            string? selector = string.Format(Constants.TAG_P_CAP_PARAM, valueRegex);

                            return data?.QuerySelector(selector)?.TextContent;
                        }
                        break;

                    default: return null;
                }
            }
            return null;
        }
    }
}
