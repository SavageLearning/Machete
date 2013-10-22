#region COPYRIGHT
// File:     mUIExtensions.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
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
        public static string tbdesclabel = "<div class=\"tb-label desc-label\">";
        public static string tbfield = "<div class=\"tb-field\">";
        public static string tbclose = "</div>";

        public static MvcHtmlString mUIDropDownYesNoFor<TModel, TBool>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TBool>> expression, 
            //CultureInfo CI, 
            object attribs)
        {            
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            CultureInfo CI = (CultureInfo)htmlHelper.ViewContext.HttpContext.Session["culture"];
            var yesNo = Lookups.yesnoSelectList(CI);
            return MvcHtmlString.Create(
                tbfield +
                htmlHelper.DropDownListFor(
                    expression,
                    new SelectList(yesNo,
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
                htmlHelper.ValidationMessageFor(expression).ToString() +
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
                htmlHelper.LabelFor(expression).ToString() +
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
        public static MvcHtmlString mUITableDescLabelFor<TModel, TSelect>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression)
        {
            return MvcHtmlString.Create(
                tbdesclabel +
                htmlHelper.LabelFor(expression).ToString() +
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
        public static MvcHtmlString mUITableLabelAndTextBoxFor<TModel, TSelect>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TSelect>> expression,
            object attribs)
        {
            return MvcHtmlString.Create(mUITableLabelFor(htmlHelper, expression).ToHtmlString() + mUITableTextBoxFor(htmlHelper, expression, attribs).ToHtmlString());
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

