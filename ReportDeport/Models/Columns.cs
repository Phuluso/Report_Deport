using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportDeport.Models
{
    public class columnItem
    {

        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public string ReportName { get; set; }
        public bool IsChecked { get; set; }

    }

    public class columnItemList
    {
        public List<columnItem> columns { get; set; }
    }

    public class columnItemListTranslations
    {
        public List<columnItem> columns { get; set; }
        public List<columnTranslation> translations { get; set; }
    }


    public class allModels
    {
        public List<CourseViewModel> courses { get; set; }
        public List<CategoryViewModel> categories { get; set; }

        public List<EnrolViewModel> enrol { get; set; }
        public List<columnItem> columns { get; set; }

        public List<columnTranslation> columnTrans { get; set; }
    }


}