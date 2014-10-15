//------------------------------------------------------
//				IntSig Information Co.,Ltd.
//
// FileName:	TileHelper.cs
// Description:	Methods to help manipulate tiles
// Author:		Adrian Wang
// Created On:	2013-08-29
//------------------------------------------------------

using System;
using System.Linq;
using Microsoft.Phone.Shell;

namespace Archive.Tiles
{
    public static class TileHelper
    {
        /// <summary>
        /// 根据Id判断是否已经创建了这个磁贴
        /// </summary>
        /// <param name="id">需要检测的磁贴Id</param>
        /// <returns></returns>
        public static bool IsPinned(string id)
        {
            return !string.IsNullOrEmpty(id) && ShellTile.ActiveTiles.Any(x => x.NavigationUri.ToString().Contains(id));
        }

        /// <summary>
        /// 使用tileData更新相应Id的动态磁贴
        /// </summary>
        /// <param name="id">需要更新的磁贴的Id</param>
        /// <param name="tileData">需要显示到磁贴上的数据</param>
        /// <returns>是否更新成功</returns>
        public static bool UpdateTile(string id, ShellTileData tileData)
        {
            var tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(id));
            if (tile == null) return false;
            tile.Update(tileData);
            return true;
        }

        /// <summary>
        /// 根据id删除相应磁贴
        /// </summary>
        /// <param name="id">需要删除的磁贴的Id</param>
        /// <returns>是否删除成功</returns>
        public static bool DeleteTile(string id)
        {
            var tile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(id));
            if (tile == null) return false;
            tile.Delete();
            return true;
        }

        /// <summary>
        /// 将磁贴附到开始界面
        /// </summary>
        /// <param name="tile">需要Pin的磁贴</param>
        /// <param name="xamlPath">磁贴将会跳转到的xaml界面的路径，默认为Relative类型</param>
        /// <param name="kind">地址对应Uri类型</param>
        public static void PinToStart(this ShellTileData tile, string xamlPath, UriKind kind = UriKind.Relative)
        {
            var supportWideTile = tile is CycleTileData;
            ShellTile.Create(new Uri(xamlPath, kind), tile, supportWideTile);
        }

        /// <summary>
        /// 将磁贴附到开始界面
        /// </summary>
        /// <param name="tile">需要Pin的磁贴</param>
        /// <param name="xamlUri">磁贴将会跳转到的xaml界面</param>
        public static void PinToStart(this ShellTileData tile, Uri xamlUri)
        {
            var supportWideTile = tile is CycleTileData;
            ShellTile.Create(xamlUri, tile, supportWideTile);
        }
    }
}
