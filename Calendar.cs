using System;

namespace Atlas
{
    static class Calendar
    {
        enum TimelineLayer
        {
            NONE,
            YEAR,
            MONTH,
            DAY
        }
        private static TimelineLayer currentLayer = TimelineLayer.NONE;
        private static TimelineLayer CurrentLayer
        {
            get => currentLayer;
            set
            {
                if (currentLayer != value)
                {
                    LoadItems(value, currentLayer == TimelineLayer.NONE ? DateTime.Today.Date : items[position]);
                    currentLayer = value;
                }
            }
        }
        private static DateTime[] items;
        private static int position;
        public static int Position
        {
            get => position;
            set
            {
                if (value >= 0 && value < items.Length)
                {
                    position = value;
                    Show(CurrentLayer);
                }
            }
        }

        static void LoadItems(TimelineLayer layer, DateTime source)
        {
            items = new DateTime[layer == TimelineLayer.YEAR ? 1000 : (layer == TimelineLayer.MONTH ? 12 : DateTime.DaysInMonth(source.Year, source.Month))];
        }

        public static void Launch()
        {
            CurrentLayer = TimelineLayer.YEAR;

            while (true)
            {

            }

            currentLayer = TimelineLayer.NONE;
        }
        static void Show(TimelineLayer layer)
        {

        }
    }
}