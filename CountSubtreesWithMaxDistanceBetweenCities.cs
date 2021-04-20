/**
LeetCode's hard challenge Count Subtrees With Max Distance Between Cities

Site: https://leetcode.com/problems/count-subtrees-with-max-distance-between-cities/

There are n cities numbered from 1 to n. 

You are given an array edges of size n-1, where edges[i] = [ui, vi] represents a bidirectional edge between cities ui and vi.

There exists a unique path between each pair of cities. In other words, the cities form a tree.

A subtree is a subset of cities where every city is reachable from every other city in the subset, 
where the path between each pair passes through only the cities from the subset. 

Two subtrees are different if there is a city in one subtree that is not present in the other.

For each d from 1 to n-1, find the number of subtrees in which the maximum distance between any two cities in the subtree is equal to d.

Return an array of size n-1 where the dth element (1-indexed) is the number of subtrees in which the maximum distance between any two cities is equal to d.

Notice that the distance between the two cities is the number of edges in the path between them.

Example:
Input: n = 4, edges = [[1,2],[2,3],[2,4]]
Output: [3,4,0]
Explanation:
The subtrees with subsets {1,2}, {2,3} and {2,4} have a max distance of 1.
The subtrees with subsets {1,2,3}, {1,2,4}, {2,3,4} and {1,2,3,4} have a max distance of 2.
No subtree has two nodes where the max distance between them is 3.
---------

My solution uses Floyd-Warshall Algorithm to find all the distances.
Then, it brute-forces all the possible combinations of cities through recursion and back-tracking with a "subset" LinkedList to count the diameters.

Author: Lucas Paz.
*/

public class Solution
{
    private int[] numberOfSubGraphs;

    private int n;
    
    private int[ , ] distancesMatrix;
    
    private const int INFINITY = 999;
    
    private LinkedList<int> subsetOfCities;
    
    public int[] CountSubgraphsForEachDiameter(int n, int[][] edges) 
    {
        numberOfSubGraphs = new int[n - 1]; // 1-indexed
        
        distancesMatrix = new int[n + 1, n + 1];
        
        foreach (int[] edge in edges)
        {
            int city = edge[0], otherCity = edge[1];
            distancesMatrix[city, otherCity] = 1;
            distancesMatrix[otherCity, city] = 1;
        }
        
        this.n = n;
        
        FindAllDistancesByFloydWarshallAlgorithm();
        
        subsetOfCities = new LinkedList<int>();
        
        for (int numberOfCities = 2; numberOfCities <= n; numberOfCities++)
            BruteForceAllCombinations(1, numberOfCities);
        
        return numberOfSubGraphs;
    }
    
    private void FindAllDistancesByFloydWarshallAlgorithm()
    {   
        for (int row = 1; row < distancesMatrix.GetLength(0); row++)
            for (int column = 1; column < distancesMatrix.GetLength(1); column++)
                if (row != column && distancesMatrix[row, column] == 0)
                    distancesMatrix[row, column] = INFINITY;
        
        for (int k = 1; k <= n; k++)
            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= n; j++)
                    if (distancesMatrix[i, k] + distancesMatrix[k, j] < distancesMatrix[i, j])
                        distancesMatrix[i, j] = distancesMatrix[i, k] + distancesMatrix[k, j];
    }
    
    private void BruteForceAllCombinations(int start, int numberOfCities)
    {
        if (numberOfCities == 1)
        {
            for (int i = start; i <= n; i++)
            {
                subsetOfCities.AddLast(i);
                calculateSubset();
                subsetOfCities.RemoveLast();
            }
            return;
        }
        
        for (int i = start; i + numberOfCities - 1 <= n; i++)
        {
            subsetOfCities.AddLast(i);
            BruteForceAllCombinations(i + 1, numberOfCities - 1);
            subsetOfCities.RemoveLast();
        }
    }
    
    private void calculateSubset()
    {
        int maxDistance = 1;
        int numberOfEdges = 0;

        foreach (int city in subsetOfCities)
            foreach (int otherCity in subsetOfCities)
            {
                if (distancesMatrix[city, otherCity] == 1) numberOfEdges++;
                maxDistance = Math.Max(maxDistance, distancesMatrix[city, otherCity]);
            }

        int numberOfCities = subsetOfCities.Count;
        if (numberOfEdges / 2 != numberOfCities - 1) return; // if cities are not linked

        int oneBasedIndex = maxDistance - 1;
        
        numberOfSubGraphs[oneBasedIndex]++;
    }
}
