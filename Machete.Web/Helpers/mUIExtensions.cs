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
        public static string tblabel = "<div class=\"tb-label\">";
        public static string tbfield = "<div class=\"tb-field\">";
        public static string tbclose = "</div>";

        public static MvcHtmlString mUIDropDownYesNoFor<TModel, TBool>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TBool>> expression, 
            //CultureInfo CI, 
            object attribs)
        {            
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            CultureInfo CI = (CultureInfo)htmlHelper.ViewContext.HttpContext.Session["culture"];
            return MvcHtmlString.Create(
                tbfield +
                htmlHelper.DropDownListFor(
                    expression,
                    new SelectList(Lookups.yesno(CI),
                                            "Value",
                                            "Text",
                                            metadata.Model),
                    Shared.choose,
                    attribs
                ).ToString() +
                htmlHelper.ValidationMessageFor(expression) +
                tbclose);
        }

        public static MvcHtmlString mUIDropDownListFor<TModel, TSelect>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            SelectList list,
            object attribs)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return MvcHtmlString.Create(
                tbfield +
                htmlHelper.DropDownListFor(
                    expression,
                    new SelectList(list,
                                "Value",
                                "Text",
                                metadata.Model),
                    Shared.choose,
                    attribs
                ).ToString() +
                htmlHelper.ValidationMessageFor(expression) +
                tbclose);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="list"></param>
        /// <param name="attribs"></param>
        /// <returns></returns>
        public static MvcHtmlString mUITableDropDownListFor<TModel, TSelect>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            SelectList list,
            object attribs)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return MvcHtmlString.Create(
                tbfield +
                htmlHelper.DropDownListFor(
                    expression,
                    new SelectList(list,
                                "Value",
                                "Text",
                                metadata.Model),
                    Shared.choose,
                    attribs
                ).ToHtmlString() +
                htmlHelper.ValidationMessageFor(expression).ToHtmlString() +
                tbclose
                );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString mUITableLabelFor<TModel, TSelect>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression)
        {
            return MvcHtmlString.Create(
                tblabel + 
                htmlHelper.LabelFor(expression).ToHtmlString() +
                tbclose
                );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="attribs"></param>
        /// <returns></returns>
        public static MvcHtmlString mUITableTextBoxFor<TModel, TSelect>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            object attribs)
        {
            var foo = htmlHelper.ValidationMessageFor(expression).ToString();
            return MvcHtmlString.Create(
                tbfield +
                htmlHelper.TextBoxFor(expression, attribs).ToString() +
                htmlHelper.ValidationMessageFor(expression).ToString() +
                tbclose
                );
        }

        public static MvcHtmlString mUITableDateTextBoxFor<TModel, TSelect>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TSelect>> expression,
            object attribs)
        {
            string value = String.Empty;
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            value = (DateTime?)metadata.Model == DateTime.MinValue ? "" : metadata.Model == null ? "" : ((DateTime)metadata.Model).ToShortDateString();
           
            return MvcHtmlString.Create(
            tbfield +
                htmlHelper.TextBox(metadata.PropertyName,
                              htmlHelper.Encode(value),
                              attribs).ToString() +
                htmlHelper.ValidationMessageFor(expression).ToString() +
            tbclose
                );
        }
    }
}

