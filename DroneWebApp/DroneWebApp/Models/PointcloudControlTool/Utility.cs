using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.PointcloudControlTool
{
    public class Utility
    {
        public static bool ContainsList(List<List<int>> list, List<int> item)
        {
            bool same = true;
            item.Sort();

            for (int i=0; i<list.Count; i++)
            {
                List<int> temp = list[i];
                if (temp.Count == item.Count)
                {
                    temp.Sort();
                    for (int j=0; j<temp.Count; j++)
                    {
                        if (temp[j] != item[j])
                        {
                            same = false;
                        }
                    }
                }
            }
            return same;
        }
    }
}