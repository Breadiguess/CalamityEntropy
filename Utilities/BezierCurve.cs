﻿using System.Collections.Generic;

namespace CalamityEntropy.Utilities
{
    public struct BaseBezierCurveInfo(List<Vector2> rawPosList, List<float> rawRotList)
    {
        public List<Vector2> CurvePositionList = rawPosList;
        public List<float> CurveRotationList = rawRotList;
    }

    public static class BezierCurveHandler
    {
        /// <summary>
        /// 直接获取二次贝塞尔曲线点的结构体
        /// </summary>
        /// <param name="rawPositionList"></param>
        /// <param name="rawRotationList"></param>
        /// <returns></returns>
        public static BaseBezierCurveInfo GetValidBeizerCurvePow(List<Vector2> rawPositionList, List<float> rawRotationList, int drawPointTime = 3)
        {
            List<Vector2> smoothPos = [];
            List<float> smoothRot = [];
            for (int i = 0; i < rawPositionList.Count - 1; i++)
            {
                Vector2 startPos = rawPositionList[i];
                Vector2 endPos = rawPositionList[i + 1];
                Vector2 controlPoint = CalculateControlPoint(rawPositionList, i);
                float rot0 = rawRotationList[i];
                float rot1 = rawRotationList[i + 1];
                //处理插值问题
                float normalRot1 = rot1;
                float deltaRot = rot1 - rot0;
                //标准化rot1避免突变
                if (deltaRot > MathHelper.Pi)
                    normalRot1 = rot0 + (deltaRot - MathHelper.TwoPi);
                else if (deltaRot < -MathHelper.Pi)
                    normalRot1 = rot0 + (deltaRot + MathHelper.TwoPi);
                for (int t = 0; t <= drawPointTime; t++)
                {
                    float progress = (float)t / drawPointTime;
                    Vector2 thePos = BezierCurve(startPos, controlPoint, endPos, progress);
                    float theRot = MathHelper.Lerp(rot0, normalRot1, progress);
                    //将其限制在0~2pi内
                    theRot = MathHelper.WrapAngle(theRot);
                    smoothPos.Add(thePos);
                    smoothRot.Add(theRot);
                }
            }
            return new BaseBezierCurveInfo(smoothPos, smoothRot);
        }
        /// <summary>
        /// 以out的形式获得一个贝塞尔曲线。
        /// </summary>
        /// <param name="rawPositionList"></param>
        /// <param name="rawRotationList"></param>
        /// <param name="smoothPosList"></param>
        /// <param name="smoothRotList"></param>
        /// <param name="drawPointTime"></param>
        public static void GetValidBeizerCurvePow(List<Vector2> rawPositionList, List<float> rawRotationList, out List<Vector2> smoothPosList, out List<float> smoothRotList, int drawPointTime = 3)
        {
            BaseBezierCurveInfo BezierCurve = GetValidBeizerCurvePow(rawPositionList, rawRotationList, drawPointTime);
            smoothPosList = BezierCurve.CurvePositionList;
            smoothRotList = BezierCurve.CurveRotationList;

        }
        private static Vector2 BezierCurve(Vector2 startPos, Vector2 controlPos, Vector2 endPos, float t)
        {
            float u = 1 - t;
            return u * u * startPos + 2 * u * t * controlPos + t * t * endPos;
        }

        private static Vector2 CalculateControlPoint(List<Vector2> points, int index)
        {
            if (index == 0)
            {
                //第一个点的控制点：使用下下个点方向
                Vector2 nextNext = points[index + 2];
                return points[index + 1] - (nextNext - points[index + 1]) * 0.25f;
            }
            else if (index == points.Count - 2)
            {
                //最后一个点的控制点：使用前前点方向
                Vector2 prevPrev = points[index - 1];
                return points[index] + (points[index] - prevPrev) * 0.25f;
            }
            else
            {
                //中间点的控制点：使用前后点的切线方向
                Vector2 prev = points[index - 1];
                Vector2 next = points[index + 2];
                return (points[index] + points[index + 1]) / 2f + (next - prev) * 0.1f;
            }
        }

    }
}