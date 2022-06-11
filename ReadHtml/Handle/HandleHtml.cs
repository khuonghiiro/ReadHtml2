using AngleSharp.Dom;
using ReadHtml.Constant;
using ReadHtml.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadHtml.Handle
{
    public class HandleHtml
    {
        public bool isCheckType(string type)
        {

            return false;
        }

        public async Task<Size?> GetSize(string? type, IElement? data)
        {

            Size size = new Size();

            int width = 0;
            int height = 0;

            if (type == "VideoStream")
            {
                height = Convert.ToInt32(data?.GetAttribute(Constants.DATA_HEIGHT)?.Replace("px", ""));

                width = Convert.ToInt32(data?.GetAttribute(Constants.DATA_WIDTH)?.Replace("px", ""));

            }

            if (type == "LayoutAlbum" || type == "wrapnote" || type == "Photo")
            {
                height = Convert.ToInt32(data?.GetAttribute(Constants.HEIGHT));

                width = Convert.ToInt32(data?.GetAttribute(Constants.WIDTH));
            }

            if (width == 0 && height == 0)
            {
                return null;
            }

            size.Height = height;
            size.Width = width;

            return await Task.FromResult(size);
        }

        public async Task<Root?> GetAllData(string? type, IElement data)
        {
            Root root = new Root();

            try
            {
                foreach (var item in data.ChildNodes)
                {
                    if (item.GetType().Name == "TextNode")
                    {
                        continue;
                    }

                    if(item.GetType().Name == "HtmlDivElement")
                    {

                    }

                    root.Type = item.NodeName;


                }
            }
            catch(NullReferenceException) 
            { 

            }

            

            return await Task.FromResult(root);
        }

    }
}
