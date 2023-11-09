using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace N_Gewinnt
{
    internal class Chip
    {
        public Ellipse ChipEllipse { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public int paddingX = 20;
        public int paddingY = 150;

        public Chip(int column)
        {
            Column = column;
            Row = 0;

            ChipEllipse = new Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
        }

    }
}
