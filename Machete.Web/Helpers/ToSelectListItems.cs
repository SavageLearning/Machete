using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Domain;
namespace Machete.Helpers
{
    public static class ToSelectListItemsHelper
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<Category> categories, int  selectedId)
        {
            return

                categories.OrderBy(category => category.Name)
                      .Select(category =>
                          new SelectListItem
                          {
                              Selected = (category.CategoryId == selectedId),
                              Text = category.Name,
                              Value = category.CategoryId.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
                this IEnumerable<Race> races, int selectedId)
        {
            return

                races.OrderBy(race => race.RaceID)
                      .Select(race =>
                          new SelectListItem
                          {
                              Selected = (race.RaceID == selectedId),
                              Text = race.racelabel,
                              Value = race.RaceID.ToString()
                          });
        }
    }
}