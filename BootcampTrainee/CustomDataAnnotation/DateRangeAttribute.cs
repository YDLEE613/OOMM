namespace BootcampTrainee.CustomDataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute(string minValue)
            : base(typeof(DateTime), minValue, DateTime.Now.ToShortDateString())
        {

        }
    }
}