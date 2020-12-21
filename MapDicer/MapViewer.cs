using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MapDicer
{
    /// <summary>
    /// This is a special panel that accepts images.
    /// See http://windowspresentationfoundationinfo.blogspot.com/2014/07/wpf-visuals.html
    /// for a tutorial on accepting visuals.
    /// </summary>
    public class MapViewer : Panel
    {
        private double zoomPPS = 32;
        private Point pan = new Point(0, 0);
        private MapDicerPos lastDrawnPos = new MapDicerPos();
        private bool isNewDatabase = true;
        public bool IsNewDatabase
        {
            set
            {
                isNewDatabase = value;
            }
            get
            {
                return isNewDatabase;
            }
        }
        public MapDicerPos LastDrawnPos
        {
            get
            {
                return lastDrawnPos;
            }
        }
        /// <summary>
        /// The zoom is in terms of the number of pixels per sample.
        /// </summary>
        public double ZoomPPS
        {
            get
            {
                return zoomPPS;
            }
        }
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

        internal Point GetWorldPos(Point relativeMVPoint)
        {
            return new Point
            {
                X = Math.Floor((relativeMVPoint.X + pan.X) / zoomPPS),
                Y = Math.Floor((relativeMVPoint.Y + pan.Y) / zoomPPS),
            };
        }

        internal MapDicerPos GetMapDicerPos(Point worldPoint, short lodId, short layerId) {
            return new MapDicerPos
            {
                LodId = lodId,
                LayerId = layerId,
                X = (short)worldPoint.X,
                Z = (short)worldPoint.Y, // ground plane is X-Z as per OpenGL
            };
        }
        internal MapDicerPos GetWorldMapDicerPos(Point relativeMVPoint, short lodId, short layerId)
        {
            return GetMapDicerPos(GetWorldPos(relativeMVPoint), lodId, layerId);
        }


        private void DrawSquare(DrawingVisual visual, Point relativeMVPoint, bool currentlySelected)
        {
            using (DrawingContext dc = visual.RenderOpen())
            {
                Brush brush = Brushes.Red;

                if (currentlySelected)
                { brush = Brushes.Yellow; }

                dc.DrawRectangle(brush, new Pen(Brushes.Black, 5), new Rect(relativeMVPoint, new Size(zoomPPS, zoomPPS)));//draw square from constants
            }
        }

        private bool IsNewWrite(MapDicerPos mpos, bool markAsWritten)
        {
            bool changed = false;
            if (mpos.X != lastDrawnPos.X)
                changed = true;
            else if (mpos.Z != lastDrawnPos.Z)
                changed = true;
            else if (mpos.LodId != lastDrawnPos.LodId)
                changed = true;
            else if (mpos.LayerId != lastDrawnPos.LayerId)
                changed = true;
            if (changed && markAsWritten)
            {
                MarkAsWritten(mpos);
            }
            return changed;
        }

        public bool IsNewWrite(MapDicerPos mpos)
        {
            if (isNewDatabase)
                return true;
            bool changed = false;
            if (mpos.X != lastDrawnPos.X)
                changed = true;
            else if (mpos.Z != lastDrawnPos.Z)
                changed = true;
            else if (mpos.LodId != lastDrawnPos.LodId)
                changed = true;
            else if (mpos.LayerId != lastDrawnPos.LayerId)
                changed = true;
            return changed;
        }
        public void MarkAsWritten(MapDicerPos mpos)
        {
            isNewDatabase = false;
            lastDrawnPos.LodId = mpos.LodId;
            lastDrawnPos.LayerId = mpos.LayerId;
            lastDrawnPos.X = mpos.X;
            lastDrawnPos.Z = mpos.Z;
        }

        private void DrawImage(DrawingVisual visual, ImageSource imageSource, Point relativeMVPoint, bool currentlySelected)
        {
            using (DrawingContext dc = visual.RenderOpen())
            {
                Rect rect = new Rect
                {
                    Location = relativeMVPoint,
                    Width = zoomPPS,
                    Height = zoomPPS,
                };
                dc.DrawImage(imageSource, rect);
            }
        }

        internal Point GetPxPos(Point worldPoint)
        {
            return new Point
            {
                X = worldPoint.X * zoomPPS + pan.X,
                Y = worldPoint.Y * zoomPPS + pan.Y,
            };
        }

        internal void Add(Point relativeMVPoint)
        {
            Point worldPoint = GetWorldPos(relativeMVPoint);
            Point pointSampleTopLeft = GetPxPos(worldPoint);
            DrawingVisual visual = new DrawingVisual();
            DrawSquare(visual, pointSampleTopLeft, false);
            AddVisual(visual);
        }

        internal void Add(Point relativeMVPoint, ImageSource imageSource)
        {
            Point worldPoint = GetWorldPos(relativeMVPoint);
            Point pointSampleTopLeft = GetPxPos(worldPoint);
            DrawingVisual visual = new DrawingVisual();
            DrawImage(visual, imageSource, pointSampleTopLeft, false);
            AddVisual(visual);
        }
    }
}
