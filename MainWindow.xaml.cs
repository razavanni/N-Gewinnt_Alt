﻿using System;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace N_Gewinnt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Chip currentChip;

        private int n { get; set; }
        public int cols { get; set; }
        public int rows { get; set; }
        double paddingX = 20;
        double paddingY = 200;
        private bool isAnimating = false;
        private int currentPlayer = 1;
        private int[] usedSpaces { get; set; }
        private double chipDia { get; set; }

        private double boardheight = 0;

        private int[,] playerPlace { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            StartDialog dlg = new StartDialog();

            if ((bool)dlg.ShowDialog())
            {
                n = dlg.GetN();
                cols = dlg.GetColumns();
                rows = dlg.GetRows();
                currentChip = new Chip(0);
                double cellWidth = (SystemParameters.PrimaryScreenWidth * 0.7 - paddingX) / dlg.GetColumns();
                DrawChip(0, cellWidth, 0);

                DrawGameBoard(dlg.GetColumns(), dlg.GetRows());
                usedSpaces = new int[cols];
                playerPlace = new int[cols, rows];
                foreach (int col in usedSpaces)
                {
                    usedSpaces[col] = 0;
                }
                foreach (int row in playerPlace)
                {
                    foreach (int col in playerPlace)
                    {
                        playerPlace[col, row] = 0;
                    }
                }
            }
            else
            {
                Close();
            }
        }

        // Draw Chip ready
        private void DrawChip(double column, double cellWidth, int usedSpacesInColumn)
        {
            if (currentChip != null)
            {
                double chipDiameter = cellWidth;
                chipDia = cellWidth;

                currentChip.ChipEllipse.Width = chipDiameter;
                currentChip.ChipEllipse.Height = chipDiameter;

                double chipX = column * cellWidth + (cellWidth - chipDiameter) / 2 + paddingX;


                Canvas.SetLeft(currentChip.ChipEllipse, chipX);
                Canvas.SetTop(currentChip.ChipEllipse, 0);

                Cvs.Children.Add(currentChip.ChipEllipse);
            }
        }
        // Draw Game Board
        private void DrawGameBoard(int columns, int rows)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            Cvs.Width = screenWidth;
            Cvs.Height = screenHeight;

            double cellWidth = (screenWidth * 0.7 - paddingX) / columns;
            double maxBoardHeight = screenHeight * 0.7;

            double boardHeight = rows * chipDia;
            boardheight = boardHeight;

            if (boardHeight > maxBoardHeight)
            {
                boardHeight = maxBoardHeight;
                boardheight = maxBoardHeight;
            }

            // Zeichnen der vertikalen Linien zwischen den Spalten
            for (int column = 0; column <= columns; column++) // Beachten Sie den <= Operator
            {
                Line verticalLine = new Line
                {
                    X1 = paddingX + column * cellWidth,
                    Y1 = paddingY,
                    X2 = paddingX + column * cellWidth,
                    // Y2 = screenHeight * 0.7,
                    Y2 = paddingY + boardHeight,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };

                Cvs.Children.Add(verticalLine);
            }

            // Zeichnen der horizontalen Linie am unteren Rand
            Line horizontalLine = new Line
            {
                X1 = paddingX,
                Y1 = paddingY + boardHeight,
                X2 = paddingX + columns * cellWidth, // Verwenden Sie hier columns
                Y2 = paddingY + boardHeight,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            Cvs.Children.Add(horizontalLine);
        }
        // Window Key Down
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (currentChip != null)
            {
                if (!isAnimating)
                {
                    if (e.Key == Key.Left)
                    {
                        // Bewegen Sie den Chip nach links (zur vorherigen Spalte)
                        MoveCurrentChipLeft();
                    }
                    else if (e.Key == Key.Right)
                    {
                        // Bewegen Sie den Chip nach rechts (zur nächsten Spalte)
                        MoveCurrentChipRight();
                    }
                    else if (e.Key == Key.Down)
                    {
                        int column = currentChip.Column;
                        if (usedSpaces[column] < rows)
                        {
                            isAnimating = true;
                            MoveCurrentChipDown();
                        }
                    }
                }
            }
        }
        // Move left
        private void MoveCurrentChipLeft()
        {
            if (currentChip != null)
            {
                if (currentChip.Column > 0)
                {
                    currentChip.Column--;
                    double cellWidth = (SystemParameters.PrimaryScreenWidth * 0.7 - paddingX) / cols;
                    double chipX = currentChip.Column * cellWidth + (cellWidth - currentChip.ChipEllipse.Width) / 2 + paddingX;
                    Canvas.SetLeft(currentChip.ChipEllipse, chipX);
                }
            }
        }
        // Move Rigjt
        private void MoveCurrentChipRight()
        {
            if (currentChip != null)
            {
                if (currentChip.Column < cols - 1)
                {
                    currentChip.Column++;
                    double cellWidth = (SystemParameters.PrimaryScreenWidth * 0.7 - paddingX) / cols;
                    double chipX = currentChip.Column * cellWidth + (cellWidth - currentChip.ChipEllipse.Width) / 2 + paddingX;
                    Canvas.SetLeft(currentChip.ChipEllipse, chipX);
                }
            }
        }
        // Move down
        private void MoveCurrentChipDown()
        {
            double screenHeight = SystemParameters.PrimaryScreenHeight * 0.7;
            double chipHeight = currentChip.ChipEllipse.Height;

            int targetRow = 0;


            for (int roww = 0; roww < rows; roww++)
            {
                double rowTop = paddingY + roww * (screenHeight * 0.7) / rows;
                if (rowTop > Canvas.GetTop(currentChip.ChipEllipse))
                {
                    break;
                }
                targetRow = roww;
            }
            // foo
            int column = currentChip.Column;
            int row = currentChip.Row;
            playerPlace[column, row] = currentPlayer;
            usedSpaces[column]++;

            DoubleAnimation animation = new DoubleAnimation
            {
                From = Canvas.GetTop(currentChip.ChipEllipse),
                To = paddingY + boardheight - chipHeight * usedSpaces[column],
                Duration = TimeSpan.FromSeconds(0.5)
            };

            animation.Completed += (sender, e) =>
            {
                isAnimating = false;

                if (Canvas.GetTop(currentChip.ChipEllipse) >= screenHeight - chipHeight * usedSpaces[column])
                {
                    checkWin(currentPlayer);
                    NewBall();

                }
            };

            currentChip.ChipEllipse.BeginAnimation(Canvas.TopProperty, animation);
        }
        // New ball 
        private void NewBall()
        {
            currentChip = new Chip(0);

            if (currentPlayer == 1)
            {
                currentChip.ChipEllipse.Fill = Brushes.Blue;
                label_player.Content = "Spieler Blau ist dran";
                playerPlace[currentChip.Column, currentChip.Row] = currentPlayer;
                currentPlayer = 2;

            }
            else if (currentPlayer == 2)
            {
                currentChip.ChipEllipse.Fill = Brushes.Red;
                label_player.Content = "Spieler Rot ist dran";
                playerPlace[currentChip.Column, currentChip.Row] = currentPlayer;
                currentPlayer = 1;
            }

            double cellWidth = (SystemParameters.PrimaryScreenWidth * 0.7 - paddingX) / cols;
            int column = currentChip.Column;

            DrawChip(column, cellWidth, 0); // Passen Sie die Methode an, um usedSpaces zu berücksichtigen
            isAnimating = false;
        }

        private bool checkWin(int player)
        {
            for (int i = 0; i < cols; i++)
            {
                bool rowWin = true;
                for (int j = 0; j < n; j++)
                {
                    if (playerPlace[i, j] != player)
                    {
                        rowWin = false;
                        break;
                    }
                }
                if (rowWin)
                {
                    MessageBox.Show("Gewinn");
                    return true;
                }

            }

            for (int j = 0; j < rows; j++)
            {
                bool columnWin = true;
                for (int i = 0; i < n; i++)
                {
                    if (playerPlace[i, j] != player)
                    {
                        columnWin = false;
                        break;
                    }
                }
                if (columnWin)
                {
                    MessageBox.Show("Gewinn");
                    return true;
                }
            }


            return false;
        }


       
    }
}