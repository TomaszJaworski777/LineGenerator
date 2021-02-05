using UnityEngine;

namespace City.Modules.Line
{
    public class LineGenerator : MonoBehaviour
    {
        [SerializeField] LineRenderer line;

        int linePointsCount;

        public void Initialize(int segmentsCount)
        {
            line.positionCount = segmentsCount;

            linePointsCount = segmentsCount;

            ResetAllLinePoints(segmentsCount);
        }

        private void ResetAllLinePoints(int segmentsCount)
        {
            for (int i = 0; i < segmentsCount; i++)
            {
                line.SetPosition(i, Vector3.zero);
            }
        }

        public void GenerateLine(Vector2 firstPoint, Vector2 secondPoint, float a = -0.4f)
        {
            float x1 = firstPoint.x;
            float y1 = firstPoint.y;

            float x2 = secondPoint.x;
            float y2 = secondPoint.y;

            x2 = ProtectFromNullValues(x1, x2);

            if (x1 > x2)
            {
                SwapValues(ref x1, ref y1, ref x2, ref y2);
            }

            float b = FactorB(a, x1, x2, y1, y2);
            float c = FactorC(a, x1, y1, b);

            SetLinePoints(a, x1, x2, b, c);
        }

        private float FactorC(float a, float x1, float y1, float b)
        {
            float sqr1 = x1 * x1;

            float c = y1 - a * sqr1 - x1 * b;

            return c;
        }

        private float FactorB(float a, float x1, float x2, float y1, float y2)
        {
            float sqr1 = x1 * x1;
            float sqr2 = x2 * x2;

            float b = (a * (sqr2 - sqr1) + y1 - y2) / (x1 - x2);

            return b;
        }

        float CalculateFunction(float x, float a, float b, float c)
        {
            float sqr = x * x;

            return a * sqr + b * x + c;
        }

        private void SwapValues(ref float x1, ref float y1, ref float x2, ref float y2)
        {
            float buffer = x2;

            x2 = x1;
            x1 = buffer;

            buffer = y2;

            y2 = y1;
            y1 = buffer;
        }

        private static float ProtectFromNullValues(float x1, float x2)
        {
            if (x1 == x2)
            {
                x2 += 0.01f;
            }

            return x2;
        }

        private void SetLinePoints(float a, float x1, float x2, float b, float c)
        {
            for (int i = 0; i < linePointsCount; i++)
            {
                float x = (Mathf.Abs(x2 - x1) / (linePointsCount - 1)) * i + x1;
                float y = CalculateFunction(x, a, b, c);

                line.SetPosition(i, new Vector3(x, y, 0));
            }
        }
    }

}