using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Windy.WebMVC.Web2.ViewsModels
{
    public class Pagination
    {
        public string Keyword { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }

        public string Order { get; set; }

        public string ActionUrl { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                if (this.TotalCount % this.Size == 0)
                    return this.TotalCount / this.Size;
                else
                    return this.TotalCount / this.Size+1;
            }
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPageIndex { get; set; }
        private int _size = 10;
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int Size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
            }
        }

        private int _PreviousPageIndex = 0;
        /// <summary>
        /// 上一页页码
        /// </summary>
        public int PreviousPageIndex
        {
            get
            {
                this._PreviousPageIndex = this.CurrentPageIndex - 1;
                return this._PreviousPageIndex;
            }
        }
        /// <summary>
        /// 下一页页码
        /// </summary>
        private int _NextPageIndex = 0;
        /// <summary>
        /// 上一页页码
        /// </summary>
        public int NextPageIndex
        {
            get
            {
                this._NextPageIndex = this.CurrentPageIndex + 1;
                return this._NextPageIndex;
            }
        }
        private int _startPage = 0;
        public int StartPage {
            get {
                this._startPage = this.CurrentPageIndex - 2;
                if (this.CurrentPageIndex <= 4)
                {
                    this._startPage = 1;
                }
                return this._startPage;
            }
        }
        private int _endPage = 0;
        public int EndPage
        {
            get
            {
                this._endPage = this.CurrentPageIndex + 2;
                if ((this.TotalPageCount-this.CurrentPageIndex) < 4)
                {
                    this._endPage = this.TotalPageCount;
                }
                if (this._endPage > this.TotalPageCount)
                    this._endPage = this.TotalPageCount;
                return this._endPage;
            }
        }
    }
}