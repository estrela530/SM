using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMGame
{
    static class Collision
    {

        public static bool RotateRectangleCollision2(
            Vector2 legTop, Vector2 legDown,
            Vector2 playerLeft, Vector2 playerRight,
            float legAngle, Vector2 center,
            int RL)
        {
            //RL =>右判定:1左判定:2
            Vector2 legTopR = new Vector2(
                (float)((center.X - legTop.X) * Math.Cos(legAngle) - (center.Y - legTop.Y) * Math.Sin(legAngle)) + center.X,
                (float)((center.X - legTop.X) * Math.Sin(legAngle) + (center.Y - legTop.Y) * Math.Cos(legAngle)) + center.Y);
            Vector2 legDownR = new Vector2(
                (float)((center.X - legDown.X) * Math.Cos(legAngle) - (center.Y - legDown.Y) * Math.Sin(legAngle)) + center.X,
                (float)((center.X - legDown.X) * Math.Sin(legAngle) + (center.Y - legDown.Y) * Math.Cos(legAngle)) + center.Y);
            //ベクトル正規化
            float legVecX = legDownR.X - legTopR.X;
            float legVecY = legDownR.Y - legTopR.Y;

            float legVecLength = (float)Math.Sqrt(legVecX * legVecX + legVecY * legVecY);
            float normalizeVecX = legVecX / legVecLength;
            float normalizeVecY = legVecY / legVecLength;


            //線からプレイヤーまでのベクトル
            float playerVecX = playerLeft.X - legTopR.X;
            float playerVecY = playerLeft.Y - legTopR.Y;

            //外積計算
            float cross = (playerVecX * normalizeVecY - normalizeVecX * playerVecY);
            if (cross < 0 && RL == 1)
            {
                return true;
            }
            if (cross > 0 && RL == 2)
            {
                return true;
            }
            return false;
        }
    }
}
