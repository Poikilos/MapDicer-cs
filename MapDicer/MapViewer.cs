using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace MapDicer
{
    public class MapViewer : Panel
    {
        #region BasicVisual
        private List<Visual> visuals = new List<Visual>();//collection of the visual objects

        //From Panel Get visual object by index
        protected override Visual GetVisualChild(int index) { return visuals[index]; }

        //From Panel The number of child
        protected override int VisualChildrenCount { get { return visuals.Count; } }

        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }
        #endregion BasicVisual

    }
}
