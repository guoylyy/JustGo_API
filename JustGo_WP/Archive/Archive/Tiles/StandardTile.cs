//------------------------------------------------------
//				IntSig Information Co.,Ltd.
//
// FileName:	CycleTile.cs
// Description:	Customed StandardTile
// Author:		Adrian Wang
// Created On:	2013-08-29
//------------------------------------------------------

using System;
using Microsoft.Phone.Shell;

namespace Archive.Tiles
{
    public class StandardTile : StandardTileData
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="backImagePath"> 正面的背景图</param>
        /// <param name="title">正面标题</param>
        /// <param name="backBackImagePath">背面的背景图</param>
        /// <param name="backTitle">背面标题</param>
        /// <param name="backContent">背面内容</param>
        public StandardTile(string backImagePath, string title = null,
            string backBackImagePath = null, string backTitle = null,
            string backContent = null)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Title = title.TrimStart(new[] { '@' });
            }
            BackTitle = backTitle;
            if (backImagePath == null)
                return;
            BackgroundImage = new Uri("isostore:/" + backImagePath, UriKind.Absolute);
            if (backBackImagePath != null)
                BackBackgroundImage = new Uri("isostore:/" + backBackImagePath, UriKind.Absolute);
            BackContent = backContent;
        }
    }
}
