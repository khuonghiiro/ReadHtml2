using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadHtml.Constant
{
    public static class Constants
    {
        public const string DIV_CLASS_PARENT = "div.clearfix";

        public const string DIV_CLASS_CHILD = "div.VCSortableInPreviewMode";

        public const string DIV_ATTR_TYPE_PARAM = "div.VCSortableInPreviewMode*[type = '{0}']";

        public const string DIV_ATTR_TYPE = "div.VCSortableInPreviewMode*[type]";

        public const string LINK_TWO_TAG = "{0} > {1}";

        //public const string TAG_DIV_PHOTO = "div*[type='Photo'] >> img";

        public const string TAG_DIV_IMG = "div >> img";

        public const string TAG_A_IMG = "a > img";

        public const string TAG_DIV_P = "div > p";

        public const string TAG_DIV_P_QUOTE = "div > p.quote";

        public const string TAG_ATTR_FILENAME = "data-vid";

        public const string TAG_ATTR_AVATAR = "data-thumb";

        public const string TAG_ATTR_STARTNAME = "p.StarNameCaption";

        public const string START_NAME_CAPTION = "StarNameCaption";

        public const string TAG_P = "p";

        public const string TAG_P_CAP = "p.{0}";

        public const string TAG_DIV_CAP = "div.{0}";

        public const string PATTERN_REGEX = "\"\\w+Caption";

        public const string CAPTION = "Caption";

        public const string WRAP_NOTE = "wrapnote";

        public const string LAYOUT_ALBUM= "LayoutAlbum";

        public const string PHOTO = "Photo";

        public const int INDEX = 1;

    }
}
