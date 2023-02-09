using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiitHard.Navigation
{
    public class NavigationFlyoutMenuItem
    {
        public NavigationFlyoutMenuItem()
        {
            TargetType = typeof(NavigationFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public Type TargetType { get; set; }
    }
}