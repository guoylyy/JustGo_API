//------------------------------------------------------
//				IntSig Information Co.,Ltd.
//
// FileName:	CycleTile.cs
// Description:	Customed CycleTile
// Author:		Adrian Wang
// Created On:	2013-08-29
//------------------------------------------------------

using System;
using Microsoft.Phone.Shell;

namespace Archive.Tiles
{
    public class CycleTile : CycleTileData
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cycleImagePaths">需要显示的图片的Path集合，默认为Relative类型</param>
        /// <param name="title">需要显示的标题</param>
        public CycleTile(string[] cycleImagePaths, string title = null)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Title = title.TrimStart(new[] { '@' });
            }
            if (cycleImagePaths.Length == 0)
                return;
            var uris = new Uri[cycleImagePaths.Length];
            for (int i = 0; i < uris.Length; i++)
            {
                uris[i] = new Uri("isostore:/" + cycleImagePaths[i], UriKind.Absolute);
            }
            CycleImages = uris;
            SmallBackgroundImage = uris[0];
        }
    }
}
