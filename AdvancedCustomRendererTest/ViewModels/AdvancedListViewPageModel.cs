using System;
using PropertyChanged;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedCustomRendererTest
{
    public class Item
    {
        public string Title { get; set; }
        public string Note { get; set; }
    }

    [ImplementPropertyChanged]
    public class AdvancedListViewPageModel
    {
        public IList<Item> ListItems { get; set; }

        public float ListPosition { get; set; }

        public Item SelectedListItem { get; set; }

        public AdvancedListViewPageModel()
        {
            ListItems = Enumerable
                .Range(0, 20)
                .Select(x => new Item
                {
                    Title = "Item" + x.ToString(),
                    Note = "Note" + x.ToString()
                })
                .ToList();

        }


    }
}

