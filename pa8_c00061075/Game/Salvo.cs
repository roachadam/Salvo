using pa8_c00061075.Enums;
using pa8_c00061075.Models;
using System;
using System.Collections.Generic;

namespace pa8_c00061075.Game
{
    class Salvo
    {
        private Random _random;
        public List<Ship> Ships { get; }

        public Salvo()
        {
            _random = new Random(Guid.NewGuid().GetHashCode());
            Ships = new List<Ship>();

            GenerateShipLocations();
        }

        private void GenerateShipLocations()
        {
            if (Ships.Count > 0)
                return;


            Ship yourShip = GetRandomShip();
            Ship opponentShip = GetRandomShip();

            Ships.Add(yourShip);
            Ships.Add(opponentShip);

        }

        private Ship GetRandomShip()
        {
            ShipOrientation myOrientation = ChooseShipOrientation();

            int startX = _random.Next(0, 5); // anchor position
            int startY = _random.Next(0, 5); // anchor position


            List<Coordinates> myCoords = new List<Coordinates>();


            if (myOrientation == ShipOrientation.Vertical)
            {
                int temp = startX;
                // Must go down
                if (temp < 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Coordinates co = new Coordinates(temp, startY);
                        myCoords.Add(co);
                        temp++;
                    }
                }
                else if (temp == 2)
                {
                    // Random, can do either direction
                    bool up = _random.Next(0, 2) == 1;
                    for (int i = 0; i < 3; i++)
                    {
                        Coordinates co = new Coordinates(temp, startY);
                        myCoords.Add(co);
                        if (up)
                            temp--;
                        else
                            temp++;
                    }
                }
                else if (temp > 2)
                {
                    // Must go up
                    for (int i = 0; i < 3; i++)
                    {
                        Coordinates co = new Coordinates(temp, startY);
                        myCoords.Add(co);
                        temp--;
                    }

                }
            }
            else if(myOrientation == ShipOrientation.Horizontal)
            {
                int temp = startY;
                // Must go right
                if (temp < 2)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Coordinates co = new Coordinates(startX, temp);
                        myCoords.Add(co);
                        temp++;
                    }
                }
                else if (temp == 2)
                {
                    // Random, can do either direction
                    bool up = _random.Next(0, 2) == 1;
                    for (int i = 0; i < 3; i++)
                    {
                        Coordinates co = new Coordinates(startX, temp);
                        myCoords.Add(co);
                        if (up)
                            temp--;
                        else
                            temp++;
                    }
                }
                else if (temp > 2)
                {
                    // Must go left
                    for (int i = 0; i < 3; i++)
                    {
                        Coordinates co = new Coordinates(startX, temp);
                        myCoords.Add(co);
                        temp--;
                    }
                }
            }

            return new Ship()
            {
                Location = new Location(myCoords)
            };
        }

        private bool IsLeftCorner(int x, int y)
        {
            return (x == 0 && y == 0) || (x == 4 && y == 0);
        }

        private bool IsRightCorner(int x, int y)
        {
            return (x == 0 && y == 4) || (x == 4 && y == 4);
        }

        private ShipOrientation ChooseShipOrientation()
        {
            if (_random.Next(0, 2) == 0)
                return ShipOrientation.Horizontal;
            return ShipOrientation.Vertical;
        }
    }
}
