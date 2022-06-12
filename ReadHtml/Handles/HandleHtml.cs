using AngleSharp.Dom;
using Newtonsoft.Json;
using ReadHtml.Common;
using ReadHtml.Constant;
using ReadHtml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadHtml.Handles
{
    public class HandleHtml
    {

        /// <summary>
        /// Get object size of width and height in tag img
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<Size?> GetSize(string? type, IElement data)
        {
            Size size = new ();

            try
            {
                int width = 0;
                int height = 0;

                if (type == "VideoStream")
                {
                    height = Convert.ToInt32(data.GetAttribute(Constants.DATA_HEIGHT)?.Replace("px", ""));

                    width = Convert.ToInt32(data.GetAttribute(Constants.DATA_WIDTH)?.Replace("px", ""));

                }

                if (type == "LayoutAlbum" || type == "wrapnote" || type == "Photo")
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
                Console.WriteLine("error size");

                return null;
            }

            return await Task.FromResult(size);
        }

        /// <summary>
        /// Get object image in tag img
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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

                    querySelector = string.Format(Constants.CONCAT_TWO_TAG, typeTag, Constants.TAG_DIV_IMG);
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
                Console.WriteLine("error image");

                return null;

            }

            return await Task.FromResult(img);
        }


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
                Console.WriteLine("error list row");
                return null;
            }
            
            return await Task.FromResult(listRowImage);
        }

        private async Task<Root> GetRoot(int index, IElement data)
        {
            Root root = new();

            try
            {
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
                Console.WriteLine("Error root");
            }

            return root;
        }

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
                return null;
            }

            return await Task.FromResult(listRoot);
        }

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

        private string? ValueCaption(int number, string? value, IElement data)
        {
            if(data != null && value != null)
            {
                switch(number)
                {
                    case 1:
                        if (value.Contains(Constants.CAPTION) && value != Constants.START_NAME_CAPTION)
                        {
                            string? selector = string.Format(Constants.DOT_TWO_TAG, Constants.TAG_DIV, value);

                            var text = data?.QuerySelector(selector)?.TextContent;

                            if (string.IsNullOrEmpty(text))
                            {
                                selector = string.Format(Constants.DOT_TWO_TAG, Constants.TAG_P, value);

                                return data?.QuerySelector(selector)?.TextContent;
                            }

                            return text;
                        }
                        break;

                    case 2:
                        if (value.Contains(Constants.CAPTION) && value == Constants.START_NAME_CAPTION)
                        {
                            string? selector = string.Format(Constants.DOT_TWO_TAG, Constants.TAG_P, value);

                            return data?.QuerySelector(selector)?.TextContent;
                        }
                        break;
                }
            }
            return null;
        }
    }
}
