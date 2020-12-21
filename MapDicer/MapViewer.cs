using MapDicer.Models;
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

        public void RemoveVisual(Visual visual)
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

        /// <summary>
        /// Get the pixel point of the top left corner of the tile relative to the MapViewer.
        /// </summary>
        /// <param name="worldPoint">A point containing the X and Z (as Y) from a MapDicerPos</param>
        /// <returns></returns>
        internal Point GetPxPos(Point worldPoint)
        {
            return new Point
            {
                X = worldPoint.X * zoomPPS + pan.X,
                Y = worldPoint.Y * zoomPPS + pan.Y,
            };
        }
        internal Point GetPxPos(MapDicerPos microPos)
        {
            return GetPxPos(new Point(microPos.X, microPos.Z));
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

        private bool IsNewWrite(MapDicerPos microPos, bool markAsWritten)
        {
            bool changed = false;
            if (microPos.X != lastDrawnPos.X)
                changed = true;
            else if (microPos.Z != lastDrawnPos.Z)
                changed = true;
            else if (microPos.LodId != lastDrawnPos.LodId)
                changed = true;
            else if (microPos.LayerId != lastDrawnPos.LayerId)
                changed = true;
            if (changed && markAsWritten)
            {
                MarkAsWritten(microPos);
            }
            return changed;
        }

        public bool IsNewWrite(MapDicerPos microPos)
        {
            if (isNewDatabase)
                return true;
            bool changed = false;
            if (microPos.X != lastDrawnPos.X)
                changed = true;
            else if (microPos.Z != lastDrawnPos.Z)
                changed = true;
            else if (microPos.LodId != lastDrawnPos.LodId)
                changed = true;
            else if (microPos.LayerId != lastDrawnPos.LayerId)
                changed = true;
            return changed;
        }
        public void MarkAsWritten(MapDicerPos microPos)
        {
            isNewDatabase = false;
            lastDrawnPos.LodId = microPos.LodId;
            lastDrawnPos.LayerId = microPos.LayerId;
            lastDrawnPos.X = microPos.X;
            lastDrawnPos.Z = microPos.Z;
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

        internal void Add(Point relativeMVPoint)
        {
            Point worldPoint = GetWorldPos(relativeMVPoint);
            Point pointSampleTopLeft = GetPxPos(worldPoint);
            DrawingVisual visual = new DrawingVisual();
            DrawSquare(visual, pointSampleTopLeft, false);
            AddVisual(visual);
        }

        internal Visual Add(Point relativeMVPoint, ImageSource imageSource)
        {
            Point worldPoint = GetWorldPos(relativeMVPoint);
            Point pointSampleTopLeft = GetPxPos(worldPoint);
            DrawingVisual visual = new DrawingVisual();
            DrawImage(visual, imageSource, pointSampleTopLeft, false);
            AddVisual(visual);
            return visual;
        }

        internal static WriteableBitmap NewWriteableBitmap(Lod lod)
        {
            double dpi = SettingController.DpiForNonViewableData;
            int width = lod.SamplesPerMapblock;
            int height = lod.SamplesPerMapblock;
            WriteableBitmap wb = new WriteableBitmap(width, height, dpi, dpi, PixelFormats.Bgra32, null);
            if ((wb.PixelWidth != width) || (wb.PixelHeight != height))
                throw new ApplicationException(String.Format("Tried to get {0}x{1} image and got {2}x{3}", width, height, wb.PixelWidth, wb.PixelHeight));
            return wb;
        }
        /*
        internal static ByteMap GetByteMap(Lod lod, Color color, bool fill)
        {
            int width = lod.SamplesPerMapblock;
            int height = lod.SamplesPerMapblock;
            return new ByteMap(width, height, 4, fill, color);
        }
        */
    }
}
