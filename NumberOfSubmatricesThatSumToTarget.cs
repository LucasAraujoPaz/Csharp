/**
LeetCode's challenge Number Of Submatrices That Sum To Target:

Level: Hard.

Given a matrix and a target, return the number of non-empty submatrices that sum to target.

A submatrix x1, y1, x2, y2 is the set of all cells matrix[x][y] with x1 <= x <= x2 and y1 <= y <= y2.

Two submatrices (x1, y1, x2, y2) and (x1', y1', x2', y2') are different if they have some coordinate that is different: for example, if x1 != x1'.

Example:
Input: matrix = [[1,-1],
                 [-1,1]] 
       target = 0
Output: 5
Explanation: The two 1x2 submatrices, plus the two 2x1 submatrices, plus the 2x2 submatrix.

Constraints:
1 <= matrix.length <= 100
1 <= matrix[0].length <= 100
-1000 <= matrix[i] <= 1000
-10^8 <= target <= 10^8

Available at: https://leetcode.com/problems/number-of-submatrices-that-sum-to-target/ (Access in 17/04/2021)

This solution beats 100% in memory.
Author of solution: Lucas Paz.
*/
public class Solution
{
    public int NumSubmatrixSumTarget(int[][] matrix, int target)
    {
        int numberOfSubmatricesThatSumToTarget = 0;

        if (matrix == null || matrix.Length == 0 || matrix[0].Length == 0
            || target < -1E7 || target > 1E7) return 0;

        int m = matrix.Length, n = matrix[0].Length;

        if (m == 1 && n == 1) return (matrix[0][0] == target) ? 1 : 0;

        IDictionary<int, int> preffixSumsAndCounters = new Dictionary<int, int>();

        transformSimpleMatrixIntoRowPreffixSumsMatrix(matrix);

        for (int upperRow = 0; upperRow < m; upperRow++)
        {
            for (int row = upperRow; row < m; row++)
            {
                preffixSumsAndCounters.Clear();
                preffixSumsAndCounters.Add(0, 1);

                for (int column = 0; column < n; column++)
                {
                    int thisPreffixSum = matrix[row][column];

                    //go to rows above adding their preffix sums
                    for (int remainingRow = row - 1; remainingRow >= upperRow; remainingRow--)
                        thisPreffixSum += matrix[remainingRow][column];

                    //(thisPreffix - desirablePreffixSum) must be equal to target:
                    //thisPreffix - desirablePreffixSum = target.
                    //Hence: desirablePreffixSum = thisPreffixSum - target;
                    int desirablePreffixSum = thisPreffixSum - target;

                    if (preffixSumsAndCounters.ContainsKey(desirablePreffixSum))
                        numberOfSubmatricesThatSumToTarget += preffixSumsAndCounters[desirablePreffixSum];

                    if (preffixSumsAndCounters.ContainsKey(thisPreffixSum))
                        preffixSumsAndCounters[thisPreffixSum]++;
                    else
                        preffixSumsAndCounters.Add(thisPreffixSum, 1);
                }
            }
        }

            return numberOfSubmatricesThatSumToTarget;
        }

        private void transformSimpleMatrixIntoRowPreffixSumsMatrix(int[][] matrix)
        {
            int m = matrix.Length, n = matrix[0].Length;

            for (int row = 0; row < m; row++)
                for (int column = 1; column < n; column++)
                    matrix[row][column] += matrix[row][column - 1];
        }
}
