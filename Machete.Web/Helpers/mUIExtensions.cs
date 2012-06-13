using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Web.Mvc.Html;
using System.Globalization;
using Machete.Web.Resources;

namespace Machete.Web.Helpers
{    
    public static class mUIExtensions
    {
        public static MvcHtmlString mUIDropDownYesNoFor<TModel, TBool>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TBool>> expression, 
            //CultureInfo CI, 
            object attribs)
        {            
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            CultureInfo CI = (CultureInfo)htmlHelper.ViewContext.HttpContext.Session["culture"];
            
            return htmlHelper.DropDownListFor(
                expression,
                new SelectList(Lookups.yesno(CI),
                                        "Value",
                                        "Text",
                                        metadata.Model),
                Shared.choose,
                attribs
                );
        }

        public static MvcHtmlString mUIDropDownListFor<TModel, TSelect>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            SelectList list,
            object attribs)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.DropDownListFor(
                expression,
                new SelectList(list,
                            "Value",
                            "Text",
                            metadata.Model),
                Shared.choose,
                attribs
                );
        }
    }
}

